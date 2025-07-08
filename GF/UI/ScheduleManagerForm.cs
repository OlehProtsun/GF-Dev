using GF.Scheduling.IO;
using GF.Scheduling;
using GF.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GF.UI.FormPageSecond;
using System.IO;
using static GF.Scheduling.IO.ScheduleFileService;

namespace GF
{
    public partial class ScheduleManagerForm : Form
    {

        private (FormPageSecond page, DataGridViewCell cell)? _firstPick;

        private bool _swapModeActive = false;
        public ScheduleManagerForm()
        {
            InitializeComponent();
            flpSchedules.Dock = DockStyle.Fill;
            flpSchedules.FlowDirection = FlowDirection.LeftToRight;
            flpSchedules.WrapContents = false;        // одна стрічка
            flpSchedules.AutoScroll = true;         // увімкнути скрол

            // події для підгонки
            flpSchedules.SizeChanged += Flp_SizeChanged;
            flpSchedules.Scroll += Flp_Scroll;  // ховатимемо V-scroll

        }

        private void Flp_Scroll(object sender, ScrollEventArgs e)
        {
            flpSchedules.VerticalScroll.Visible = false;
            flpSchedules.VerticalScroll.Enabled = false;
        }

        private void btnOpenSchedules_Click(object sender, EventArgs e)
        {
            var containers = ContainerStorage.GetAll();
            
            using (var dlg = new DialogOpenSchedule(containers))   // ← передали
            {
                        if (dlg.ShowDialog(this) != DialogResult.OK) return;
            LoadMany(dlg.SelectedSchedules);                    // ← DTO-об’єкти
            }
        }

        private void Flp_SizeChanged(object sender, EventArgs e) => AdjustCardsHeight();

        private void AdjustCardsHeight()
        {
            int hostH = flpSchedules.ClientSize.Height;

            // перебираємо ТІЛЬКИ ті контролі, що реально є FormPageSecond
            foreach (Control ctrl in flpSchedules.Controls)
            {
                if (ctrl is FormPageSecond card)        // ✔ безпечне pattern-match
                {
                    int h = hostH - card.Margin.Vertical;   // віднімаємо відступи
                    if (card.ClientSize.Height != h)
                        card.ClientSize = new Size(card.ClientSize.Width, h);
                }
            }
        }

        // колбек, який отримує клітинку з будь-якої картки
        private void OnCellPicked(FormPageSecond page, DataGridViewCell cell)
        {
            if (_firstPick == null)
            {
                _firstPick = (page, cell);
                cell.Selected = true;
                return;
            }

            var (page1, cell1) = _firstPick.Value;
            var page2 = page;
            var cell2 = cell;

            DoSwap(page1, cell1, page2, cell2);
            CancelSwapMode();
        }

        private void LoadMany(IEnumerable<SavedSchedule> schedules)
        {
            flpSchedules.SuspendLayout();

            foreach (var dto in schedules)
            {
                var card = new FormPageSecond();            // порожній ctor
                card.ShowOpenMode();
                card.LoadSavedSchedule(dto);
                card.PrepareForEmbed();
                card.Margin = new Padding(0, 0, 15, 0);

                flpSchedules.Controls.Add(card);
                card.Show();
            }

            flpSchedules.ResumeLayout();
            AdjustCardsHeight();
        }

        // ScheduleManagerForm.cs  (додайте всередині класу)
        private void DoSwap(FormPageSecond p1, DataGridViewCell c1,
                            FormPageSecond p2, DataGridViewCell c2)
        {
            string day = p1.GetDayLabel(c1.RowIndex);

            // що стояло ДО операції
            string shift1 = Convert.ToString(c1.Value);
            string shift2 = Convert.ToString(c2.Value);

            var swap = new SwapInfo
            {
                FromFile = p1.SourceFile,
                ToFile = p2.SourceFile,
                Day = day,
                FromEmployee = p1.GetEmployeeName(c1.ColumnIndex),
                ToEmployee = p2.GetEmployeeName(c2.ColumnIndex),
                FromShift = shift1,
                ToShift = shift2,
                When = DateTime.Now
            };

            // обмін / передача
            var tmp = c1.Value;
            c1.Value = string.IsNullOrWhiteSpace(Convert.ToString(c2.Value))
                       ? null
                       : c2.Value;
            c2.Value = tmp;

            p1.MarkSwapped(c1.ColumnIndex, c1.RowIndex, swap);
            p2.MarkSwapped(c2.ColumnIndex, c2.RowIndex, swap);

            // ⬇️ замість RecalcConflictsAndHours() викликаємо публічну обгортку
            p1.Recalc();
            if (p2 != p1) p2.Recalc();
        }



        private void btnSwapShift_Click(object sender, EventArgs e)
        {
            ActivateSwapMode();
            MyMessageBox.Show("Pick TWO cells – they may be in different schedules.");
        }

        private void ActivateSwapMode()
        {
            if (_swapModeActive) return;

            _swapModeActive = true;
            _firstPick = null;

            //-- візуально «утискаємо» кнопку


            btnStopSwapShift.Enabled = true;
            btnSwapShift.Enabled = false;
            Cursor = Cursors.Hand;

            foreach (var card in flpSchedules.Controls.OfType<FormPageSecond>())
                card.EnablePickMode(OnCellPicked);
        }

        private void CancelSwapMode()
        {
            if (!_swapModeActive) return;

            _swapModeActive = false;
            _firstPick = null;

            //-- повертаємо звичайний вигляд


            btnStopSwapShift.Enabled = false;
            btnSwapShift.Enabled = true;
            Cursor = Cursors.Default;

            foreach (var card in flpSchedules.Controls.OfType<FormPageSecond>())
                card.DisablePickMode();
        }

        private void btnStopSwapShift_Click(object sender, EventArgs e)
        {
            CancelSwapMode();
        }
    }
}

