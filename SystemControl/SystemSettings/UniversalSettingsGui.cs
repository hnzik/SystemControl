using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControl.SystemSettings.Windows_settings;
using SystemControl.SystemSettings.Windows_settings.StartUpItems;
using SystemControl.SystemSettings.Windows_settings.PrivacySettings;

namespace SystemControl.SystemSettings
{
    public partial class UniversalSettingsGui : Form
    {
        protected UserControl activeControl;
        protected UserControl pan1 = null;
        protected UserControl pan2 = null;
        protected UserControl pan3 = null;

        protected Panel selectedMenuItem = null;
        public UniversalSettingsGui(int code)
        {
            InitializeComponent();
            //Build all the panels on init
            this.buildGui(code);

        }

        public  virtual void buildGui(int code) {
            switch (code)
            {
                case 1:
                    setupWindowsSettings();
                    break;
                case 2:
                    setupFireWall();
                    break;
                case 3:
                    setupNetworking();
                    break;
                case 4:
                    setupDefender();
                    break;
            }
        }

        private void setupNetworking()
        {
            //Change lables
            this.metroLabel1.Text = "Select Adapter";
            this.metroLabel2.Text = "Adapter information";
            this.metroLabel3.Text = "Adapter settings IPV4";
            this.metroLabel4.Text = "Adapter settings IPV6";

            pan2 = new Networking.NetworkingMain();
            Point location = new Point(202, 0);
            this.Controls.Add(pan2);
            pan2.Location = location;
            activeControl = pan2;
            selectedMenuItem = this.panel2;
            setPanelActive(this.panel2);

            Networking.NetworkingEdit networkingEdit = new Networking.NetworkingEdit();
            pan1 = networkingEdit;
            this.Controls.Add(pan1);
            pan1.Location = location;
            pan1.Visible = false;

            Networking.NetworkingEditIPV6 networkingEditV6 = new Networking.NetworkingEditIPV6();
            pan3 = networkingEditV6;
            this.Controls.Add(pan3);
            pan3.Location = location;
            pan3.Visible = false;

            pan2.Controls[0].Click += networkingEdit.metroButton3_Click;
            pan2.Controls[0].Click += networkingEditV6.metroButton3_Click;

        }

        private void setupFireWall() {
            //Change lables
            this.metroLabel1.Text = "Firewall settings";
            this.metroLabel2.Text = "Inbound rules";
            this.metroLabel3.Text = "Outbound rules";
            this.metroLabel4.Text = "Settings";

            pan2 = new Firewall.FirewallRules(true);
            Point location = new Point(202, 0);
            this.Controls.Add(pan2);
            pan2.Location = location;
            activeControl = pan2;
            selectedMenuItem = this.panel2;
            setPanelActive(this.panel2);

            pan1 = new Firewall.FirewallRules(false);
            location = new Point(202, 0);
            this.Controls.Add(pan1);
            pan1.Location = location;
            pan1.Visible = false;

            pan3 = new Firewall.FirewallSettings();
            location = new Point(202, 0);
            this.Controls.Add(pan3);
            pan3.Location = location;
            pan3.Visible = false;
        }

        private void setupDefender()
        {
            //Change lables
            this.metroLabel1.Text = "Defender settings";
            this.metroLabel2.Text = "General Defender settings";
            this.metroLabel3.Text = "Exclusions";

            pan2 = new DefenderSettings.DefenderPanel();
            Point location = new Point(202, 0);
            this.Controls.Add(pan2);
            pan2.Location = location;
            activeControl = pan2;
            selectedMenuItem = this.panel2;
            setPanelActive(this.panel2);

            DefenderSettings.ExclusionPanel exP = new DefenderSettings.ExclusionPanel();
            pan1 = exP;
            this.Controls.Add(pan1);
            pan1.Location = location;
            pan1.Visible = false;

            this.panel4.Visible = false;
        }

        private void setupWindowsSettings() {
            //Setup AppXPanel
            pan1 = new AppXPanel();
            Point location = new Point(202, 0);
            this.Controls.Add(pan1);
            pan1.Location = location;
            activeControl = pan1;
            selectedMenuItem = this.panel3;
            setPanelActive(this.panel3);

            //Setup StartUpItemsPanel
            pan2 = new StartUpItemsPanel();
            location = new Point(202, 0);
            this.Controls.Add(pan2);
            pan2.Location = location;
            pan2.Visible = false;

            //Setup privacyPanel
            pan3 = new PrivacySettingsPanel();
            location = new Point(202, 0);
            this.Controls.Add(pan3);
            pan3.Location = location;
            pan3.Visible = false;
        }



        private void UniversalSettingsGui_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void setColorToPanel(Panel pan,Color col) {
            Color newBackColor = col;
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

        private void OnPanelMouseEnter(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            if (pan != this.selectedMenuItem) {
                Color newBackColor = Color.FromArgb(235, 235, 235);
                setColorToPanel(pan, newBackColor);

            }
        }

        private void OnPanelMouseLeave(object sender, EventArgs e)
        {
            Panel pan = (Panel)sender;
            if (pan != this.selectedMenuItem)
            {
                Color newBackColor = Color.FromArgb(242, 242, 242);
                setColorToPanel(pan, newBackColor);
            }
        }

        protected private void setPanelActive(Panel pan) {

            Color newBackColor = Color.FromArgb(242, 242, 242);
            setColorToPanel(this.selectedMenuItem, newBackColor);
            this.selectedMenuItem = pan;      
            newBackColor = Color.FromArgb(230, 230, 230);
            setColorToPanel(pan, newBackColor);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            activeControl.Visible = false;
            setPanelActive((Panel)sender);
            pan2.Visible = true;
            activeControl = pan2;
        }

        private void panel3_Click(object sender, EventArgs e)
        {

            activeControl.Visible = false;
            setPanelActive((Panel)sender);
            pan1.Visible = true;
            activeControl = pan1;
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            activeControl.Visible = false;
            setPanelActive((Panel)sender);
            pan3.Visible = true;
            activeControl = pan3;
        }

        private void UniversalSettingsGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void panel5_Click(object sender, EventArgs e)
        {

        }
    }
}
