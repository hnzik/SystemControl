using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Media;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SystemControl.ComputerPerformace.Performance.ProcessManager
{
    public partial class ProcessManager : Form
    {
        private IComparer comparer = null;
        Thread updateThread;
        ProcessManagerHandeler handeler;
        List<ProcessItem> processItems;

        public ProcessManager()
        {
            InitializeComponent();
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
            this.listView1.Columns.Add("PID", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CPU - ", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Memory - ", 100, HorizontalAlignment.Left);
        }


        private void addNewProcess() {
            bool found = false;
            foreach (ProcessItem pItem in this.processItems)
            {
                foreach (ListViewItem item in this.listView1.Items) {
                    if(item.Tag != null)
                    {
                        ProcessItem process = (ProcessItem)item.Tag;
                        if (process.PID == pItem.PID && process.processName == pItem.processName)
                        {
                            found = true;
                            break;
                        }
                    }   
                }
                if (!found)
                {
                    ListViewItem item = new ListViewItem(new string[] { pItem.processName, pItem.PID, pItem.cpuUsage == null ? "0%" : pItem.cpuUsage, converBytesToString(pItem.ramUsage), "0,5 MB/s"});
                    this.listView1.Items.Add(item);
                    item.Tag = pItem;
                }
                found = false;
            }
        }

        private void discardExitedProcess()
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                ProcessItem process   = (ProcessItem)item.Tag;
                if (processItems.FindIndex(p => p.PID == process.PID) == -1)
                    this.listView1.Items.Remove(item);
            }
        }

        private void updateListView()
        {
            updateThread = new Thread(() =>
            {
                while (true)
                {
                    processItems = handeler.getProcessList();
          
                    for (int i= 0; i<this.processItems.Count; i++)
                    {
                        ProcessItem tempProcessItem = this.processItems[i];
                        tempProcessItem.cpuUsage = this.handeler.cpuProcessUsageTime(this.processItems[i].PID, ref tempProcessItem);
                        processItems[i] = tempProcessItem;
                    }

                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        this.discardExitedProcess();
                        this.addNewProcess();
                        this.listView1.Columns[2].Text = "CPU "+this.handeler.getCurrentCpuUsage();
                        this.listView1.Columns[3].Text = "RAM " + this.handeler.getTotalRamUsage() + "%";

                        foreach (ListViewItem item in this.listView1.Items)
                        {
                            foreach (ProcessItem pItem in this.processItems)
                            {
                                if(item.Tag != null)
                                {
                                    ProcessItem process = (ProcessItem)item.Tag;

                                    if (process.PID == pItem.PID)
                                    {
                                        
                                        item.BackColor = Color.White;
                                        item.SubItems[3].Text = converBytesToString(pItem.ramUsage);
                                        if (pItem.cpuUsage != null)
                                        {
                                            if (Int32.Parse(pItem.cpuUsage) != 0)
                                            {
                                                item.BackColor = Color.FromArgb(1, 255, 255 - Int32.Parse(pItem.cpuUsage) * 2, 255 - (Int32.Parse(pItem.cpuUsage) * 2));
                                            }
                                        }
                                        if(pItem.cpuUsage == null)
                                            item.SubItems[2].Text = "0"+ "%";
                                       else
                                            item.SubItems[2].Text = pItem.cpuUsage + "%";
                                    }
                                }
                            }
                        }
                        if (comparer != null)
                        {
                            this.listView1.ListViewItemSorter = null;

                            this.listView1.ListViewItemSorter = comparer;
                        }
                    });                   
                    Thread.Sleep(100);
                }
            });
            updateThread.Start();
        }

        private void fillListView() {
            processItems = handeler.getProcessList();
            foreach (ProcessItem p in processItems)
            {
                ListViewItem item = new ListViewItem(new string[] { p.processName, p.PID ,p.cpuUsage, converBytesToString(p.ramUsage), "0,5 MB/s", "0.4 Mbps", "2%" });
                item.Tag = p;
                this.listView1.Items.Add(item);
            }
            this.updateListView();
        }

     

        private void ProcessManager_Load(object sender, EventArgs e)
        {
            handeler = new ProcessManagerHandeler();
            this.listView1.Sorting = SortOrder.None;
            this.fillListView();
            this.setupListView();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ProcessManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateThread.Abort();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            comparer = new ListViewItemComparer(e.Column);
            this.listView1.ListViewItemSorter = comparer;
        }
    }
    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            switch (col)
            {
                case 0:
                    return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                case 1:
                    if (Int32.Parse(((ListViewItem)x).SubItems[col].Text) > Int32.Parse(((ListViewItem)y).SubItems[col].Text))
                        return 1;
                    return 0;
                case 2:
                    if (((ListViewItem)x).SubItems[col].Text.Length <= 1 || ((ListViewItem)x).SubItems[col].Text.IndexOf("%") <= 0)
                        return 1;
                    if (((ListViewItem)y).SubItems[col].Text.Length <= 1 || ((ListViewItem)y).SubItems[col].Text.IndexOf("%") <= 0)
                        return 0;
                    if (Int32.Parse(((ListViewItem)x).SubItems[col].Text.Substring(0, ((ListViewItem)x).SubItems[col].Text.IndexOf("%"))) < Int32.Parse(((ListViewItem)y).SubItems[col].Text.Substring(0, ((ListViewItem)y).SubItems[col].Text.IndexOf("%"))))
                        return 1;
                    return 0;
                case 3:
                    ListViewItem itemY = (ListViewItem)y;
                    ListViewItem itemX = (ListViewItem)x;
                    if (itemY.Tag == null)
                        return 0;
                    if (itemX.Tag == null)
                        return 1;
                    ProcessItem processY = (ProcessItem)itemY.Tag;
                    ProcessItem processX = (ProcessItem)itemX.Tag;
                    if (processX.ramUsage < processY.ramUsage)
                        return 1;
                    return 0;


            }
            if (Int32.Parse(((ListViewItem)x).SubItems[col].Text) > Int32.Parse(((ListViewItem)y).SubItems[col].Text))
                return 1;
            return 0;

        }
    }

}
