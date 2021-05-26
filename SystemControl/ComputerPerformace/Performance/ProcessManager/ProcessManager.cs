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

namespace SystemControl.ComputerPerformace.Performance.ProcessManager
{
    public partial class ProcessManager : Form
    {
        ProcessManagerHandeler handeler;
        public ProcessManager()
        {
            handeler = new ProcessManagerHandeler();
            InitializeComponent();
        }

        private void addProcess()
        {
            ListViewItem item = new ListViewItem(new string[] { "Google Chrome", "50%", "200 MB", "0,5 MB/s", "0.4 Mbps", "2%" });
            this.listView1.Items.Add(item);
        }

        private string converBytesToString(Int64 value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
        private void setupListView() {
            listView1.View = View.Details;
            this.listView1.Columns.Add("Name ", 300, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CPU - ", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Memory - ", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("I/O Total - ", 80, HorizontalAlignment.Left);
        }

        private void updateListView()
        {
            Thread t = new Thread(() =>
            {

                while (true)
                {
                    ListViewItem listViewItems;
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listViewItems = listView1.Items;

                    });
                    foreach (ListViewItem item in listView1.Items)
                        {
                           
                            ProcessItem pItem = this.handeler.getcpuUsage((string)item.Tag);
                            item.SubItems[1].Text = pItem.cpuUsage;
                        }
                    
                   
                    Thread.Sleep(1000);
                }
            });
            t.Start();
        }

        private void fillListView() {
            foreach (ProcessItem p in handeler.getProcessList())
            {
                ListViewItem item = new ListViewItem(new string[] { p.processName, p.cpuUsage, p.ramUsage.ToString(), "0,5 MB/s", "0.4 Mbps", "2%" });
                item.Tag = p.processName;
                this.listView1.Items.Add(item);
            }
            this.updateListView();
        }

        private void ProcessManager_Load(object sender, EventArgs e)
        {
            this.fillListView();
            this.setupListView();
            this.addProcess();
        }
    }
}
