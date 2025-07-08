// FormPageSecond.cs – C# 7.3
using GF.Scheduling;
using GF.Utils;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using GF.Scheduling.IO;
using System.Management.Instrumentation;
using ClosedXML.Excel;
using ClosedXML.Graphics;
using static GF.Scheduling.IO.ScheduleFileService;
using DocumentFormat.OpenXml.Bibliography;

namespace GF.UI
{
    public partial class FormPageSecond : Form
    {
        /*–––– дані з PageOne ––––*/
        private readonly DataGridView _dispo;
        private readonly List<Employee> _employees;
        private int _month;                 // 1‑12

        /*–––– runtime‑поля ––––*/
        private Color _chosenColor = Color.Yellow;
        private readonly Dictionary<Point, Color> _cellBorders = new Dictionary<Point, Color>();
        private readonly HashSet<Point> _customFilled = new HashSet<Point>();
        private readonly List<List<CellMemento>> _undoStack = new List<List<CellMemento>>(5);

        // для динамічного аналізу після редагування
        private ScheduleParameters _lastParams;     // WorkersPerShift + часи
        private int _days;
        private int _totalsRow;
        private int _year;

        private readonly List<MonthContainer> _allContainers;   // ← НОВЕ

        public bool IsScheduleEmpty => dgvSchedule.Rows.Count == 0;

        // FormPageSecond.cs  (у приватних полях форми)
        private readonly Dictionary<Point, string> _cellComments = new Dictionary<Point, string>();   // X = ColumnIndex, Y = RowIndex

        private readonly Dictionary<Point, SwapInfo> _swapInfos
                = new Dictionary<Point, SwapInfo>();

        private Action<FormPageSecond, DataGridViewCell> _pickCallback;

        // додайте поруч з іншими runtime-полями
        private readonly HashSet<Point> _swappedCells = new HashSet<Point>();

        public string SourceFile { get; private set; }      // "May24.gfs" …

        private MonthContainer _container;

        public void Recalc() => RecalcConflictsAndHours();

        /* порожній ctor */
        public FormPageSecond()
        {
            InitializeComponent();
            InitRuntimeOnlyThings();
            dgvSchedule.CellPainting += DgvSchedule_CellPainting;
            dgvSchedule.CellDoubleClick += DgvSchedule_CellDoubleClick;
        }

        /* ctor із даними */
        public FormPageSecond(DataGridView dispo,
                      List<Employee> employees,
                      MonthContainer container,
                      List<MonthContainer> allContainers) : this()
        {
            _dispo = dispo;
            _employees = employees;
            _container = container;
            _allContainers = allContainers;          // ← запам’ятали

            _month = container.Month;
            _year = container.Year;
            FillPriorityPanel();
        }

        public void SetSourceFile(string path)
        {
            SourceFile = Path.GetFileName(path);
            Text = SourceFile;              // щоб заголовок картки = файл
        }

        public string GetEmployeeName(int col) =>
            dgvSchedule.Columns[col].HeaderText;

        public void MarkSwapped(int col, int row, SwapInfo info)
        {
            var key = new Point(col, row);
            _swappedCells.Add(key);       // кружок
            _swapInfos[key] = info;       // паспорт
            dgvSchedule.InvalidateCell(col, row);
        }

        public void EnablePickMode(Action<FormPageSecond, DataGridViewCell> cb)
        {
            _pickCallback = cb;
            dgvSchedule.CellClick += DgvSchedule_PickClick;
        }

        public void DisablePickMode()
        {
            dgvSchedule.CellClick -= DgvSchedule_PickClick;
            _pickCallback = null;
        }

        private void DgvSchedule_PickClick(object s,
                                           DataGridViewCellEventArgs e)
        {
            if (_pickCallback == null || e.RowIndex < 1 || e.ColumnIndex < 1)
                return;

            var cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
            _pickCallback(this, cell);
        }

        public static class MyMessageBox
        {
            /* ---------- MessageBox-аналог ---------- */

