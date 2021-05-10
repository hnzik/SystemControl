using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.ComputerPerformace.Cleaning
{
    public partial class SettingsGui : Form
    {
        public SettingsGui()
        {
            InitializeComponent();
        }

        bool checkIfAtleastOnCheckBoxIsChecked() {          
            foreach (var control in this.Controls) {
                if (control is MetroFramework.Controls.MetroCheckBox) {
                    MetroFramework.Controls.MetroCheckBox checkBox = (MetroFramework.Controls.MetroCheckBox)control;
                    if (checkBox.Checked)
                        return true;
                }
            }
            return false;
        }

        private void SettingsGui_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (checkIfAtleastOnCheckBoxIsChecked()) {
                CleaningOption cleaningOption = new CleaningOption();
                cleaningOption.CleanLocalTempFile = this.metroCheckBox1.Checked;
                cleaningOption.CleanHistory = this.metroCheckBox3.Checked;
                cleaningOption.CleanInternetCookies = this.metroCheckBox4.Checked;
                cleaningOption.CleanInternetCache = this.metroCheckBox6.Checked;
                cleaningOption.CleanTempFiles = this.metroCheckBox5.Checked;
                cleaningOption.CleanEventLogs = this.metroCheckBox7.Checked;
                cleaningOption.CleanWindowsUpdateFiles = this.metroCheckBox2.Checked;
                ActiveCleaningGui activeCleaningGui = new ActiveCleaningGui(cleaningOption);
                activeCleaningGui.Show();
                this.Dispose();

            }
        }
    }
}
