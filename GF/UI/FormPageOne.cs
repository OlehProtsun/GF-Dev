// FormPageOne.cs • C# 7.3  (UI: Guna.UI2)
using GF.Scheduling;
using GF.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static GF.Scheduling.IO.ScheduleFileService;
using static GF.UI.FormPageSecond;

namespace GF.UI
{
    public partial class FormPageOne : Form
    {
        /* ————— поля ————— */
        private readonly List<string> employeeNames = new List<string>();
        private List<BindModel> binds;
        private readonly string bindFile;
        private DateTime lastBindWriteUtc;

        private readonly List<MonthContainer> monthContainers = new List<MonthContainer>();
        private const string CreateItemCaption = "Create new…";
        private const string CreateEmployeeCaption = "Create new…";

        public List<MonthContainer> MonthContainers => monthContainers;

        /* ———— ПОДІЯ ———— */
        /// <summary>
        /// Виникає, коли користувач натискає «Create».
        /// </summary>
        public event Action<DataGridView, List<Employee>, MonthContainer> ScheduleRequested;

        /* ———— ctor ———— */
        public FormPageOne()
        {
            InitializeComponent();

            /* підписки */
            KeyPreview = true;
            KeyDown += FormKeyDown;

            dataGridView.CellEndEdit += DataGridView_CellEndEdit;
            comboBoxContainer.SelectedIndexChanged += ComboBoxContainer_SelectedIndexChanged;
            buttonAddEmployee.Click += ButtonAddEmployee_Click;
            buttonRemoveColumn.Click += ButtonRemoveColumn_Click;
            buttonGenerate.Click += ButtonGenerate_Click;




            SelectEmployeeComboBox.SelectedIndexChanged += SelectEmployeeComboBox_SelectedIndexChanged;
            SelectEmployeeComboBox.Items.Add(CreateEmployeeCaption);
            SelectEmployeeComboBox.SelectedIndex = -1;

            comboBoxContainer.Items.Add(CreateItemCaption);   // перший (та єдиний) елемент
            comboBoxContainer.SelectedIndex = -1;

            /* binds */
            bindFile = Path.Combine(System.Windows.Forms.Application.StartupPath, "binds.json");
            binds = BindStorage.Load();
            lastBindWriteUtc = File.Exists(bindFile)
                               ? File.GetLastWriteTimeUtc(bindFile)
                               : DateTime.MinValue;

            monthContainers.AddRange(ContainerStorage.GetAll());

            foreach (var m in monthContainers)
                comboBoxContainer.Items.Insert(comboBoxContainer.Items.Count - 1, m);

            /* початковий стан */
            RefreshRemoveCombo();
        }

        private void SelectEmployeeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectEmployeeComboBox.SelectedIndex < 0) return;

