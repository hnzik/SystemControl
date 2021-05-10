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
    public partial class FirewallRules : UserControl
    {
        private bool init = true;
        private int direction = 1;
        public FirewallRules(bool inbound)
        {
            InitializeComponent();
            if (inbound)
            {
                this.label1.Text = "Inbound rules";
                this.label2.Text = "Inbound firewall rules protect the network against incoming traffic from the internet";
            }
            else {
                this.label1.Text = "Outbound rules";
                this.label2.Text = "Originate from inside the network, destined for services on the internet or outside network";
                this.metroButton2.Text = "Add outbound rule";
                direction = 2;
            }

        }


        private void setupItems() {
            init = true;
            this.listView1.Items.Clear();
            FirewallManager firewallManager = new FirewallManager();
            IEnumerable<FireWallItem> ieFireWall = firewallManager.getAllFireWallRules(this.direction);
            foreach (var item in ieFireWall) {
                ListViewItem listViewItem = new ListViewItem(new[] { item.name, item.programPath, item.localAddress, item.remoteAddress, item.protocol, item.localPort, item.remotePort });
                listViewItem.Checked = item.enabled;
                this.listView1.Items.Add(listViewItem);
            }
        }

        private void FirewallRules_Load(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Name", 150);
            this.listView1.Columns.Add("Program", 150);
            this.listView1.Columns.Add("Local address", 50);
            this.listView1.Columns.Add("Remote address", 50);
            this.listView1.Columns.Add("Protocol", 50);
            this.listView1.Columns.Add("Local port", 50);
            this.listView1.Columns.Add("Remote port", 50);
            setupItems();
            this.listView1.CheckBoxes = true;
            init = false;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            FirewallAddRuleDialog firewallAddRuleDialog = new FirewallAddRuleDialog(1);
            firewallAddRuleDialog.Show();
            firewallAddRuleDialog.FormClosing += (s, args) =>
            {
                this.setupItems();

            };
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            FirewallManager firewallManager = new FirewallManager();

            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                firewallManager.deleteFireWallRule(item.SubItems[0].Text, this.getProtocolNumber(item.SubItems[4].Text),item.SubItems[5].Text, this.direction);
            }
            this.setupItems();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        private int getProtocolNumber(string protocol)
        {
            switch (protocol)
            {
                case "UDP":
                    return 17;
                    
                case "TCP":
                    return  6;
            }
            return 0;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            FirewallManager firewallManager = new FirewallManager();
            if (!this.init)
            {
                firewallManager.manageFireWallRule(e.Item.Checked,e.Item.SubItems[0].Text, this.getProtocolNumber(e.Item.SubItems[4].Text), e.Item.SubItems[5].Text,this.direction);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
