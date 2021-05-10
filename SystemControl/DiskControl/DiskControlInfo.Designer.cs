﻿
namespace SystemControl.DiskControl
{
    partial class DiskControlInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox2 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox3 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox4 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox5 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox6 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox7 = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 30);
            this.label1.TabIndex = 6;
            this.label1.Text = "Disk info";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(17, 95);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(44, 19);
            this.metroLabel1.TabIndex = 8;
            this.metroLabel1.Text = "Label:";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel2.Location = new System.Drawing.Point(17, 135);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(48, 19);
            this.metroLabel2.TabIndex = 9;
            this.metroLabel2.Text = "Name:";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(17, 173);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(128, 19);
            this.metroLabel3.TabIndex = 10;
            this.metroLabel3.Text = "Manufacture name:";
            this.metroLabel3.Click += new System.EventHandler(this.metroLabel3_Click);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel4.Location = new System.Drawing.Point(17, 256);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(96, 19);
            this.metroLabel4.TabIndex = 11;
            this.metroLabel4.Text = "Interface type:";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel6.Location = new System.Drawing.Point(20, 328);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(76, 19);
            this.metroLabel6.TabIndex = 13;
            this.metroLabel6.Text = "Free space:";
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel7.Location = new System.Drawing.Point(17, 213);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(96, 19);
            this.metroLabel7.TabIndex = 14;
            this.metroLabel7.Text = "Serial number:";
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel8.Location = new System.Drawing.Point(17, 294);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(79, 19);
            this.metroLabel8.TabIndex = 15;
            this.metroLabel8.Text = "Total space:";
            // 
            // metroTextBox1
            // 
            this.metroTextBox1.Location = new System.Drawing.Point(151, 95);
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox1.TabIndex = 16;
            this.metroTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.metroTextBox1_KeyDown);
            this.metroTextBox1.Leave += new System.EventHandler(this.metroTextBox1_Leave);
            // 
            // metroTextBox2
            // 
            this.metroTextBox2.Location = new System.Drawing.Point(151, 135);
            this.metroTextBox2.Name = "metroTextBox2";
            this.metroTextBox2.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox2.TabIndex = 17;
            this.metroTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.metroTextBox1_KeyDown);
            this.metroTextBox2.Leave += new System.EventHandler(this.metroTextBox1_Leave);
            // 
            // metroTextBox3
            // 
            this.metroTextBox3.Enabled = false;
            this.metroTextBox3.Location = new System.Drawing.Point(151, 173);
            this.metroTextBox3.Name = "metroTextBox3";
            this.metroTextBox3.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox3.TabIndex = 23;
            // 
            // metroTextBox4
            // 
            this.metroTextBox4.Enabled = false;
            this.metroTextBox4.Location = new System.Drawing.Point(151, 213);
            this.metroTextBox4.Name = "metroTextBox4";
            this.metroTextBox4.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox4.TabIndex = 24;
            // 
            // metroTextBox5
            // 
            this.metroTextBox5.Enabled = false;
            this.metroTextBox5.Location = new System.Drawing.Point(151, 252);
            this.metroTextBox5.Name = "metroTextBox5";
            this.metroTextBox5.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox5.TabIndex = 25;
            // 
            // metroTextBox6
            // 
            this.metroTextBox6.Enabled = false;
            this.metroTextBox6.Location = new System.Drawing.Point(151, 290);
            this.metroTextBox6.Name = "metroTextBox6";
            this.metroTextBox6.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox6.TabIndex = 26;
            // 
            // metroTextBox7
            // 
            this.metroTextBox7.Enabled = false;
            this.metroTextBox7.Location = new System.Drawing.Point(151, 328);
            this.metroTextBox7.Name = "metroTextBox7";
            this.metroTextBox7.Size = new System.Drawing.Size(187, 23);
            this.metroTextBox7.TabIndex = 27;
            // 
            // DiskControlInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.metroTextBox7);
            this.Controls.Add(this.metroTextBox6);
            this.Controls.Add(this.metroTextBox5);
            this.Controls.Add(this.metroTextBox4);
            this.Controls.Add(this.metroTextBox3);
            this.Controls.Add(this.metroTextBox2);
            this.Controls.Add(this.metroTextBox1);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.metroLabel7);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Enabled = false;
            this.Name = "DiskControlInfo";
            this.Size = new System.Drawing.Size(599, 453);
            this.Load += new System.EventHandler(this.DiskControlInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroTextBox metroTextBox1;
        private MetroFramework.Controls.MetroTextBox metroTextBox2;
        private MetroFramework.Controls.MetroTextBox metroTextBox3;
        private MetroFramework.Controls.MetroTextBox metroTextBox4;
        private MetroFramework.Controls.MetroTextBox metroTextBox5;
        private MetroFramework.Controls.MetroTextBox metroTextBox6;
        private MetroFramework.Controls.MetroTextBox metroTextBox7;
    }
}