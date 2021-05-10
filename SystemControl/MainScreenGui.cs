using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SystemControl
{
    public partial class MainScreenGui : Form
    {
        public MainScreenGui()
        {
            InitializeComponent();
        }


        private void setPictureAndLabel(Panel pan) {
            if (pan.BackgroundImage != null) {
                var pic = new Bitmap(pan.BackgroundImage, new Size(80, 80));
                pan.BackgroundImage = pic;
                if (pan.Controls.Count > 0) {
                    MetroFramework.Controls.MetroLabel metroLabel = (MetroFramework.Controls.MetroLabel)pan.Controls[0];
                   // metroLabel.Location = new Point(, 10);
                }

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            var enumeRator = this.Controls.GetEnumerator();
            while (enumeRator.MoveNext())
            {
                this.setPictureAndLabel((Panel)enumeRator.Current);
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
                      Color.Black, 0, ButtonBorderStyle.Dotted,
                      Color.Black, 0, ButtonBorderStyle.Solid,
                      Color.Black, 0, ButtonBorderStyle.Solid,
                      Color.Black, 0, ButtonBorderStyle.Dotted);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel2.ClientRectangle,
               Color.Black, 0, ButtonBorderStyle.Dotted,
               Color.Black, 0, ButtonBorderStyle.Solid,
               Color.Black, 0, ButtonBorderStyle.Solid,
               Color.Black, 0, ButtonBorderStyle.Dotted);
        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel3.ClientRectangle,
                           Color.Black, 0, ButtonBorderStyle.Dotted,
                           Color.Black, 0, ButtonBorderStyle.Solid,
                           Color.Black, 0, ButtonBorderStyle.Solid,
                           Color.Black, 0, ButtonBorderStyle.Dotted);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel3.ClientRectangle,
                     Color.Black, 0, ButtonBorderStyle.Dotted,
                     Color.Black, 0, ButtonBorderStyle.Solid,
                     Color.Black, 0, ButtonBorderStyle.Solid,
                     Color.Black, 0, ButtonBorderStyle.Dotted);
        }

        private void OnPanelMouseEnter(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            Color newBackColor = Color.FromArgb(229, 243, 255);
            pan.BackColor = newBackColor;
            if (pan.Controls.Count > 0) {
                MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)pan.Controls[0];
                label.BackColor = newBackColor;
            }
        }

        private void OnPanelMouseLeave(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            Color newBackColor = Color.FromArgb(255, 255, 255);
            pan.BackColor = newBackColor;
            if (pan.Controls.Count > 0)
            {
                MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)pan.Controls[0];
                label.BackColor = newBackColor;
            }

        }
        private void panel3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                FileBrowser.FileBrowserGui fileBrowserGui = new FileBrowser.FileBrowserGui();
                fileBrowserGui.ShowDialog();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void OnPanelMouseDown(object sender, MouseEventArgs e)
        {
            Panel pan = (Panel)sender;
            Color newBackColor = Color.FromArgb(205, 232, 255);
            pan.BackColor = newBackColor;
            if (pan.Controls.Count > 0)
            {
                MetroFramework.Controls.MetroLabel label = (MetroFramework.Controls.MetroLabel)pan.Controls[0];
                label.BackColor = newBackColor;
            }
        }

        private void OnPanelMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ComputerPerformace.ComputerPerformaceGui computerPerformaceGui = new ComputerPerformace.ComputerPerformaceGui(this);
            computerPerformaceGui.StartPosition = FormStartPosition.Manual;
            computerPerformaceGui.Location = this.Location;
            computerPerformaceGui.Show(this);           
            //Není to nejlepší způsob ale už jsem neměl moc času
            Button closeButton = (Button)computerPerformaceGui.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                computerPerformaceGui.Dispose();
            };
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            this.Hide();
            SystemControl.SystemSettings.SystemSettingsGui systemSettingsGui = new SystemControl.SystemSettings.SystemSettingsGui(this);
            systemSettingsGui.StartPosition = FormStartPosition.Manual;
            systemSettingsGui.Location = this.Location;
            systemSettingsGui.Show();
            //Není to nejlepší způsob ale už jsem neměl moc času
            Button closeButton = (Button)systemSettingsGui.Controls[3].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                systemSettingsGui.Dispose();
            };
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Hide();
             DiskControl.DiskControlMain diskControlMain = new DiskControl.DiskControlMain(1);
            diskControlMain.StartPosition = FormStartPosition.Manual;
            diskControlMain.Location = this.Location;
            diskControlMain.Show(this);
            Panel closeButton = (Panel)diskControlMain.Controls[0].Controls[0];
            closeButton.Click += (s, args) =>
            {
                this.Show();
                diskControlMain.Dispose();
            };
        }
    }
}
