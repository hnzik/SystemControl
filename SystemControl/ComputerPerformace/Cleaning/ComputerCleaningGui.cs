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

namespace SystemControl.ComputerPerformace.Cleaning
{
    public partial class ComputerCleaningGui : Form
    {
        
  

        public ComputerCleaningGui()
        {
            InitializeComponent();
        }


        private void setPictureAndLabel(Panel pan)
        {
            if (pan.BackgroundImage != null)
            {
                var pic = new Bitmap(pan.BackgroundImage, new Size(120, 120));
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

        private void ComputerCleaningGui_Load(object sender, EventArgs e)
        {
            var enumeRator = this.Controls.GetEnumerator();
            while (enumeRator.MoveNext())
            {
                this.setPictureAndLabel((Panel)enumeRator.Current);
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.setupButton((Button)this.panel1.Controls[0]);
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

        private void panel1_Click(object sender, EventArgs e)
        {
            CleaningOption options = new CleaningOption();
            options.CleanHistory = true;
            options.CleanInternetCache = true;
            options.CleanInternetCookies = true;
            options.CleanLocalTempFile = true;
            options.CleanTempFiles = true;
            options.CleanWindowsUpdateFiles = true;
            options.EmptyRecycleBin = true;
            options.CleanEventLogs = true;

            Thread t = new Thread(() =>
            {
                var activeCleaningGui = new ActiveCleaningGui(options);
                activeCleaningGui.ShowDialog();
            }) ;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void panel3_click(object sender, EventArgs e) {
            SettingsGui settings = new SettingsGui();
            settings.Show();
        }
    }
}
