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
    public partial class DiskControlSelect : UserControl
    {
        DiskHandeler handeler;
        public DiskControlSelect()
        {
            handeler = new DiskHandeler();
            InitializeComponent();   
        }



        private void DiskControlSelect_Load(object sender, EventArgs e)
        {
            foreach (var item in handeler.getHardDriveInfo())
            {
                ListViewItem itemList = new ListViewItem(item.driveName + " - " + item.volumeLabel + " - " + item.driveType);
                this.comboBox1.Items.Add(itemList.Text);

            }

            if (comboBox1.Items.Count != 0)
                this.comboBox1.SelectedIndex = 0;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            metroButton3.PerformClick();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
        }
    }
}
