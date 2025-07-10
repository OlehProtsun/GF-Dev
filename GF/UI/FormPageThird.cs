using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GF.UI
{
    // ---------------------------------------------------------------------
    //  ЛОГІЧНА ЧАСТИНА ФОРМИ
    // ---------------------------------------------------------------------
    public partial class FormPageThird : Form
    {
        private readonly string bindsFilePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "binds.json");
        private List<BindModel> binds = new List<BindModel>();

        public FormPageThird()
        {
            InitializeComponent();
            LoadBinds();
        }

        // Додаємо новий бінд
        private void buttonAddBind_Click(object sender, EventArgs e)
        {
            string key = textBoxKey.Text.Trim();
            string text = textBoxText.Text.Trim();

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(text))
            {
                binds.Add(new BindModel { Key = key, Text = text, IsActive = true });
                SaveBinds();
                RefreshGrid();
                textBoxKey.Clear();
                textBoxText.Clear();
            }
        }

        // Клік по клітинці DataGridView (видалення / активність)
        private void dataGridViewBinds_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dataGridViewBinds.Columns[e.ColumnIndex].Name;

            if (columnName == "Delete")
            {
                binds.RemoveAt(e.RowIndex);
                SaveBinds();
                RefreshGrid();
            }
            else if (columnName == "IsActive")
            {
                binds[e.RowIndex].IsActive = !binds[e.RowIndex].IsActive;
                dataGridViewBinds.Rows[e.RowIndex].Cells["IsActive"].Value =
                    binds[e.RowIndex].IsActive;          // відобразити у гріді
                SaveBinds();
            }
        }

        // Натиснення клавіші – виконуємо активні бінди
        private void FormPageThird_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var bind in binds)
            {
                if (bind.IsActive && e.KeyCode.ToString().Equals(bind.Key, StringComparison.OrdinalIgnoreCase))
                {
                    SendKeys.Send(bind.Text);
                    break;
                }
            }
        }

        // -----------------------------------------------------------------
        //  Зчитування та збереження JSON
        // -----------------------------------------------------------------
        private void LoadBinds()
        {
            try
            {
                if (File.Exists(bindsFilePath))
                {
                    string json = File.ReadAllText(bindsFilePath);
                    binds = JsonSerializer.Deserialize<List<BindModel>>(json) ?? new List<BindModel>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося завантажити binds.json: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RefreshGrid();
            }
        }

        private void SaveBinds()
        {
            try
            {
                string json = JsonSerializer.Serialize(binds, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(bindsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося зберегти binds.json: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Оновлення DataGridView
        private void RefreshGrid()
        {
            dataGridViewBinds.Rows.Clear();

            foreach (var bind in binds)
            {
                dataGridViewBinds.Rows.Add(bind.Key, bind.Text, bind.IsActive);
            }
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {

        }
    }

    // ---------------------------------------------------------------------
    //  МОДЕЛЬ ДАНИХ "БІНДА"
    // ---------------------------------------------------------------------
}