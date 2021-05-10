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
    public partial class NetworkingEditIPV6 : UserControl
    {
        NetworkHandaler handaler;
        private string adapterDescription;
        private string adapterNameSave;
        public NetworkingEditIPV6()
        {
            InitializeComponent();
        }

        private void fillNetworkOptions(string adapterName)
        {
            AdapterProperties prop = handaler.getAdapterProperties(handaler.getNetworkAdapterByName(adapterName));
            adapterDescription = prop.adapterDescription;
            adapterNameSave = adapterName;
            metroTextBox1.Text = prop.ipv6Address;
            metroTextBox2.Text = prop.subnetMaskV6;
            metroTextBox3.Text = prop.defaultGateway;
            metroTextBox4.Text = prop.dns1;
            metroTextBox5.Text = prop.dns2;
            metroCheckBox1.Checked = prop.dhcp;
            metroCheckBox2.Checked = prop.dynamicDns;
        }


        private void NetworkingEditIPV6_Load(object sender, EventArgs e)
        {
            handaler = new NetworkHandaler();
        }

        public void metroButton3_Click(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton user = (MetroFramework.Controls.MetroButton)sender;
            ComboBox comboBox = (ComboBox)user.Parent.Controls[2];
            fillNetworkOptions(comboBox.SelectedItem.ToString());
        }

        private void metroTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
