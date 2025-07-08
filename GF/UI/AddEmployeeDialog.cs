using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GF.UI.MyMessageBox;

namespace GF.UI
{
    public partial class AddEmployeeDialog: Form
    {

        /*──────── drag-move поля ────────*/
        private bool isDragging;
        private Point lastCursor;
        private Point lastForm;

        public AddEmployeeDialog()
        {
            InitializeComponent();
        }

        public string EmployeeName => textBoxName.Text.Trim();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isDragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            Point cur = Cursor.Position;
            int dx = cur.X - lastCursor.X;
            int dy = cur.Y - lastCursor.Y;
            Location = new Point(lastForm.X + dx, lastForm.Y + dy);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isDragging = true;
            lastCursor = Cursor.Position;
            lastForm = Location;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MyMessageBox.Show(@"Введіть ім’я!", @"Помилка", MessageBoxButtons.OK);
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
