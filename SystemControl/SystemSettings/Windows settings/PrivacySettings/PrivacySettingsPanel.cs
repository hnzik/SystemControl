using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemControl.Utils;

namespace SystemControl.SystemSettings.Windows_settings.PrivacySettings
{
    public struct PrivacyRegKey
    {
        public SectionEnum section;
        public string path;
        public string valueName;
    };

    public partial class PrivacySettingsPanel : UserControl
    {
        RegistryManger registryManger = null;
        public PrivacySettingsPanel()
        {
            InitializeComponent();
            registryManger = new RegistryManger();
        }

        private void PrivacySettingsPanel_Load(object sender, EventArgs e)
        {
            this.metroToggle1.Tag = new PrivacyRegKey() {
                section = SectionEnum.User,
                path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Search",
                valueName = "CortanaEnabled"
            };

            this.metroToggle2.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\Windows Search",
                valueName = "DisableWebSearch"
            };

            this.metroToggle3.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\LocationAndSensors",
                valueName = "DisableLocation"
            };

            this.metroToggle4.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.User,
                path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Search",
                valueName = "BingSearchEnabled"
            };

            this.metroToggle5.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\TabletPC",
                valueName = "PreventHandwritingDataSharing"
            };

            this.metroToggle6.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\HandwritingErrorReports",
                valueName = "PreventHandwritingErrorReports"
            };

            this.metroToggle7.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat",
                valueName = "DisableInventory"
            };

            this.metroToggle8.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo",
                valueName = "Enabled"
            };

            this.metroToggle9.Tag = new PrivacyRegKey()
            {
                section = SectionEnum.Machine,
                path = "SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection",
                valueName = "AllowTelemetry"
            };
        }

        private void togleChecked(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroToggle toggle = (MetroFramework.Controls.MetroToggle)sender;
            PrivacyRegKey regKey = (PrivacyRegKey)toggle.Tag;
            if (toggle.Checked) {
                registryManger.setRegKeyValue(regKey.section, regKey.path, regKey.valueName, 1);
            }
            else
            {
                registryManger.setRegKeyValue(regKey.section, regKey.path, regKey.valueName, 0);
            }
        }
    }
}