            public static DialogResult Show(
                string text,
                string caption = "Message",
                MessageBoxButtons buttons = MessageBoxButtons.OK,
                IWin32Window owner = null)
            {
                DialogResult result;

                using (var dlg = new MessangeForm(text, buttons))   // жодної іконки
                {
                    dlg.Text = caption;

                    result = owner == null
                           ? dlg.ShowDialog()
                           : dlg.ShowDialog(owner);
                }

                return result;
            }

            /* ---------- InputBox-аналог ---------- */

            /// <summary>
            ///  Показує діалог із полем введення.
            ///  Повертає введений текст або null, якщо натиснуто Cancel / Esc.
            /// </summary>
            public static string Input(
                string placeholder = "",          // ← новий параметр
                string caption = "Input",
                string defaultText = "",
                IWin32Window owner = null)
            {
                string userValue = null;

                using (var dlg = new MessangeForm(
                                    placeholder,                 // ← йде в PlaceholderText
                                    MessageBoxButtons.OKCancel,
                                    askInput: true,
                                    defaultInput: defaultText))
                {
                    dlg.Text = caption;

                    DialogResult dr = owner == null
                                    ? dlg.ShowDialog()
                                    : dlg.ShowDialog(owner);

                    if (dr == DialogResult.OK)
                        userValue = dlg.UserInput;
                }

                return userValue;       // null, якщо Cancel
            }
        }


        private void DgvSchedule_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var key = new Point(e.ColumnIndex, e.RowIndex);

            if (_swapInfos.TryGetValue(key, out var s))
            {
                string left = $"({s.FromFile}, {s.Day}, {s.FromEmployee}, {s.FromShift ?? "—"})";
                string right = $"({s.ToFile}, {s.Day}, {s.ToEmployee}, {s.ToShift ?? "—"})";

                MyMessageBox.Show($"{left}  -->  {right}\n\nTime: {s.When:g}",
                                  "Shift swap", owner: this);
                return;
            }

            if (!_cellComments.TryGetValue(key, out string text)) return;     // нема коментаря

            if (MyMessageBox.Show(
                text + "\n\n | Delete this comment?",
                "Comment",
                MessageBoxButtons.YesNo,
                this) == DialogResult.Yes)
            {
                _cellComments.Remove(key);
                dgvSchedule.InvalidateCell(e.ColumnIndex, e.RowIndex);
            }
        }



        /*──────────────────── runtime init ───────────────────*/
        private void InitRuntimeOnlyThings()
        {
            /* NumericUpDown */
            numWorkers.BorderRadius = 10;
            numWorkers.Minimum = 1;
            numWorkers.Maximum = 10;
            numWorkers.Value = 2;

            /* TimePickers */
            foreach (var p in new[] { dtMorning, dtAfternoon, dtEvening })
            {
                p.Format = DateTimePickerFormat.Time;
                p.ShowUpDown = true;
                p.BorderRadius = 10;
            }
            dtMorning.Value = DateTime.Today.AddHours(9);
            dtAfternoon.Value = DateTime.Today.AddHours(15);
            dtEvening.Value = DateTime.Today.AddHours(21);

            /* Кнопки */
            btnCreateSchedule.Click += BtnCreateSchedule_Click;
        }

