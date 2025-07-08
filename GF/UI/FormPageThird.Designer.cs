using Guna.UI2.WinForms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GF.UI
{
    public partial class FormPageThird
    {
        private IContainer components = null;
        private Guna2TextBox textBoxKey;
        private Guna2TextBox textBoxText;
        private Guna2Button buttonAddBind;
        private Guna2DataGridView dataGridViewBinds;

        // --- колонки ---
        private DataGridViewTextBoxColumn KeyColumn;
        private DataGridViewTextBoxColumn TextColumn;
        private DataGridViewCheckBoxColumn IsActiveColumn;
        private DataGridViewButtonColumn DeleteColumn;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPageThird));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBoxKey = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBoxText = new Guna.UI2.WinForms.Guna2TextBox();
            this.buttonAddBind = new Guna.UI2.WinForms.Guna2Button();
            this.dataGridViewBinds = new Guna.UI2.WinForms.Guna2DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2GroupBox6 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox9 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox8 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox7 = new Guna.UI2.WinForms.Guna2GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBinds)).BeginInit();
            this.guna2GroupBox6.SuspendLayout();
            this.guna2GroupBox9.SuspendLayout();
            this.guna2GroupBox8.SuspendLayout();
            this.guna2GroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxKey
            // 
            this.textBoxKey.Animated = true;
            this.textBoxKey.BackColor = System.Drawing.Color.Transparent;
            this.textBoxKey.BorderRadius = 15;
            this.textBoxKey.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxKey.DefaultText = "";
            this.textBoxKey.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxKey.Location = new System.Drawing.Point(4, 5);
            this.textBoxKey.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.PlaceholderText = "Key";
            this.textBoxKey.SelectedText = "";
            this.textBoxKey.ShadowDecoration.BorderRadius = 25;
            this.textBoxKey.ShadowDecoration.Depth = 2;
            this.textBoxKey.ShadowDecoration.Enabled = true;
            this.textBoxKey.Size = new System.Drawing.Size(64, 33);
            this.textBoxKey.TabIndex = 0;
            // 
            // textBoxText
            // 
            this.textBoxText.Animated = true;
            this.textBoxText.BackColor = System.Drawing.Color.Transparent;
            this.textBoxText.BorderRadius = 15;
            this.textBoxText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxText.DefaultText = "";
            this.textBoxText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxText.Location = new System.Drawing.Point(4, 5);
            this.textBoxText.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.PlaceholderText = "Text";
            this.textBoxText.SelectedText = "";
            this.textBoxText.ShadowDecoration.BorderRadius = 25;
            this.textBoxText.ShadowDecoration.Depth = 2;
            this.textBoxText.ShadowDecoration.Enabled = true;
            this.textBoxText.Size = new System.Drawing.Size(182, 33);
            this.textBoxText.TabIndex = 1;
            // 
            // buttonAddBind
            // 
            this.buttonAddBind.Animated = true;
            this.buttonAddBind.BackColor = System.Drawing.Color.Transparent;
            this.buttonAddBind.BorderRadius = 15;
            this.buttonAddBind.FillColor = System.Drawing.SystemColors.MenuHighlight;
            this.buttonAddBind.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonAddBind.ForeColor = System.Drawing.Color.Black;
            this.buttonAddBind.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddBind.Image")));
            this.buttonAddBind.Location = new System.Drawing.Point(5, 5);
            this.buttonAddBind.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddBind.Name = "buttonAddBind";
            this.buttonAddBind.ShadowDecoration.BorderRadius = 25;
            this.buttonAddBind.ShadowDecoration.Depth = 5;
            this.buttonAddBind.ShadowDecoration.Enabled = true;
            this.buttonAddBind.Size = new System.Drawing.Size(89, 33);
            this.buttonAddBind.TabIndex = 2;
            this.buttonAddBind.Text = "Add Bind";
            this.buttonAddBind.Click += new System.EventHandler(this.buttonAddBind_Click);
            // 
            // dataGridViewBinds
            // 
            this.dataGridViewBinds.AllowUserToAddRows = false;
            this.dataGridViewBinds.AllowUserToDeleteRows = false;
            this.dataGridViewBinds.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewBinds.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBinds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBinds.ColumnHeadersHeight = 40;
            this.dataGridViewBinds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Text,
            this.IsActive,
            this.Delete});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBinds.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewBinds.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewBinds.Location = new System.Drawing.Point(5, 59);
            this.dataGridViewBinds.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewBinds.MultiSelect = false;
            this.dataGridViewBinds.Name = "dataGridViewBinds";
            this.dataGridViewBinds.RowHeadersVisible = false;
            this.dataGridViewBinds.RowTemplate.Height = 32;
            this.dataGridViewBinds.Size = new System.Drawing.Size(387, 858);
            this.dataGridViewBinds.TabIndex = 3;
            this.dataGridViewBinds.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBinds.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewBinds.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewBinds.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewBinds.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewBinds.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBinds.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewBinds.ThemeStyle.HeaderStyle.Height = 40;
            this.dataGridViewBinds.ThemeStyle.ReadOnly = false;
            this.dataGridViewBinds.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBinds.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewBinds.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dataGridViewBinds.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dataGridViewBinds.ThemeStyle.RowsStyle.Height = 32;
            this.dataGridViewBinds.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewBinds.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewBinds.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBinds_CellContentClick);
            // 
            // Key
            // 
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            // 
            // Text
            // 
            this.Text.HeaderText = "Text";
            this.Text.Name = "Text";
            this.Text.ReadOnly = true;
            // 
            // IsActive
            // 
            this.IsActive.HeaderText = "Active";
            this.IsActive.Name = "IsActive";
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.Text = "X";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 25;
            // 
            // guna2GroupBox6
            // 
            this.guna2GroupBox6.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox6.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox6.BorderRadius = 25;
            this.guna2GroupBox6.Controls.Add(this.guna2GroupBox9);
            this.guna2GroupBox6.Controls.Add(this.guna2GroupBox8);
            this.guna2GroupBox6.Controls.Add(this.guna2GroupBox7);
            this.guna2GroupBox6.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox6.FillColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox6.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox6.Location = new System.Drawing.Point(5, 2);
            this.guna2GroupBox6.Name = "guna2GroupBox6";
            this.guna2GroupBox6.Size = new System.Drawing.Size(386, 52);
            this.guna2GroupBox6.TabIndex = 11;
            // 
            // guna2GroupBox9
            // 
            this.guna2GroupBox9.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox9.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox9.BorderRadius = 20;
            this.guna2GroupBox9.Controls.Add(this.buttonAddBind);
            this.guna2GroupBox9.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox9.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox9.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox9.Location = new System.Drawing.Point(281, 4);
            this.guna2GroupBox9.Name = "guna2GroupBox9";
            this.guna2GroupBox9.ShadowDecoration.BorderRadius = 25;
            this.guna2GroupBox9.ShadowDecoration.Depth = 5;
            this.guna2GroupBox9.ShadowDecoration.Enabled = true;
            this.guna2GroupBox9.Size = new System.Drawing.Size(99, 43);
            this.guna2GroupBox9.TabIndex = 11;
            // 
            // guna2GroupBox8
            // 
            this.guna2GroupBox8.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox8.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.BorderRadius = 20;
            this.guna2GroupBox8.Controls.Add(this.textBoxText);
            this.guna2GroupBox8.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox8.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox8.Location = new System.Drawing.Point(84, 4);
            this.guna2GroupBox8.Name = "guna2GroupBox8";
            this.guna2GroupBox8.ShadowDecoration.BorderRadius = 25;
            this.guna2GroupBox8.ShadowDecoration.Depth = 5;
            this.guna2GroupBox8.ShadowDecoration.Enabled = true;
            this.guna2GroupBox8.Size = new System.Drawing.Size(191, 43);
            this.guna2GroupBox8.TabIndex = 10;
            // 
            // guna2GroupBox7
            // 
            this.guna2GroupBox7.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox7.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox7.BorderRadius = 20;
            this.guna2GroupBox7.Controls.Add(this.textBoxKey);
            this.guna2GroupBox7.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox7.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox7.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox7.Location = new System.Drawing.Point(6, 4);
            this.guna2GroupBox7.Name = "guna2GroupBox7";
            this.guna2GroupBox7.ShadowDecoration.BorderRadius = 25;
            this.guna2GroupBox7.ShadowDecoration.Depth = 5;
            this.guna2GroupBox7.ShadowDecoration.Enabled = true;
            this.guna2GroupBox7.Size = new System.Drawing.Size(73, 43);
            this.guna2GroupBox7.TabIndex = 9;
            // 
            // FormPageThird
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(410, 679);
            this.Controls.Add(this.guna2GroupBox6);
            this.Controls.Add(this.dataGridViewBinds);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormPageThird";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBinds)).EndInit();
            this.guna2GroupBox6.ResumeLayout(false);
            this.guna2GroupBox9.ResumeLayout(false);
            this.guna2GroupBox8.ResumeLayout(false);
            this.guna2GroupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DataGridViewTextBoxColumn Key;
        private DataGridViewTextBoxColumn Text;
        private DataGridViewCheckBoxColumn IsActive;
        private DataGridViewButtonColumn Delete;
        private Guna2Elipse guna2Elipse1;
        private Guna2GroupBox guna2GroupBox6;
        private Guna2GroupBox guna2GroupBox9;
        private Guna2GroupBox guna2GroupBox8;
        private Guna2GroupBox guna2GroupBox7;
    }
}
