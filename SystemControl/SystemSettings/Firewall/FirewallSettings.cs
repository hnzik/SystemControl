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
    public partial class FirewallSettings : UserControl
    {
        public FirewallSettings()
        {
            InitializeComponent();
        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private string getActiveDomainName(int num) {
            switch (num)
            {
                case 1:
                    return "Domain network";
                case 2:
                    return "Private network";
                case 4:
                    return "Public network";
            }
            return "null";
        }

        private void FirewallSettings_Load(object sender, EventArgs e)
        {
            FirewallManager firewallMan = new FirewallManager();
            this.metroLabel2.Text = getActiveDomainName(firewallMan.getActiveProfile());
            this.metroToggle1.Checked = firewallMan.isFireWallEnabled(2);
            this.metroToggle2.Checked = firewallMan.isFireWallEnabled(4);
            this.metroToggle3.Checked = firewallMan.isFireWallEnabled(1);
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            FirewallManager firewallMan = new FirewallManager();
            MetroFramework.Controls.MetroToggle toggle = (MetroFramework.Controls.MetroToggle)sender;
            firewallMan.changeFireWallSettingsFireWall(2, toggle.Checked);
        }

        private void metroToggle2_CheckedChanged(object sender, EventArgs e)
        {
            FirewallManager firewallMan = new FirewallManager();
            MetroFramework.Controls.MetroToggle toggle = (MetroFramework.Controls.MetroToggle)sender;
            firewallMan.changeFireWallSettingsFireWall(4, toggle.Checked);
        }

        private void metroToggle3_CheckedChanged(object sender, EventArgs e)
        {
            FirewallManager firewallMan = new FirewallManager();
            MetroFramework.Controls.MetroToggle toggle = (MetroFramework.Controls.MetroToggle)sender;
            firewallMan.changeFireWallSettingsFireWall(1, toggle.Checked);
        }
    }
}
