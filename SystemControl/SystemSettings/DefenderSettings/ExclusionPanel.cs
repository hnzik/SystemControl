using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.SystemSettings.DefenderSettings
{
    public partial class ExclusionPanel : UserControl
    {
        DefenderHandeler handler;
        public ExclusionPanel()
        {
            InitializeComponent();
        }
        private void fillListBox()
        {
            this.listView1.Items.Clear();
            IEnumerable<String> exclusions = handler.getExistingExclusions();        
            foreach (var item in exclusions)
            {
                Console.WriteLine(item);
                ListViewItem listItem = new ListViewItem(item);
                this.listView1.Items.Add(listItem);
            }
        }
        private void ExclusionPanel_Load(object sender, EventArgs e)
        {
            handler = new DefenderHandeler();
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Items", this.listView1.Width);
            fillListBox();

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Aplication files (*.exe)|*.exe";
            openFileDialog1.ShowDialog();
            handler.addExclusion(openFileDialog1.FileName);
            Thread.Sleep(500);
            fillListBox();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
                {
                    handler.deleteExclusion(selectedItem.Text);
                }
            }
            Thread.Sleep(500);
            fillListBox();

        }
    }
}
