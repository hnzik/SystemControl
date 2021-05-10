using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace SystemControl.SystemSettings.Networking
{
    public partial class NetworkingMain : UserControl
    {
        private NetworkHandaler handaler;
        public NetworkingMain()
        {
            InitializeComponent();
        }

        private void NetworkingMain_Load(object sender, EventArgs e)
        {
            handaler = new NetworkHandaler();
            setupComboxItems();
        }

        private void setupComboxItems() {
            this.comboBox1.Items.AddRange(handaler.getAdapterNames().ToArray());
            if (comboBox1.Items.Count != 0)
                this.comboBox1.SelectedIndex = 0;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            handaler.getAdapterProperties(handaler.getNetworkAdapterByName(this.comboBox1.Text));
        }
    }
}
