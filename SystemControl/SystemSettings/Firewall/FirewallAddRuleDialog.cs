using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.SystemSettings.Firewall
{
    public partial class FirewallAddRuleDialog : Form
    {
        private int direction;
        public FirewallAddRuleDialog(int direction)
        {
            InitializeComponent();
            this.direction = direction;
        }



        private void FirewallAddRuleDialog_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //Required for prompt text to work
            this.metroTextBox1.AppendText("a");
            this.metroTextBox1.ResetText();
            this.metroTextBox2.AppendText("a");
            this.metroTextBox2.ResetText();
            this.metroTextBox3.AppendText("a");
            this.metroTextBox3.ResetText();

            this.comboBox1.Items.Add("TCP/UDP");
            this.comboBox1.Items.Add("TCP");
            this.comboBox1.Items.Add("UDP");
            this.comboBox1.SelectedIndex = 0;

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            FirewallManager firewallManager = new FirewallManager();

            int portNum = 0;
            Int32.TryParse(metroTextBox4.Text, out portNum);   
            
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    firewallManager.addFireWallRule(this.metroTextBox1.Text, this.metroTextBox2.Text, this.metroTextBox3.Text, portNum, 6,this.direction);
                    firewallManager.addFireWallRule(this.metroTextBox1.Text, this.metroTextBox2.Text, this.metroTextBox3.Text, portNum, 17, this.direction);
                    break;
                case 1:
                    firewallManager.addFireWallRule(this.metroTextBox1.Text, this.metroTextBox2.Text, this.metroTextBox3.Text, portNum, 17, this.direction);
                    break;
                case 2:
                    firewallManager.addFireWallRule(this.metroTextBox1.Text, this.metroTextBox2.Text, this.metroTextBox3.Text, portNum, 6, this.direction);
                    break;
            }
            this.Close();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Aplication files (*.exe)|*.exe";
            openFileDialog1.ShowDialog();
            this.metroTextBox3.Text = openFileDialog1.FileName;
        }
    }
}
