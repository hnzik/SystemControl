using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.SystemSettings
{
    public partial class SystemSettingsGui : Form
    {
        Form parent = null;
        public SystemSettingsGui(Form parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void setPictureAndLabel(Panel pan)
        {
            if (pan.BackgroundImage != null)
            {
                var pic = new Bitmap(pan.BackgroundImage, new Size(60, 60));
                pan.BackgroundImage = pic;
            }
        }
        private void setupButton(Button button)
        {
            if (button.Image != null)
            {
                var pic = new Bitmap(button.Image, new Size(15, 15));
                button.Image = pic;
            }
            Color newBackColor = Color.FromArgb(255, 255, 255);
            button.BackColor = newBackColor;
            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }

        private void SystemSettingsGui_Load(object sender, EventArgs e)
        {
            var enumeRator = this.Controls.GetEnumerator();
            while (enumeRator.MoveNext())
            {
                this.setPictureAndLabel((Panel)enumeRator.Current);
            }
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            setupButton((Button)this.panel1.Controls[0]);
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            Color newBackColor = Color.FromArgb(229, 243, 255);
            pan.BackColor = newBackColor;
            if (pan.Controls.Count > 0)
            {
                for (int i = 0; i < pan.Controls.Count; i++)
                {
                    if (pan.Controls[i] is MetroFramework.Controls.MetroLabel)
                    {
                        MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)pan.Controls[i];
                        label.BackColor = newBackColor;
                    }
                    else if (pan.Controls[i] is Button)
                    {
                        Button button = (Button)pan.Controls[i];
                        button.BackColor = newBackColor;
                    }
                }
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            Color newBackColor = Color.FromArgb(255, 255, 255);
            pan.BackColor = newBackColor;
            if (pan.Controls.Count > 0)
            {
                for (int i = 0; i < pan.Controls.Count; i++)
                {
                    if (pan.Controls[i] is MetroFramework.Controls.MetroLabel)
                    {
                        MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)pan.Controls[i];
                        label.BackColor = newBackColor;
                    }
                    else if (pan.Controls[i] is Button)
                    {
                        Button button = (Button)pan.Controls[i];
                        button.BackColor = newBackColor;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.parent.Show();
        }

        private void SystemSettingsGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            this.Hide();
            UniversalSettingsGui universalGui = new UniversalSettingsGui(1);
            universalGui.StartPosition = FormStartPosition.Manual;
            universalGui.Location = this.Location;
            universalGui.Show(this);
            Panel closeButton = (Panel)universalGui.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                universalGui.Dispose();
            };

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }


        private void panel3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            UniversalSettingsGui universalGui = new UniversalSettingsGui(2);
            universalGui.StartPosition = FormStartPosition.Manual;
            universalGui.Location = this.Location;
            universalGui.Show(this);
            Panel closeButton = (Panel)universalGui.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                universalGui.Dispose();
            };
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Hide();
            UniversalSettingsGui universalGui = new UniversalSettingsGui(3);
            universalGui.StartPosition = FormStartPosition.Manual;
            universalGui.Location = this.Location;
            universalGui.Show(this);
            Panel closeButton = (Panel)universalGui.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                universalGui.Dispose();
            };
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            UniversalSettingsGui universalGui = new UniversalSettingsGui(4);
            universalGui.StartPosition = FormStartPosition.Manual;
            universalGui.Location = this.Location;
            universalGui.Show(this);
            Panel closeButton = (Panel)universalGui.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                universalGui.Dispose();
            };
        }
    }
}
