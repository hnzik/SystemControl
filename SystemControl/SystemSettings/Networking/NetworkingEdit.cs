using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.SystemSettings.Networking
{
    public partial class NetworkingEdit : UserControl
    {
        NetworkHandaler handaler;
        private string adapterDescription;
        private string adapterNameSave;

        public NetworkingEdit()
        {
            handaler = new NetworkHandaler();
            InitializeComponent();
        }

        private void fillNetworkOptions(string adapterName)
        {
            AdapterProperties prop = handaler.getAdapterProperties(handaler.getNetworkAdapterByName(adapterName));
            adapterDescription = prop.adapterDescription;
            adapterNameSave = adapterName;
            metroTextBox1.Text = prop.ipv4Address;
            metroTextBox2.Text = prop.subnetMask;
            metroTextBox3.Text = prop.defaultGateway;
            metroTextBox4.Text = prop.dns1;
            metroTextBox5.Text = prop.dns2;
            metroCheckBox1.Checked = prop.dhcp;
            metroCheckBox2.Checked = prop.dynamicDns;
        }

        private void NetworkingEdit_Load(object sender, EventArgs e)
        {
            this.handaler = new NetworkHandaler();
        }

        public void metroButton3_Click(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton user = (MetroFramework.Controls.MetroButton)sender;
            ComboBox comboBox = (ComboBox)user.Parent.Controls[2];
            fillNetworkOptions(comboBox.SelectedItem.ToString());
        }
        //Verify IP configuration and apply changes
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (handaler.verifyIpAdress(metroTextBox1.Text) && handaler.verifyIpAdress(metroTextBox2.Text) && handaler.verifyIpAdress(metroTextBox3.Text) && handaler.verifyIpAdress(metroTextBox4.Text))
            {
                handaler.setIpForAdapter(adapterDescription, new string[] { metroTextBox1.Text }, metroTextBox2.Text, metroTextBox3.Text);
                Console.WriteLine("Passed");
                fillNetworkOptions(adapterNameSave);
            }
        }

        private void metroCheckBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroCheckBox1_Click_1(object sender, EventArgs e)
        {
            handaler.enableDHCP(adapterNameSave);
        }
    }
}