            // лише пункт «Create new…» має логіку
            if (SelectEmployeeComboBox.SelectedItem.ToString() == CreateEmployeeCaption)
            {
                SelectEmployeeComboBox.SelectedIndex = -1; // скидаємо виділення
                ShowAddEmployeeDialog();
            }
            // якщо це звичайний працівник — нічого не робимо,
            // додавати будемо через кнопку «Add Employee»
        }

        private void ShowAddEmployeeDialog()
        {
            // Потрібно, щоб був вибраний контейнер
            var mc = comboBoxContainer.SelectedItem as MonthContainer;
            if (mc == null)
            {
                MyMessageBox.Show("Спершу виберіть місяць-контейнер!", "Увага",
                                  MessageBoxButtons.OK);
                return;
            }

            using (var dlg = new AddEmployeeDialog())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                string name = dlg.EmployeeName;
                if (string.IsNullOrWhiteSpace(name)) return;

                if (mc.Employees.Contains(name))
                {
                    MyMessageBox.Show("Такий працівник уже існує в контейнері.", "Увага",
                                      MessageBoxButtons.OK);
                    return;
                }

                // Додаємо в контейнер
                mc.Employees.Add(name);

                // Оновлюємо вміст комбо-бокса
                RefreshEmployeeCombo();

                // За бажання – відразу виділяємо новостворений пункт
                SelectEmployeeComboBox.SelectedItem = name;
            }
        }


        private void RefreshEmployeeCombo()
        {
            SelectEmployeeComboBox.Items.Clear();

            if (comboBoxContainer.SelectedItem is MonthContainer mc)
                SelectEmployeeComboBox.Items.AddRange(mc.Employees.ToArray());

            SelectEmployeeComboBox.Items.Add(CreateEmployeeCaption);   // завжди останнім
            SelectEmployeeComboBox.SelectedIndex = -1;
        }


        private void ComboBoxContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxContainer.SelectedIndex < 0) return;

            if (comboBoxContainer.SelectedItem?.ToString() == CreateItemCaption)
            {
                comboBoxContainer.SelectedIndex = -1;
                CreateNewContainer();
                return;
            }

            if (comboBoxContainer.SelectedItem is MonthContainer mc)
            {
                GenerateDays(mc.Month);        // тепер місяць береться звідси
                RebuildEmployeeColumns();
                RefreshRemoveCombo();
                RefreshEmployeeCombo();
            }
        }


        private void CreateNewContainer()
        {
            using (var dlg = new MonthCreationDialog())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var mc = new MonthContainer(dlg.SelectedMonth,
                                            dlg.SelectedYear,
                                            dlg.ContainerName);

                monthContainers.Add(mc);

                // ⬇️  тепер зберігаємо ВЕСЬ перелік
                ContainerStorage.Save(monthContainers);

                comboBoxContainer.Items.Insert(comboBoxContainer.Items.Count - 1, mc);
                comboBoxContainer.SelectedItem = mc;
            }
        }


        /*══════════════  КНОПКА «Create»  (відкриваємо FormPageSecond) ══════════════*/
        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            if (!(comboBoxContainer.SelectedItem is MonthContainer mc))
            {
                MessageBox.Show(@"Спершу створіть або виберіть місяць-контейнер!",
                                @"Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (comboBoxContainer.SelectedIndex < 0)              // нічого не обрано
            {
                MyMessageBox.Show("Оберіть місяць!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            if (employeeNames.Count == 0) return;

            var dispo = BuildDispoGridTransposed();
            var employees = BuildEmployeeList();
            ScheduleRequested?.Invoke(dispo, employees, mc);   // mc — обраний MonthContainer
        }


        /*–––––– helper: список Employee ––––––*/
        private List<Employee> BuildEmployeeList()
        {
            var list = new List<Employee>();
            foreach (string n in employeeNames)
                list.Add(new Employee { Name = n, IsFullTime = true });
            return list;
        }

        /*–––––– helper: транспонована диспозиція ––––––*/
        private DataGridView BuildDispoGridTransposed()
        {
            int dayRows = dataGridView.Rows.Count - 1;
            var gv = new DataGridView();
            gv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Employee" });
            for (int d = 1; d <= dayRows; d++)
                gv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = d.ToString() });

            for (int c = 1; c < dataGridView.Columns.Count; c++)
            {
                string name = dataGridView.Rows[0].Cells[c].Value?.ToString();
                if (string.IsNullOrWhiteSpace(name)) continue;

                var row = new DataGridViewRow();
                row.CreateCells(gv);
                row.Cells[0].Value = name;
                for (int d = 1; d <= dayRows; d++)
                    row.Cells[d].Value = dataGridView.Rows[d].Cells[c].Value ?? "";
                gv.Rows.Add(row);
            }
            return gv;
        }

        /*══════════════  UI-ЛОГІКА «ДИСПОЗИЦІЇ»  ══════════════*/

        /*–1–  швидкі бінди («+», «-», години…) –*/
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (File.Exists(bindFile))
            {
                var wt = File.GetLastWriteTimeUtc(bindFile);
                if (wt != lastBindWriteUtc)
                {
                    binds = BindStorage.Load();
                    lastBindWriteUtc = wt;
                }
            }

            if (dataGridView.CurrentCell == null ||
                dataGridView.CurrentCell.ColumnIndex == 0)
                return;

            foreach (var b in binds)
            {
                if (!b.IsActive) continue;
                if (e.KeyCode.ToString().Equals(b.Key, StringComparison.OrdinalIgnoreCase))
                {
                    e.SuppressKeyPress = true; e.Handled = true;
                    InsertTextAndMoveDown(b.Text);
                    break;
                }
            }
        }
        private void InsertTextAndMoveDown(string text)
        {
            var cell = dataGridView.CurrentCell;
            cell.Value = text;
            cell.Style.BackColor = text == "-" ? Color.LightCoral : Color.Empty;

            int next = cell.RowIndex + 1;
            if (next < dataGridView.Rows.Count)
                dataGridView.CurrentCell = dataGridView.Rows[next].Cells[cell.ColumnIndex];
        }

        /*–2–  фарбуємо після редагування –*/
        private void DataGridView_CellEndEdit(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= 0 || e.ColumnIndex == 0) return;
            var cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var value = (cell.Value ?? "").ToString().Trim();
            cell.Style.BackColor = value == "-" ? Color.LightCoral : Color.Empty;
        }

        /*–3–  вибір місяця –*/
        private void ComboBoxMonth_SelectedIndexChanged(object s, EventArgs e)
        {
            RebuildEmployeeColumns();
            RefreshRemoveCombo();
        }

        /*–4–  додати працівника –*/
        private void ButtonAddEmployee_Click(object s, EventArgs e)
        {
            // 1) Перевірка контейнера
            var mc = comboBoxContainer.SelectedItem as MonthContainer;
            if (mc == null)
            {
                MyMessageBox.Show("Спершу виберіть місяць-контейнер!", "Увага",
                                  MessageBoxButtons.OK);
                return;
            }

            // 2) Перевірка вибору працівника
            if (SelectEmployeeComboBox.SelectedItem == null ||
                SelectEmployeeComboBox.SelectedItem.ToString() == CreateEmployeeCaption)
            {
                MyMessageBox.Show("Оберіть працівника зі списку!", "Увага",
                                  MessageBoxButtons.OK);
                return;
            }

            string name = SelectEmployeeComboBox.SelectedItem.ToString();

            // 3) Якщо вже присутній у гріді — ігноруємо
            if (employeeNames.Contains(name)) return;

            // 4) Додаємо у таблицю
            employeeNames.Add(name);
            AddEmployeeColumn(name);
            RefreshRemoveCombo();
        }

        /*–5–  видалити колонку –*/
        private void ButtonRemoveColumn_Click(object s, EventArgs e)
        {
            if (!(comboBoxRemoveColumn.SelectedItem is string name)) return;

            for (int i = 1; i < dataGridView.Columns.Count; i++)
                if (dataGridView.Columns[i].HeaderText == name)
                {
                    dataGridView.Columns.RemoveAt(i);
                    break;
                }
            employeeNames.Remove(name);
            RefreshRemoveCombo();
        }
        private void RefreshRemoveCombo()
        {
            comboBoxRemoveColumn.Items.Clear();
            comboBoxRemoveColumn.Items.AddRange(employeeNames.ToArray());
            comboBoxRemoveColumn.SelectedIndex = -1;
        }

        /*–6–  згенерувати список днів місяця –*/
        private void GenerateDays(int month)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Day",
                Width = 60,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            int year = DateTime.Now.Year;

            int days = DateTime.DaysInMonth(year, month);

            dataGridView.Rows.Add(); // 0-й рядок – імена співробітників

            for (int d = 1; d <= days; d++)
            {
                string label = $"{d}, {DateUtils.GetDayAbbr(year, month, d)}.";
                int r = dataGridView.Rows.Add(label);

                var dt = new DateTime(year, month, d);
                if (dt.DayOfWeek == DayOfWeek.Saturday ||
                    dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataGridView.Rows[r].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        /*–7–  колонки працівників –*/
        private void AddEmployeeColumn(string name)
        {
            var col = new DataGridViewTextBoxColumn
            {
                HeaderText = name,
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dataGridView.Columns.Add(col);

            for (int r = 0; r < dataGridView.Rows.Count; r++)
                dataGridView.Rows[r].Cells[col.Index].Value = r == 0 ? name : "";
        }
        private void RebuildEmployeeColumns()
        {
            while (dataGridView.Columns.Count > 1)
                dataGridView.Columns.RemoveAt(1);
            foreach (string n in employeeNames)
                AddEmployeeColumn(n);
        }

        private void comboBoxMonth_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
