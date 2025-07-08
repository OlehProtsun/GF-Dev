using System;
using System.Drawing;
using System.Windows.Forms;

namespace GF.UI
{
    public partial class MessangeForm : Form
    {
        /*-------------------------------------------------
         *  ПУБЛІЧНІ ВЛАСТИВОСТІ
         *------------------------------------------------*/
        public string UserInput => guna2TextBox1.Text;

        /*-------------------------------------------------
         *  КОНСТРУКТОР
         *------------------------------------------------*/
        public MessangeForm(string text = "",
                            MessageBoxButtons buttons = MessageBoxButtons.OK,
                            bool askInput = false,
                            string defaultInput = "")
        {
            InitializeComponent();

            /* ---- ТЕКСТОВЕ ПОЛЕ ---- */
            if (askInput)
            {
                //  INPUT-режим: поле редагується
                guna2TextBox1.ReadOnly = false;
                guna2TextBox1.TextAlign = HorizontalAlignment.Left;
                guna2TextBox1.Text = defaultInput;
                guna2TextBox1.PlaceholderText = text;    // «Enter comment…»
                guna2TextBox1.Select();                  // одразу курсор
            }
            else
            {
                //  MESSAGE-режим: просто виводимо повідомлення
                guna2TextBox1.ReadOnly = true;
                guna2TextBox1.TextAlign = HorizontalAlignment.Center;
                guna2TextBox1.Text = text;
            }

            /* ---- КНОПКИ ---- */
            ConfigureButtons(buttons, askInput);
        }

        /*-------------------------------------------------
         *  КОНФІГУРАЦІЯ КНОПОК
         *------------------------------------------------*/
        private void ConfigureButtons(MessageBoxButtons buttons, bool askInput)
        {
            // спершу ховаємо всі
            guna2Button1.Visible = btnCancel.Visible =
            btnYes.Visible = btnNo.Visible = false;

            // ----- режим Input:  OK / Cancel -----
            if (askInput)
            {
                guna2Button1.Visible = true;   // OK
                btnCancel.Visible = true;   // Cancel

                guna2Button1.DialogResult = DialogResult.OK;
                btnCancel.DialogResult = DialogResult.Cancel;

                AcceptButton = guna2Button1;
                CancelButton = btnCancel;
                return;
            }

            // ----- режим MessageBox -----
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    guna2Button1.Visible = true;           // OK
                    guna2Button1.DialogResult = DialogResult.OK;
                    AcceptButton = CancelButton = guna2Button1;
                    break;

                case MessageBoxButtons.OKCancel:
                    guna2Button1.Visible = true;           // OK
                    btnCancel.Visible = true;           // Cancel

                    guna2Button1.DialogResult = DialogResult.OK;
                    btnCancel.DialogResult = DialogResult.Cancel;

                    AcceptButton = guna2Button1;
                    CancelButton = btnCancel;
                    break;

                case MessageBoxButtons.YesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;

                    btnYes.DialogResult = DialogResult.Yes;
                    btnNo.DialogResult = DialogResult.No;

                    AcceptButton = btnYes;
                    CancelButton = btnNo;
                    break;
            }
        }

        /*-------------------------------------------------
         *  СЛУЖБОВЕ: перетягування вікна
         *------------------------------------------------*/
        private bool _drag;
        private Point _startCursor, _startForm;

        private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _drag = true;
            _startCursor = Cursor.Position;
            _startForm = Location;
        }

        private void guna2Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_drag) return;

            Point cur = Cursor.Position;
            Location = new Point(_startForm.X + cur.X - _startCursor.X,
                                 _startForm.Y + cur.Y - _startCursor.Y);
        }

        private void guna2Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _drag = false;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
