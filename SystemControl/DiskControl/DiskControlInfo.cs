using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.DiskControl
{
    public partial class DiskControlInfo : UserControl
    {
        HardDriveInfo hddInfo;
        DiskHandeler handeler;
        public DiskControlInfo()
        {
            handeler = new DiskHandeler();
            InitializeComponent();
        }
        private string converBytesToString(Int64 value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        private void setHardDriveInfo() {
            this.metroTextBox1.Text = hddInfo.driveName;
            this.metroTextBox2.Text = hddInfo.volumeLabel;
            this.metroTextBox3.Text = hddInfo.model;
            this.metroTextBox4.Text = hddInfo.serialNumber;
            this.metroTextBox5.Text = hddInfo.InterfaceType;
            this.metroTextBox6.Text = converBytesToString(Int64.Parse(hddInfo.totalSize));
            this.metroTextBox7.Text = converBytesToString(Int64.Parse(hddInfo.totalFreeSpace));

        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel12_Click(object sender, EventArgs e)
        {

        }

        private void DiskControlInfo_Load(object sender, EventArgs e)
        {

        }
        public void getDisk(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton user = (MetroFramework.Controls.MetroButton)sender;
            ComboBox comboBox = (ComboBox)user.Parent.Controls[2];
            handeler.getHardDriveInfo();
            this.hddInfo = handeler.getHardDriveInfoSpecific(comboBox.SelectedItem.ToString().Substring(0, comboBox.SelectedItem.ToString().IndexOf("-") - 1));
            setHardDriveInfo();
        }

        private void metroTextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
