using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.SystemSettings.DefenderSettings
{
    public partial class DefenderPanel : UserControl
    {
        DefenderHandeler handeler;
        public DefenderPanel()
        {
            handeler = new DefenderHandeler();
            InitializeComponent();
        }

        private void DefenderPanel_Load(object sender, EventArgs e)
        {
            DefenderSettings settings = handeler.getDefenderSettings();
            metroToggle1.Checked = settings.behaviorMonitoring;
            metroToggle1.Tag = 2;
            metroToggle4.Checked = settings.realtimeMonitoring;
            metroToggle4.Tag = 1;
            metroToggle3.Checked = settings.blockAtFirstSeen;
            metroToggle3.Tag = 3;
            metroToggle5.Checked = settings.intrusionPreventionSystem;
            metroToggle5.Tag = 7;
            metroToggle10.Checked = settings.IOAVProtection;
            metroToggle10.Tag = 4;
            metroToggle8.Checked = settings.signatureDisableUpdateOnStartupWithoutEngine;
            metroToggle8.Tag = 5;
            metroToggle7.Checked = settings.archiveScanning;
            metroToggle7.Tag = 6;
            metroToggle6.Checked = settings.scriptScanning;
            metroToggle6.Tag = 8;
            metroToggle11.Checked = settings.MAPSReporting;
            metroToggle11.Tag = 9;
        }

        private void metroLabel11_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void metroToggle1_Click(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroToggle toggle = (MetroFramework.Controls.MetroToggle)sender;
            handeler.changeDefenderSettings((int)toggle.Tag);
        }
    }
}