        /*──── чек‑бокси пріоритету ────*/
        private void FillPriorityPanel()
        {
            flpPriority.Controls.Clear();
            if (_employees == null) return;

            foreach (var emp in _employees)
            {
                var cb = new Guna2CheckBox
                {
                    Text = emp.Name,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9f)
                };
                cb.CheckedState.FillColor = Color.SeaGreen;
                cb.UncheckedState.BorderColor = Color.Gray;
                flpPriority.Controls.Add(cb);
            }
        }

        private bool IsPriority(string employeeName)
        {
            return flpPriority.Controls.OfType<Guna2CheckBox>()
                              .Any(cb => cb.Text == employeeName && cb.Checked);
        }

        /*════════════  Generate  ════════════*/
        private void BtnCreateSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                if (_employees == null || _dispo == null)
                {
                    using (var dlg = new MessangeForm("Open or create a disposition first."))
                    {
                        dlg.StartPosition = FormStartPosition.CenterParent; // щоб вирівнялось по центру
                        dlg.ShowDialog(this);   // this—батьківська форма; можна без параметра
                    }
                    return;
                }

                /* позначаємо пріоритет */
                foreach (var emp in _employees)
                    emp.IsFullTime = IsPriority(emp.Name);

                var p = new ScheduleParameters
                {
                    WorkersPerShift = (int)numWorkers.Value,
                    MorningStart = dtMorning.Value.TimeOfDay,
                    AfternoonStart = dtAfternoon.Value.TimeOfDay,
                    EveningEnd = dtEvening.Value.TimeOfDay
                };

                // зберігаємо для подальшого динамічного перерахунку
                _lastParams = p;

                var result = ScheduleGenerator.Generate(_employees, _dispo, p);
                if (result == null || result.Table.Rows.Count == 0)
                {
                    MyMessageBox.Show("No schedule generated. Check your parameters and disposition.",
                                      "Generation failed", MessageBoxButtons.OK, owner: this);
                    return;
                }
                ShowResultInGrid(result);

                if (_container != null)
                {
                    _container.Schedules.Add(BuildSavedSchedule(
                    $"auto_{DateTime.Now:yyyyMMdd_HHmmss}"));

                    // зберігаємо весь список (у т. ч. оновлений поточний контейнер)
                    ContainerStorage.Save(_allContainers);           // перезаписуємо
                }


                guna2GroupBox6.Visible = false;
                guna2GroupBox2.Visible = true;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("Error: " + ex.Message, "Generation failed", MessageBoxButtons.OK, owner: this);
            }
        }

        /*══════════  відображення результату  ══════════*/
        private void ShowResultInGrid(ScheduleResult res)
        {
            dgvSchedule.CellEndEdit -= DgvSchedule_CellEndEdit; // уникнути подвійного підпису

            dgvSchedule.Columns.Clear();
            dgvSchedule.Rows.Clear();

            /* 1. перший стовпець – дати */
            var dayCol = new DataGridViewTextBoxColumn
            {
                HeaderText = string.Empty,
                Width = 60,
                ReadOnly = true
            };
            dayCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dayCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dayCol.DefaultCellStyle.Padding = new Padding(2, 0, 0, 0);
            dgvSchedule.Columns.Add(dayCol);

            /* 2. перший рядок – імена */
            dgvSchedule.Rows.Add(string.Empty);

            /* 3. дні місяця */
            _year = DateTime.Now.Year;   // якщо потрібен інший — передайте сюди
            _days = DateTime.DaysInMonth(_year, _month);

            for (int d = 1; d <= _days; d++)
            {
                string label = string.Format("{0}, {1}.", d, DateUtils.GetDayAbbr(_year, _month, d));
                int r = dgvSchedule.Rows.Add(label);

                var dow = new DateTime(_year, _month, d).DayOfWeek;
                if (dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday)
                    dgvSchedule.Rows[r].DefaultCellStyle.BackColor = Color.LightGray;
            }

            /* 4. рядок Hours */
            _totalsRow = dgvSchedule.Rows.Add("Hours");
            dgvSchedule.Rows[_totalsRow].DefaultCellStyle.BackColor = Color.LightYellow;

            /* 5. заповнення */
            foreach (DataRow row in res.Table.Rows)
            {
                string emp = row["Employee"].ToString();
                int col = GetOrAddEmployeeColumn(emp);

                dgvSchedule.Rows[0].Cells[col].Value = emp;
                dgvSchedule.Rows[_totalsRow].Cells[col].Value = row["Hours"];

                for (int d = 1; d <= _days; d++)
                    dgvSchedule.Rows[d].Cells[col].Value = row[d + 1]?.ToString();
            }

            /* 6. конфлікти */
            foreach (int day in res.ConflictDays)
                for (int c = 1; c < dgvSchedule.Columns.Count; c++)
                    dgvSchedule.Rows[day].Cells[c].Style.BackColor = Color.Orange;

            MarkUnavailableWithRedBorder();

            // динамічний аналіз при ручному редагуванні
            dgvSchedule.CellEndEdit += DgvSchedule_CellEndEdit;
            RecalcConflictsAndHours();
        }

        /*―――――― допоміжні методи гріду ――――――*/
        private int GetOrAddEmployeeColumn(string name)
        {
            for (int c = 1; c < dgvSchedule.Columns.Count; c++)
                if (dgvSchedule.Columns[c].HeaderText == name)
                    return c;

            var col = new DataGridViewTextBoxColumn
            {
                HeaderText = name,
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvSchedule.Columns.Add(col);
            return dgvSchedule.Columns.Count - 1;
        }

        /*―――――― динамічний перерахунок ――――――*/
        private void DgvSchedule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex == _totalsRow) return;
            RecalcConflictsAndHours();
        }

        private void RecalcConflictsAndHours()
        {
            if (_lastParams == null) return;

            /* 1) години */
            for (int col = 1; col < dgvSchedule.Columns.Count; col++)
            {
                int minutes = 0;

                for (int row = 1; row <= _days; row++)
                {
                    string mark = dgvSchedule.Rows[row].Cells[col].Value?.ToString()?.Trim();
                    if (mark == "9-15" || mark == "15-21") minutes += 6 * 60;
                    else if (mark == "9-21") minutes += 12 * 60;
                }
                dgvSchedule.Rows[_totalsRow].Cells[col].Value = minutes / 60;
            }

            /* 2) конфлікти */
            for (int day = 1; day <= _days; day++)
            {
                int morning = 0, evening = 0;

                for (int col = 1; col < dgvSchedule.Columns.Count; col++)
                {
                    string mark = dgvSchedule.Rows[day].Cells[col].Value?.ToString()?.Trim();
                    if (mark == "9-15") morning++;
                    else if (mark == "15-21") evening++;
                    else if (mark == "9-21") { morning++; evening++; }
                }

                bool conflict = morning < _lastParams.WorkersPerShift || evening < _lastParams.WorkersPerShift;

                var dow = new DateTime(_year, _month, day).DayOfWeek;
                Color baseCol = (dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday) ? Color.LightGray : Color.White;

                for (int col = 1; col < dgvSchedule.Columns.Count; col++) {

                    var key = new Point(col, day);
                    if (_customFilled.Contains(key)) continue;
                    dgvSchedule.Rows[day].Cells[col].Style.BackColor = conflict ? Color.Orange : baseCol;
                }
                
            }
        }

        /*―――――― форматування кнопками ――――――*/
        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
                if (dlg.ShowDialog() == DialogResult.OK)
                    _chosenColor = dlg.Color;
            btnPickColor.FillColor = _chosenColor;
        }

        private void btnFillCell_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedCells.Count == 0) return;
            PushUndoSnapshot(dgvSchedule.SelectedCells.Cast<DataGridViewCell>());

            foreach (DataGridViewCell cell in dgvSchedule.SelectedCells)
            {
                cell.Style.BackColor = _chosenColor;
                cell.Style.SelectionBackColor = Color.Empty;
                _customFilled.Add(new Point(cell.ColumnIndex, cell.RowIndex));
                dgvSchedule.InvalidateCell(cell);
            }
            RecalcConflictsAndHours();
        }

        private void btnTextColor_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedCells.Count == 0) return;
            PushUndoSnapshot(dgvSchedule.SelectedCells.Cast<DataGridViewCell>());

            foreach (DataGridViewCell cell in dgvSchedule.SelectedCells)
            {
                cell.Style.ForeColor = _chosenColor;
                cell.Style.SelectionForeColor = _chosenColor;
                dgvSchedule.InvalidateCell(cell);
            }
        }

        private void btnBorderCell_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedCells.Count == 0) return;
            PushUndoSnapshot(dgvSchedule.SelectedCells.Cast<DataGridViewCell>());

            foreach (DataGridViewCell cell in dgvSchedule.SelectedCells)
            {
                var key = new Point(cell.ColumnIndex, cell.RowIndex);
                _cellBorders[key] = _chosenColor;
                dgvSchedule.InvalidateCell(cell);
            }
        }

        // !! додайте десь у класі FormPageSecond

        /// <summary>Текст у першій колонці для вказаного рядка (мітка "3, Fri.").</summary>
        public string GetDayLabel(int rowIndex)
            => dgvSchedule.Rows[rowIndex].Cells[0].Value?.ToString();

        /// <summary>Перераховує години та конфлікти (тонка логіка закрита всередині).</summary>



        private void btnClearFormat_Click(object sender, EventArgs e)
        {



            if (dgvSchedule.SelectedCells.Count == 0) return;
            PushUndoSnapshot(dgvSchedule.SelectedCells.Cast<DataGridViewCell>());

            foreach (DataGridViewCell cell in dgvSchedule.SelectedCells)
            {
                cell.Style.BackColor = Color.Empty;
                cell.Style.ForeColor = Color.Empty;
                cell.Style.SelectionBackColor = Color.Empty;
                cell.Style.SelectionForeColor = Color.Empty;

                var key = new Point(cell.ColumnIndex, cell.RowIndex);


                _swappedCells.Remove(key);
                _swapInfos.Remove(key);
                _cellBorders.Remove(key);
                _customFilled.Remove(key);
                dgvSchedule.InvalidateCell(cell);
            }
            dgvSchedule.ClearSelection();
            RecalcConflictsAndHours();
        }

        /*―――――― Undo ――――――*/
        private void btnUndo_Click(object sender, EventArgs e) => PopAndRestore();

        private void PushUndoSnapshot(IEnumerable<DataGridViewCell> cells)
        {
            var snap = new List<CellMemento>();

            foreach (var cell in cells)
            {
                var key = new Point(cell.ColumnIndex, cell.RowIndex);
                _cellBorders.TryGetValue(key, out Color bCol);

                snap.Add(new CellMemento
                {
                    Key = key,
                    Back = cell.Style.BackColor,
                    Fore = cell.Style.ForeColor,
                    SelBack = cell.Style.SelectionBackColor,
                    SelFore = cell.Style.SelectionForeColor,
                    HadBorder = _cellBorders.ContainsKey(key),
                    BorderColor = bCol,
                    HadCustomFill = _customFilled.Contains(key)
                });
            }
            if (snap.Count == 0) return;
            if (_undoStack.Count == 5) _undoStack.RemoveAt(0);
            _undoStack.Add(snap);
            btnUndo.Enabled = true;
        }

        private void PopAndRestore()
        {
            if (_undoStack.Count == 0) return;

            var snap = _undoStack[_undoStack.Count - 1];
            _undoStack.RemoveAt(_undoStack.Count - 1);
            btnUndo.Enabled = _undoStack.Count > 0;

            foreach (var m in snap)
            {
                var cell = dgvSchedule.Rows[m.Key.Y].Cells[m.Key.X];

                cell.Style.BackColor = m.Back;
                cell.Style.ForeColor = m.Fore;
                cell.Style.SelectionBackColor = m.SelBack;
                cell.Style.SelectionForeColor = m.SelFore;

                if (m.HadBorder) _cellBorders[m.Key] = m.BorderColor; else _cellBorders.Remove(m.Key);
                if (m.HadCustomFill) _customFilled.Add(m.Key); else _customFilled.Remove(m.Key);

                dgvSchedule.InvalidateCell(cell);
            }
            RecalcConflictsAndHours();
        }

        /*―――――― custom CellPainting ――――――*/
        private void DgvSchedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;   // заголовки — нехтуємо

            var key = new Point(e.ColumnIndex, e.RowIndex);

            /* 1. стандартне малювання без системної рамки */
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

            /* 2. рамка, якщо є */
            if (_cellBorders.TryGetValue(key, out Color borderCol))
            {
                var rect = e.CellBounds;
                rect.Width -= 1;
                rect.Height -= 1;
                using (var pen = new Pen(borderCol, 2))
                    e.Graphics.DrawRectangle(pen, rect);
            }

            /* 3. індикатор коментаря */
            if (_cellComments.ContainsKey(key))
            {
                const int r = 5;                                // радіус кружка
                var bounds = e.CellBounds;
                var dot = new Rectangle(bounds.Right - r * 2 - 2, // відступ від країв
                                        bounds.Top + 2,
                                        r * 2, r * 2);

                using (var brush = new SolidBrush(Color.Red))
                    e.Graphics.FillEllipse(brush, dot);
            }

            if (_swappedCells.Contains(key))
            {
                var dot = new Rectangle(e.CellBounds.Left + 2, e.CellBounds.Top + 2, 10, 10);
                using (var br = new SolidBrush(Color.LimeGreen))
                    e.Graphics.FillEllipse(br, dot);
            }

            e.Handled = true;
        }

        /*―――――― порожні / службові обробники, залишились без змін ――――――*/
        private void lblWorkers_Click(object sender, EventArgs e) { }
        private void dtEvening_ValueChanged(object sender, EventArgs e) { }
        private void flpPriority_Paint(object sender, PaintEventArgs e) { }
        private void numWorkers_ValueChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = MyMessageBox.Input(
                             placeholder: "Enter a graphic name...",   // текст-підказка
                             caption: "Save schedule");          // заголовок діалогу

            if (string.IsNullOrWhiteSpace(name)) return;

            /* 1. Формуємо DTO разом із форматуванням */
            SavedSchedule dto = BuildSavedSchedule(name);

            /* 2. Пишемо у файл */
            try
            {
                ScheduleFileService.Save(dto);
                MyMessageBox.Show("Schedule successfully saved!",
                                                "OK", MessageBoxButtons.OK, owner: this);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("Failed to save: " + ex.Message);
            }
        }

        

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var containers = ContainerStorage.GetAll();
            
             using (var dlg = new DialogOpenSchedule(containers))
                 {
                     if (dlg.ShowDialog(this) != DialogResult.OK) return;
                
                     // відкриваємо лише перший вибраний графік
                var first = dlg.SelectedSchedules.FirstOrDefault();
                     if (first == null) return;
                
                LoadSavedSchedule(first);
                 }
        }

        private SavedSchedule BuildSavedSchedule(string name)
        {
            var schedule = new SavedSchedule
            {
                Name = name,
                SavedAt = DateTime.Now,
                Year = _year,
                Month = _month,
                ColumnHeaders = dgvSchedule.Columns
                                           .Cast<DataGridViewColumn>()
                                           .Select(c => c.HeaderText)
                                           .ToArray(),
                Values = new string[dgvSchedule.Rows.Count, dgvSchedule.Columns.Count]
            };

            for (int r = 0; r < dgvSchedule.Rows.Count; r++)
            {
                for (int c = 0; c < dgvSchedule.Columns.Count; c++)
                {
                    var cell = dgvSchedule.Rows[r].Cells[c];
                    schedule.Values[r, c] = cell.Value?.ToString();

                    /* ----- задній план ----- */
                    Color back = cell.Style.BackColor.IsEmpty
                                   ? cell.InheritedStyle.BackColor   // ← бере колір рядка, якщо є
                                   : cell.Style.BackColor;

                    if (!back.IsEmpty)
                        schedule.BackColors[$"{c}_{r}"] = ColorTranslator.ToHtml(back);

                    /* ----- колір тексту ----- */
                    Color fore = cell.Style.ForeColor.IsEmpty
                                   ? cell.InheritedStyle.ForeColor
                                   : cell.Style.ForeColor;

                    if (!fore.IsEmpty)
                        schedule.ForeColors[$"{c}_{r}"] = ColorTranslator.ToHtml(fore);
                }
            }

            foreach (var kvp in _cellBorders)
            {
                schedule.Borders[$"{kvp.Key.X}_{kvp.Key.Y}"] =
                    ColorTranslator.ToHtml(kvp.Value);
            }

            foreach (var kvp in _cellComments)
            {
                schedule.Comments[$"{kvp.Key.X}_{kvp.Key.Y}"] = kvp.Value;
            }


            return schedule;
        }

        public void ShowOpenMode()
        {
            guna2GroupBox2.Visible = true;   // група «Відкрити»
            guna2GroupBox6.Visible = false;
        }

        public void PrepareForEmbed()
        {
            const int MIN_CARD_WIDTH = 515;          // <- ваша мінімальна ширина

            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;

            dgvSchedule.AutoResizeColumns();
            dgvSchedule.AutoResizeRows();

            /* 1) обчислюємо «контентну» ширину */
            int wContent = dgvSchedule.GetPreferredSize(Size.Empty).Width
                           + dgvSchedule.Left * 2 + 10;     // поля-відступи

            /* 2) беремо максимум із контенту та порогу 400 px */
            int w = Math.Max(MIN_CARD_WIDTH, wContent);

            /* 3) висоту, як і раніше, – під available-host */
            int hostH = Parent?.ClientSize.Height ?? 600;
            int h = hostH - Margin.Vertical;

            ClientSize = new Size(w, h);

            /* 4) захищаємося від подальшого «стискання» менше 400 */
            MinimumSize = new Size(MIN_CARD_WIDTH, 0);
        }

        public void LoadSavedSchedule(SavedSchedule s)
        {
            dgvSchedule.CellEndEdit -= DgvSchedule_CellEndEdit;

            _year = s.Year;
            _month = s.Month;
            _days = DateTime.DaysInMonth(_year, _month);
            _cellBorders.Clear();
            _customFilled.Clear();

            dgvSchedule.Columns.Clear();
            foreach (string head in s.ColumnHeaders)
                dgvSchedule.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = head, Width = 60 });

            dgvSchedule.Rows.Clear();
            for (int r = 0; r < s.Values.GetLength(0); r++)
                dgvSchedule.Rows.Add();

            // значення + кольори
            for (int r = 0; r < s.Values.GetLength(0); r++)
                for (int c = 0; c < s.Values.GetLength(1); c++)
                {
                    var cell = dgvSchedule.Rows[r].Cells[c];
                    cell.Value = s.Values[r, c];

                    string key = $"{c}_{r}";

                    if (s.BackColors.TryGetValue(key, out var bg))
                    {
                        cell.Style.BackColor = ColorTranslator.FromHtml(bg);
                        _customFilled.Add(new Point(c, r));         // ← маркуємо як кастом
                    }
                    if (s.ForeColors.TryGetValue(key, out var fg))
                        cell.Style.ForeColor = ColorTranslator.FromHtml(fg);
                }

            // рамки
            foreach (var kv in s.Borders)
            {
                var parts = kv.Key.Split('_');
                int c = int.Parse(parts[0]);
                int r = int.Parse(parts[1]);

                var pt = new Point(c, r);
                _cellBorders[pt] = ColorTranslator.FromHtml(kv.Value);
                _customFilled.Add(pt);                // рамка = теж кастом-заливка
            }

            _cellComments.Clear();
            foreach (var kv in s.Comments)
            {
                var parts = kv.Key.Split('_');
                int c = int.Parse(parts[0]);
                int r = int.Parse(parts[1]);
                _cellComments[new Point(c, r)] = kv.Value;
            }
            dgvSchedule.Invalidate();

            _lastParams = new ScheduleParameters
            {
                WorkersPerShift = (int)numWorkers.Value,      // або s.WorkersPerShift, якщо ви його збережете
                MorningStart = dtMorning.Value.TimeOfDay, // базові значення достатні
                AfternoonStart = dtAfternoon.Value.TimeOfDay,
                EveningEnd = dtEvening.Value.TimeOfDay
            };
                

            _totalsRow = dgvSchedule.Rows.Count - 1;

            dgvSchedule.CellEndEdit += DgvSchedule_CellEndEdit;
            RecalcConflictsAndHours();
            dgvSchedule.Refresh();
        }


        private void MarkUnavailableWithRedBorder()
        {
            if (_dispo == null) return;

            // map <ім'я, колонка> у готовому графіку
            var nameToCol = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int c = 1; c < dgvSchedule.Columns.Count; c++)
                nameToCol[dgvSchedule.Columns[c].HeaderText] = c;

            // ітеруємо dispo
            foreach (DataGridViewRow dRow in _dispo.Rows)
            {
                string emp = dRow.Cells[0].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(emp) || !nameToCol.TryGetValue(emp, out int col))
                    continue;

                for (int day = 1; day <= _days; day++)
                {
                    string mark = dRow.Cells[day].Value?.ToString()?.Trim();
                    if (mark == "-")                                   // недоступний
                    {
                        int row = day;                                  // 1-й рядок – день 1
                        var key = new Point(col, row);

                        _cellBorders[key] = Color.Red;                  // рамка                      // захист від Recalc*
                        dgvSchedule.InvalidateCell(col, row);           // перемалювати одразу
                    }
                }
            }
        }

        private void BtnAddComment_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedCells.Count == 0)
            {
                MyMessageBox.Show("Select at least one cell first.");
                return;
            }

            // ✔ простіше за все – InputBox, але можна зробити власну форму
            string comment = MyMessageBox.Input(
                                caption: "Add comment",
                                placeholder: "Enter comment…",
                                owner: this);


            if (string.IsNullOrWhiteSpace(comment)) return;

            foreach (DataGridViewCell cell in dgvSchedule.SelectedCells)
            {
                var key = new Point(cell.ColumnIndex, cell.RowIndex);
                _cellComments[key] = comment;
                dgvSchedule.InvalidateCell(cell);          // примусове перерисування
            }
        }

        private static void PutValue(IXLCell cell, object v)
        {
            switch (v)
            {
                case null:
                case DBNull _:
                    cell.Clear();                 // порожня комірка
                    break;

                case int i:
                    cell.SetValue(i);
                    break;

                case double d:
                    cell.SetValue(d);
                    break;

                case decimal m:
                    cell.SetValue((double)m);     // Excel не знає decimal
                    break;

                case bool b:
                    cell.SetValue(b);
                    break;

                case DateTime dt:
                    cell.SetValue(dt);            // формат дати Excel
                    break;

                case TimeSpan ts:
                    cell.SetValue(ts);            // записує як час
                    cell.Style.NumberFormat.Format = "hh:mm";
                    break;

                default:
                    cell.SetValue(v.ToString());  // усе інше → рядок
                    break;
            }
        }

        
        private void ExportToExcel(string path)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Schedule");

            /* ——— заголовки колонок ——— */
            for (int c = 0; c < dgvSchedule.Columns.Count; c++)
            {
                var xl = ws.Cell(1, c + 1);
                xl.Value = dgvSchedule.Columns[c].HeaderText;
                xl.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                xl.Style.Font.Bold = true;
            }

            /* ——— дані + колір тексту/фону ——— */
            for (int r = 0; r < dgvSchedule.Rows.Count; r++)
            {
                for (int c = 0; c < dgvSchedule.Columns.Count; c++)
                {
                    var src = dgvSchedule.Rows[r].Cells[c];
                    var dest = ws.Cell(r + 2, c + 1);

                    PutValue(dest, src.Value);     // ← замість SetValue(v)

                    /* текст */
                    Color fore = src.Style.ForeColor.IsEmpty
                               ? src.InheritedStyle.ForeColor
                               : src.Style.ForeColor;
                    if (!fore.IsEmpty)
                        dest.Style.Font.FontColor = XLColor.FromColor(fore);

                    /* фон */
                    Color back = src.Style.BackColor.IsEmpty
                               ? src.InheritedStyle.BackColor
                               : src.Style.BackColor;
                    if (!back.IsEmpty)
                        dest.Style.Fill.BackgroundColor = XLColor.FromColor(back);
                }
            }


            /* ——— рамки, які ви малювали червоним/будь-яким ——— */
            foreach (var kv in _cellBorders)
            {
                var pt = kv.Key;      // (X = ColumnIndex, Y = RowIndex)
                var col = pt.X + 1;    // Excel 1-based
                var row = pt.Y + 2;    // +1 через заголовок +1 через 0-based

                var cell = ws.Cell(row, col);
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.FromColor(kv.Value);
            }

            /* ——— коментарі ——— */
            foreach (var kv in _cellComments)
            {
                var pt = kv.Key;
                var col = pt.X + 1;
                var row = pt.Y + 2;

                ws.Cell(row, col).GetComment()
                                 .AddText(kv.Value);
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(path);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "Schedule.xlsx"
            })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    ExportToExcel(sfd.FileName);
                    MyMessageBox.Show("File exported successfully!",
                                      "Excel", owner: this);
                }
                catch (Exception ex)
                {
                    MyMessageBox.Show("Export failed:\n" + ex.Message,
                                      "Error", MessageBoxButtons.OK, owner: this);
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
