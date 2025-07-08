// DialogOpenSchedule.cs • C# 7.3
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GF.Scheduling;
using GF.Scheduling.IO;
using static GF.UI.MyMessageBox;

namespace GF.UI
{
    public partial class DialogOpenSchedule : Form
    {
        // ────────────── константи / enum ──────────────
        private const string BACK_ITEM = "← Back";

        private enum ViewMode
        {
            Containers,
            Schedules
        }

        // ────────────── поля ──────────────
        private readonly List<MonthContainer> _containers;
        private MonthContainer _current;
        private ViewMode _mode = ViewMode.Containers;

        public List<SavedSchedule> SelectedSchedules { get; } = new List<SavedSchedule>();

        // ────────────── ctor ──────────────
        public DialogOpenSchedule(IEnumerable<MonthContainer> containers)
        {
            InitializeComponent();

            /* один ListBox — lstMain (додається у Designer) */
            lstFiles.SelectionMode = SelectionMode.MultiExtended;
            lstFiles.DoubleClick += lstMain_DoubleClick;

            _containers = containers?.ToList() ?? new List<MonthContainer>();
            ShowContainers();
        }

        // ────────────── helpers ──────────────
        private void ShowContainers()
        {
            _mode = ViewMode.Containers;
            lstFiles.Items.Clear();
            lstFiles.Items.AddRange(_containers.Select(c => c.Name).ToArray());
            lstFiles.Text = "Containers";        // підписи за бажання
        }

        private void ShowSchedules(MonthContainer c)
        {
            _mode = ViewMode.Schedules;
            _current = c;

            lstFiles.Items.Clear();
            lstFiles.Items.Add(BACK_ITEM);                      // 0-й пункт
            lstFiles.Items.AddRange(c.Schedules.Select(s => s.Name).ToArray());
            lstFiles.Text = $"Schedules of «{c.Name}»";
        }

        // ────────────── події ──────────────
        private void lstMain_DoubleClick(object sender, EventArgs e)
        {
            int idx = lstFiles.SelectedIndex;
            if (idx < 0) return;

            if (_mode == ViewMode.Containers)
            {
                ShowSchedules(_containers[idx]);
            }
            else // Schedules
            {
                if (idx == 0)       // Back
                {
                    ShowContainers();
                }
                /* Подвійний клік по графіку можна відкрити одразу,
                   але традиційно лишаємо це на кнопку Open */
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (_mode != ViewMode.Schedules)
            {
                MyMessageBox.Show("Choose a container first.");
                return;
            }

            SelectedSchedules.Clear();
            foreach (string name in lstFiles.SelectedItems.Cast<string>())
            {
                if (name == BACK_ITEM) continue;
                var sch = _current.Schedules.FirstOrDefault(s => s.Name == name);
                if (sch != null) SelectedSchedules.Add(sch);
            }

            if (SelectedSchedules.Count == 0)
            {
                MyMessageBox.Show("Select at least one schedule.");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_mode != ViewMode.Schedules)
            {
                MyMessageBox.Show("Switch to schedules first.");
                return;
            }

            var toDel = lstFiles.SelectedItems
                               .Cast<string>()
                               .Where(n => n != BACK_ITEM)
                               .ToList();
            if (toDel.Count == 0) return;

            if (MyMessageBox.Show($"Delete {toDel.Count} schedule(s)?",
                                  "Confirm", MessageBoxButtons.YesNo)
                != DialogResult.Yes) return;

            _current.Schedules.RemoveAll(s => toDel.Contains(s.Name));
            ScheduleFileService.ContainerStorage.Save(_containers);

            ShowSchedules(_current);         // оновити список
        }

        private void btnCancel_Click(object s, EventArgs e) =>
            DialogResult = DialogResult.Cancel;

        // ────────────── drag-move (як було) ──────────────
        private bool _drag;
        private Point _lastCur, _lastLoc;

        private void panelHeader_MouseDown(object s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _drag = true;
            _lastCur = Cursor.Position;
            _lastLoc = Location;
        }

        private void panelHeader_MouseMove(object s, MouseEventArgs e)
        {
            if (!_drag) return;
            var cur = Cursor.Position;
            Location = new Point(_lastLoc.X + cur.X - _lastCur.X,
                                 _lastLoc.Y + cur.Y - _lastCur.Y);
        }

        private void panelHeader_MouseUp(object s, MouseEventArgs e) => _drag = false;
    }
}
