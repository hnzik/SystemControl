using Microsoft.Win32;
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

namespace SystemControl.SystemSettings.Windows_settings.StartUpItems
{
    public partial class StartUpItemsPanel : UserControl
    {
        RegistryManger registryManger = null;
        bool init = true;
        public StartUpItemsPanel()
        {
            registryManger = new RegistryManger();
            InitializeComponent();
        }


        private bool isItemEnabled(string item)
        {
            Dictionary<string, object> res = this.registryManger.getKeyValuePairs(SectionEnum.User, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run");

            foreach (var valueKeyPair in res)
            {
                if (valueKeyPair.Key == item) {
                    System.Byte[] enabled = (System.Byte[])valueKeyPair.Value;
                    if (enabled[0] == 2) {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }


        private void changeItemStartupStatus(string valueName,bool on) {
            Byte[] newVal = new Byte[12];
            if (on)
            {
                newVal[0] = 2;
                this.registryManger.setRegKeyValue(SectionEnum.User, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run", valueName, newVal);
            }
            else {
                newVal[0] = 3;
                this.registryManger.setRegKeyValue(SectionEnum.User, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run", valueName, newVal);
            }
        }

        private void loadStartUpItems() {
            Dictionary<string, object> res = this.registryManger.getKeyValuePairs(SectionEnum.User, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");   
            foreach (var valueKeyPair in res) {
                this.listView1.Items.Add(new ListViewItem(new[] { "", valueKeyPair.Key, valueKeyPair.Value.ToString() })).Checked = isItemEnabled(valueKeyPair.Key) ? true:false ;
            }
        }


        private void StartUpItemsPanel_Load(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Enabled", 50);
            this.listView1.Columns.Add("Name", 200);
            this.listView1.Columns.Add("File", 200);
            this.loadStartUpItems();
            this.listView1.CheckBoxes = true;
            init = false;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!init) {
                if (e.Item.Checked)
                {
                    changeItemStartupStatus(e.Item.SubItems[1].Text, true);
                }
                else
                {
                    changeItemStartupStatus(e.Item.SubItems[1].Text, false);
                }
            }
        }
    }
}
