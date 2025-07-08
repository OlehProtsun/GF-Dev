namespace GF
{
    partial class ScheduleManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleManagerForm));
            this.flpSchedules = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnSwapShift = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox4 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnOpenSchedules = new Guna.UI2.WinForms.Guna2Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnStopSwapShift = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2GroupBox4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpSchedules
            // 
            this.flpSchedules.BackColor = System.Drawing.Color.Gainsboro;
            this.flpSchedules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSchedules.Location = new System.Drawing.Point(0, 0);
            this.flpSchedules.Name = "flpSchedules";
            this.flpSchedules.Size = new System.Drawing.Size(800, 702);
            this.flpSchedules.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.guna2GroupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 62);
            this.panel1.TabIndex = 1;
            // 
            // guna2GroupBox2
            // 
            this.guna2GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox2.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox2.BorderRadius = 25;
            this.guna2GroupBox2.Controls.Add(this.guna2GroupBox1);
            this.guna2GroupBox2.Controls.Add(this.guna2GroupBox4);
            this.guna2GroupBox2.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox2.FillColor = System.Drawing.Color.DarkGray;
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2GroupBox2.Location = new System.Drawing.Point(2, 3);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Size = new System.Drawing.Size(393, 53);
            this.guna2GroupBox2.TabIndex = 7;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.BorderRadius = 20;
            this.guna2GroupBox1.Controls.Add(this.btnStopSwapShift);
            this.guna2GroupBox1.Controls.Add(this.btnSwapShift);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(126, 5);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(261, 42);
            this.guna2GroupBox1.TabIndex = 9;
            // 
            // btnSwapShift
            // 
            this.btnSwapShift.Animated = true;
            this.btnSwapShift.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapShift.BorderRadius = 15;
            this.btnSwapShift.FillColor = System.Drawing.Color.GreenYellow;
            this.btnSwapShift.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSwapShift.ForeColor = System.Drawing.Color.Black;
            this.btnSwapShift.Image = ((System.Drawing.Image)(resources.GetObject("btnSwapShift.Image")));
            this.btnSwapShift.Location = new System.Drawing.Point(5, 5);
            this.btnSwapShift.Name = "btnSwapShift";
            this.btnSwapShift.ShadowDecoration.BorderRadius = 25;
            this.btnSwapShift.ShadowDecoration.Depth = 2;
            this.btnSwapShift.ShadowDecoration.Enabled = true;
            this.btnSwapShift.Size = new System.Drawing.Size(123, 32);
            this.btnSwapShift.TabIndex = 8;
            this.btnSwapShift.Text = "Start Swap shift";
            this.btnSwapShift.Click += new System.EventHandler(this.btnSwapShift_Click);
            // 
            // guna2GroupBox4
            // 
            this.guna2GroupBox4.BorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.BorderRadius = 20;
            this.guna2GroupBox4.Controls.Add(this.btnOpenSchedules);
            this.guna2GroupBox4.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.FillColor = System.Drawing.Color.LightGray;
            this.guna2GroupBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2GroupBox4.Location = new System.Drawing.Point(5, 5);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.Size = new System.Drawing.Size(115, 42);
            this.guna2GroupBox4.TabIndex = 6;
            // 
            // btnOpenSchedules
            // 
            this.btnOpenSchedules.Animated = true;
            this.btnOpenSchedules.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSchedules.BorderRadius = 15;
            this.btnOpenSchedules.FillColor = System.Drawing.Color.DeepSkyBlue;
            this.btnOpenSchedules.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenSchedules.ForeColor = System.Drawing.Color.Black;
            this.btnOpenSchedules.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenSchedules.Image")));
            this.btnOpenSchedules.Location = new System.Drawing.Point(5, 5);
            this.btnOpenSchedules.Name = "btnOpenSchedules";
            this.btnOpenSchedules.ShadowDecoration.BorderRadius = 25;
            this.btnOpenSchedules.ShadowDecoration.Depth = 2;
            this.btnOpenSchedules.ShadowDecoration.Enabled = true;
            this.btnOpenSchedules.Size = new System.Drawing.Size(105, 32);
            this.btnOpenSchedules.TabIndex = 8;
            this.btnOpenSchedules.Text = "Open multi";
            this.btnOpenSchedules.Click += new System.EventHandler(this.btnOpenSchedules_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flpSchedules);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 702);
            this.panel2.TabIndex = 2;
            // 
            // btnStopSwapShift
            // 
            this.btnStopSwapShift.Animated = true;
            this.btnStopSwapShift.BackColor = System.Drawing.Color.Transparent;
            this.btnStopSwapShift.BorderRadius = 15;
            this.btnStopSwapShift.Enabled = false;
            this.btnStopSwapShift.FillColor = System.Drawing.Color.LightCoral;
            this.btnStopSwapShift.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStopSwapShift.ForeColor = System.Drawing.Color.Black;
            this.btnStopSwapShift.Image = ((System.Drawing.Image)(resources.GetObject("btnStopSwapShift.Image")));
            this.btnStopSwapShift.Location = new System.Drawing.Point(134, 5);
            this.btnStopSwapShift.Name = "btnStopSwapShift";
            this.btnStopSwapShift.ShadowDecoration.BorderRadius = 25;
            this.btnStopSwapShift.ShadowDecoration.Depth = 2;
            this.btnStopSwapShift.ShadowDecoration.Enabled = true;
            this.btnStopSwapShift.Size = new System.Drawing.Size(123, 32);
            this.btnStopSwapShift.TabIndex = 9;
            this.btnStopSwapShift.Text = "Stop Swap shift";
            this.btnStopSwapShift.Click += new System.EventHandler(this.btnStopSwapShift_Click);
            // 
            // ScheduleManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 764);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScheduleManagerForm";
            this.Text = "ScheduleManagerForm";
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpSchedules;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox2;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox4;
        private Guna.UI2.WinForms.Guna2Button btnOpenSchedules;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2Button btnSwapShift;
        private Guna.UI2.WinForms.Guna2Button btnStopSwapShift;
    }
}