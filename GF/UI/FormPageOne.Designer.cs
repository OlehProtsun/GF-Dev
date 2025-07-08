// FormPageOne.Designer.cs
using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace GF.UI
{
    partial class FormPageOne
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2ComboBox comboBoxContainer;
        private Guna2Button buttonAddEmployee;

        private Guna2ComboBox comboBoxRemoveColumn;
        private Guna2Button buttonRemoveColumn;

        private Guna2DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

        private Panel panel1;
        private Panel panel2;

        private Guna2Elipse guna2Elipse1;
        private Guna2Elipse guna2Elipse2;

        private Guna2Button buttonGenerate;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPageOne));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.buttonAddEmployee = new Guna.UI2.WinForms.Guna2Button();
            this.comboBoxRemoveColumn = new Guna.UI2.WinForms.Guna2ComboBox();
            this.buttonRemoveColumn = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.comboBoxContainer = new Guna.UI2.WinForms.Guna2ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2GroupBox5 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2GroupBox4 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.buttonGenerate = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guna2Elipse3 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.guna2GroupBox6 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.SelectEmployeeComboBox = new Guna.UI2.WinForms.Guna2ComboBox();
            this.guna2GroupBox7 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox8 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.panel1.SuspendLayout();
            this.guna2GroupBox5.SuspendLayout();
            this.guna2GroupBox4.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.guna2GroupBox6.SuspendLayout();
            this.guna2GroupBox7.SuspendLayout();
            this.guna2GroupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 30;
            // 
            // buttonAddEmployee
            // 
            this.buttonAddEmployee.Animated = true;
            this.buttonAddEmployee.BackColor = System.Drawing.Color.Transparent;
            this.buttonAddEmployee.BorderRadius = 15;
            this.buttonAddEmployee.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(157)))), ((int)(((byte)(7)))));
            this.buttonAddEmployee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonAddEmployee.ForeColor = System.Drawing.Color.Black;
            this.buttonAddEmployee.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddEmployee.Image")));
            this.buttonAddEmployee.Location = new System.Drawing.Point(6, 7);
            this.buttonAddEmployee.Name = "buttonAddEmployee";
            this.buttonAddEmployee.ShadowDecoration.BorderRadius = 25;
            this.buttonAddEmployee.ShadowDecoration.Depth = 5;
            this.buttonAddEmployee.ShadowDecoration.Enabled = true;
            this.buttonAddEmployee.Size = new System.Drawing.Size(121, 36);
            this.buttonAddEmployee.TabIndex = 1;
            this.buttonAddEmployee.Text = "Add Employee";
            // 
            // comboBoxRemoveColumn
            // 
            this.comboBoxRemoveColumn.BackColor = System.Drawing.Color.Transparent;
            this.comboBoxRemoveColumn.BorderRadius = 15;
            this.comboBoxRemoveColumn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxRemoveColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRemoveColumn.FocusedColor = System.Drawing.Color.Empty;
            this.comboBoxRemoveColumn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxRemoveColumn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboBoxRemoveColumn.ItemHeight = 30;
            this.comboBoxRemoveColumn.Location = new System.Drawing.Point(5, 5);
            this.comboBoxRemoveColumn.Name = "comboBoxRemoveColumn";
            this.comboBoxRemoveColumn.Size = new System.Drawing.Size(113, 36);
            this.comboBoxRemoveColumn.TabIndex = 2;
            // 
            // buttonRemoveColumn
            // 
            this.buttonRemoveColumn.Animated = true;
            this.buttonRemoveColumn.BackColor = System.Drawing.Color.Transparent;
            this.buttonRemoveColumn.BorderRadius = 15;
            this.buttonRemoveColumn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonRemoveColumn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonRemoveColumn.ForeColor = System.Drawing.Color.Black;
            this.buttonRemoveColumn.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveColumn.Image")));
            this.buttonRemoveColumn.Location = new System.Drawing.Point(124, 5);
            this.buttonRemoveColumn.Name = "buttonRemoveColumn";
            this.buttonRemoveColumn.ShadowDecoration.BorderRadius = 25;
            this.buttonRemoveColumn.ShadowDecoration.Depth = 5;
            this.buttonRemoveColumn.ShadowDecoration.Enabled = true;
            this.buttonRemoveColumn.Size = new System.Drawing.Size(86, 36);
            this.buttonRemoveColumn.TabIndex = 3;
            this.buttonRemoveColumn.Text = "Remove";
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 30;
            // 
            // comboBoxContainer
            // 
            this.comboBoxContainer.BackColor = System.Drawing.Color.Transparent;
            this.comboBoxContainer.BorderRadius = 15;
            this.comboBoxContainer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContainer.FocusedColor = System.Drawing.Color.Empty;
            this.comboBoxContainer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboBoxContainer.ItemHeight = 30;
            this.comboBoxContainer.Location = new System.Drawing.Point(6, 7);
            this.comboBoxContainer.Name = "comboBoxContainer";
            this.comboBoxContainer.ShadowDecoration.Depth = 5;
            this.comboBoxContainer.Size = new System.Drawing.Size(126, 36);
            this.comboBoxContainer.TabIndex = 0;
            this.comboBoxContainer.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonth_SelectedIndexChanged_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.guna2GroupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1071, 91);
            this.panel1.TabIndex = 1;
            // 
            // guna2GroupBox5
            // 
            this.guna2GroupBox5.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox5.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox5.BorderRadius = 30;
            this.guna2GroupBox5.Controls.Add(this.guna2GroupBox8);
            this.guna2GroupBox5.Controls.Add(this.guna2GroupBox4);
            this.guna2GroupBox5.Controls.Add(this.guna2GroupBox1);
            this.guna2GroupBox5.Controls.Add(this.guna2GroupBox2);
            this.guna2GroupBox5.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox5.FillColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox5.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox5.Location = new System.Drawing.Point(4, 3);
            this.guna2GroupBox5.Name = "guna2GroupBox5";
            this.guna2GroupBox5.Size = new System.Drawing.Size(973, 80);
            this.guna2GroupBox5.TabIndex = 9;
            this.guna2GroupBox5.Click += new System.EventHandler(this.guna2GroupBox5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(23, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Select a month";
            // 
            // guna2GroupBox4
            // 
            this.guna2GroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox4.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.BorderRadius = 20;
            this.guna2GroupBox4.Controls.Add(this.buttonGenerate);
            this.guna2GroupBox4.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox4.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox4.Location = new System.Drawing.Point(715, 19);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.Size = new System.Drawing.Size(102, 46);
            this.guna2GroupBox4.TabIndex = 7;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Animated = true;
            this.buttonGenerate.BackColor = System.Drawing.Color.Transparent;
            this.buttonGenerate.BorderRadius = 15;
            this.buttonGenerate.FillColor = System.Drawing.Color.LightGreen;
            this.buttonGenerate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonGenerate.ForeColor = System.Drawing.Color.Black;
            this.buttonGenerate.Image = ((System.Drawing.Image)(resources.GetObject("buttonGenerate.Image")));
            this.buttonGenerate.Location = new System.Drawing.Point(5, 5);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.ShadowDecoration.BorderRadius = 25;
            this.buttonGenerate.ShadowDecoration.Depth = 5;
            this.buttonGenerate.ShadowDecoration.Enabled = true;
            this.buttonGenerate.Size = new System.Drawing.Size(91, 36);
            this.buttonGenerate.TabIndex = 0;
            this.buttonGenerate.Text = "Next step";
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox3.BorderColor = System.Drawing.Color.Black;
            this.guna2GroupBox3.BorderRadius = 20;
            this.guna2GroupBox3.Controls.Add(this.comboBoxContainer);
            this.guna2GroupBox3.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox3.FillColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox3.Location = new System.Drawing.Point(8, 8);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(138, 49);
            this.guna2GroupBox3.TabIndex = 8;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox1.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.BorderRadius = 20;
            this.guna2GroupBox1.Controls.Add(this.comboBoxRemoveColumn);
            this.guna2GroupBox1.Controls.Add(this.buttonRemoveColumn);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox1.Location = new System.Drawing.Point(492, 19);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(217, 46);
            this.guna2GroupBox1.TabIndex = 6;
            // 
            // guna2GroupBox2
            // 
            this.guna2GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox2.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox2.BorderRadius = 26;
            this.guna2GroupBox2.Controls.Add(this.guna2GroupBox7);
            this.guna2GroupBox2.Controls.Add(this.label2);
            this.guna2GroupBox2.Controls.Add(this.guna2GroupBox6);
            this.guna2GroupBox2.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox2.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox2.Location = new System.Drawing.Point(169, 8);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Size = new System.Drawing.Size(317, 64);
            this.guna2GroupBox2.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 91);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1071, 786);
            this.panel2.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView.ColumnHeadersVisible = false;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.GridColor = System.Drawing.Color.DimGray;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 28;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(1071, 786);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridView.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridView.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridView.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridView.ThemeStyle.BackColor = System.Drawing.Color.Gainsboro;
            this.dataGridView.ThemeStyle.GridColor = System.Drawing.Color.DimGray;
            this.dataGridView.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dataGridView.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridView.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.ThemeStyle.HeaderStyle.Height = 23;
            this.dataGridView.ThemeStyle.ReadOnly = false;
            this.dataGridView.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dataGridView.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dataGridView.ThemeStyle.RowsStyle.Height = 28;
            this.dataGridView.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.dataGridView.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // guna2Elipse3
            // 
            this.guna2Elipse3.BorderRadius = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(22, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Select an employee";
            // 
            // guna2GroupBox6
            // 
            this.guna2GroupBox6.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox6.BorderColor = System.Drawing.Color.Black;
            this.guna2GroupBox6.BorderRadius = 20;
            this.guna2GroupBox6.Controls.Add(this.SelectEmployeeComboBox);
            this.guna2GroupBox6.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox6.FillColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox6.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox6.Location = new System.Drawing.Point(7, 8);
            this.guna2GroupBox6.Name = "guna2GroupBox6";
            this.guna2GroupBox6.Size = new System.Drawing.Size(164, 49);
            this.guna2GroupBox6.TabIndex = 10;
            // 
            // SelectEmployeeComboBox
            // 
            this.SelectEmployeeComboBox.BackColor = System.Drawing.Color.Transparent;
            this.SelectEmployeeComboBox.BorderRadius = 15;
            this.SelectEmployeeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.SelectEmployeeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectEmployeeComboBox.FocusedColor = System.Drawing.Color.Empty;
            this.SelectEmployeeComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.SelectEmployeeComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.SelectEmployeeComboBox.ItemHeight = 30;
            this.SelectEmployeeComboBox.Location = new System.Drawing.Point(6, 7);
            this.SelectEmployeeComboBox.Name = "SelectEmployeeComboBox";
            this.SelectEmployeeComboBox.ShadowDecoration.Depth = 5;
            this.SelectEmployeeComboBox.Size = new System.Drawing.Size(152, 36);
            this.SelectEmployeeComboBox.TabIndex = 0;
            // 
            // guna2GroupBox7
            // 
            this.guna2GroupBox7.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox7.BorderColor = System.Drawing.Color.Black;
            this.guna2GroupBox7.BorderRadius = 20;
            this.guna2GroupBox7.Controls.Add(this.buttonAddEmployee);
            this.guna2GroupBox7.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox7.FillColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox7.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox7.Location = new System.Drawing.Point(176, 8);
            this.guna2GroupBox7.Name = "guna2GroupBox7";
            this.guna2GroupBox7.Size = new System.Drawing.Size(134, 49);
            this.guna2GroupBox7.TabIndex = 11;
            // 
            // guna2GroupBox8
            // 
            this.guna2GroupBox8.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox8.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.BorderRadius = 26;
            this.guna2GroupBox8.Controls.Add(this.label1);
            this.guna2GroupBox8.Controls.Add(this.guna2GroupBox3);
            this.guna2GroupBox8.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox8.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox8.Location = new System.Drawing.Point(8, 8);
            this.guna2GroupBox8.Name = "guna2GroupBox8";
            this.guna2GroupBox8.Size = new System.Drawing.Size(155, 64);
            this.guna2GroupBox8.TabIndex = 12;
            // 
            // FormPageOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1071, 877);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormPageOne";
            this.Text = "Employee Day Table";
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox5.ResumeLayout(false);
            this.guna2GroupBox4.ResumeLayout(false);
            this.guna2GroupBox3.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2GroupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.guna2GroupBox6.ResumeLayout(false);
            this.guna2GroupBox7.ResumeLayout(false);
            this.guna2GroupBox8.ResumeLayout(false);
            this.guna2GroupBox8.PerformLayout();
            this.ResumeLayout(false);

        }
        private Guna2Elipse guna2Elipse3;
        private Guna2GroupBox guna2GroupBox1;
        private Guna2GroupBox guna2GroupBox3;
        private Guna2GroupBox guna2GroupBox2;
        private Guna2GroupBox guna2GroupBox4;
        private Guna2GroupBox guna2GroupBox5;
        private Label label1;
        private Label label2;
        private Guna2GroupBox guna2GroupBox6;
        private Guna2ComboBox SelectEmployeeComboBox;
        private Guna2GroupBox guna2GroupBox7;
        private Guna2GroupBox guna2GroupBox8;
    }
}
