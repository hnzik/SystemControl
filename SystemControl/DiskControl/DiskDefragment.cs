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

namespace SystemControl.DiskControl
{
    public partial class DiskDefragment : UserControl
    {
        private DiskHandeler diskHandeler;
        private string diskName;
        private Thread t;
        public DiskDefragment()
        {
            diskHandeler = new DiskHandeler();
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            bool killThread = false;

            t = new Thread(() =>
            {
                if(diskHandeler.startDiskDefrag(diskName))
                while (!killThread)
                {

                    string nextLine = diskHandeler.getConsoleBuffer(ref killThread);
                    if (nextLine != "") {
                        listView1.Invoke((MethodInvoker)delegate ()
                        {
                            this.listView1.Items.Add(nextLine);
                            this.listView1.Items[listView1.Items.Count - 1].EnsureVisible();

                        });
                    }      
                   Thread.Sleep(50);
                }
                listView1.Invoke((MethodInvoker)delegate ()
                {
                    this.listView1.Items.Add("Succesfully finished defragmentation");
                });

            });
            t.Start();
            this.listView1.Items.Add("Succesfully started defrag.exe");
        }

        private void DiskDefragment_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            this.listView1.Columns.Add("Console output", this.listView1.Size.Width, HorizontalAlignment.Left);
        }

        public void getDisk(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton user = (MetroFramework.Controls.MetroButton)sender;
            ComboBox comboBox = (ComboBox)user.Parent.Controls[2];
            this.diskName = comboBox.SelectedItem.ToString().Substring(0, comboBox.SelectedItem.ToString().IndexOf("-") - 2);
            this.listView1.Items.Clear();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            diskHandeler.stopDefrag();
            t.Abort();
            this.listView1.Items.Add("Succesfully stoped defragmentation process");

        }
    }
}
