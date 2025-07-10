// FormMain.cs  •  C#-7.3
using GF.Scheduling;
using GF.UI;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GF
{
    public partial class FormMain : Form
    {
        /*──────── drag-move поля ────────*/
        private bool isDragging;
        private Point lastCursor;
        private Point lastForm;

        /*──────── сторінки ────────*/
        private readonly FormPageOne pageOne = new FormPageOne();
        private FormPageSecond pageSecond;            // створюємо, коли натиснуто Create
        private ScheduleManagerForm scheduleManagerForm;
        private readonly FormPageThird pageThird = new FormPageThird();
        private readonly IServiceProvider _sp;

        /*──────── ctor ────────*/
        public FormMain(IServiceProvider sp)
        {
            _sp = sp;
            InitializeComponent();
            DoubleBuffered = true;

            /* ловимо подію з Page 1 */
            pageOne.ScheduleRequested += OnScheduleRequested;

            /* відкриваємо першу сторінку за замовчуванням */
            //loadform(pageOne);

        }

        /*══════════════ Навігація ══════════════*/
        private void loadform(Form f)
        {
            if (panelLoadMain.Controls.Count > 0)
                panelLoadMain.Controls.RemoveAt(0);

            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            panelLoadMain.Controls.Add(f);
            panelLoadMain.Tag = f;
            f.Show();
        }

        /* ——— кнопки сторінок ——— */
        private void pageButton1_Click(object sender, EventArgs e) => loadform(pageOne);

        private void pageButton2_Click(object sender, EventArgs e)
        {
            // створюємо форму, якщо її ще немає (або була закрита)
            if (pageSecond == null || pageSecond.IsDisposed)
                pageSecond = ActivatorUtilities.CreateInstance<FormPageSecond>(_sp);

            // якщо розклад порожній — показуємо повідомлення і виходимо
            if (pageSecond.IsScheduleEmpty)
            {
                FormPageSecond.MyMessageBox.Show(
                    "Please fill the dispo first.",   // текст
                    "Empty schedule");                // заголовок
                return;
            }

            // у розкладі є дані – відкриваємо картку
            pageSecond.ShowOpenMode();
            loadform(pageSecond);

        }

        private void pageButton3_Click(object sender, EventArgs e) => loadform(pageThird);

        /*══════════════ реакція на натиск «Create» у Page 1 ══════════════*/
        private void OnScheduleRequested(DataGridView dispo,
                                 List<Employee> employees,
                                 MonthContainer mc)
        {
            pageSecond?.Dispose();

            pageSecond = ActivatorUtilities.CreateInstance<FormPageSecond>(
                             _sp,            // той, що зберегли в полі
                             dispo,
                             employees,
                             mc,
                             pageOne.MonthContainers);   // 4-й аргумент

            loadform(pageSecond);
        }

        /*══════════════ Drag-move верхньої панелі ══════════════*/
        private void panelUpperWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isDragging = true;
            lastCursor = Cursor.Position;
            lastForm = Location;
        }

        private void panelUpperWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            Point cur = Cursor.Position;
            int dx = cur.X - lastCursor.X;
            int dy = cur.Y - lastCursor.Y;
            Location = new Point(lastForm.X + dx, lastForm.Y + dy);
        }

        private void panelUpperWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isDragging = false;
        }

        /*══════════════ Кнопки вікна ══════════════*/
        private void guna2Button1_Click(object sender, EventArgs e) {
            Environment.Exit(0);
        }   

        private void guna2Button2_Click(object sender, EventArgs e)   // «-»
            => WindowState = FormWindowState.Minimized;

        private void guna2Button3_Click(object sender, EventArgs e)   // «🗖»
            => WindowState = WindowState == FormWindowState.Normal
                            ? FormWindowState.Maximized
                            : FormWindowState.Normal;

        private void btnScheduleManagerPage_Click(object sender, EventArgs e)
        {
            // Створюємо форму лише один раз,
            // або заново — якщо попередній екземпляр закрили.
            if (scheduleManagerForm == null || scheduleManagerForm.IsDisposed)
                scheduleManagerForm = new ScheduleManagerForm();

            loadform(scheduleManagerForm);
        }
    }
}
