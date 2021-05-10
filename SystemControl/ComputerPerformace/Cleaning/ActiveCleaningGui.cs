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

namespace SystemControl.ComputerPerformace.Cleaning
{
    public partial class ActiveCleaningGui : Form
    {
        Cleaner cleaner = null;
        CleaningOption cleaningOptions = null;
        int totalNumberOfEnabledOptions = 0;
        Thread cleanerThread;
        public ActiveCleaningGui(CleaningOption cleaningOptions)
        {
            InitializeComponent();
            this.cleaner = new Cleaner();
            this.cleaningOptions = cleaningOptions;
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

        private void cleanerFunction()
        {
            this.totalNumberOfEnabledOptions = cleaningOptions.getAllEnabled();
            if (cleaningOptions.EmptyRecycleBin)
            {
                cleaner.emptyRecycleBin();
                logProgressToList("Recycle bin", 0);
                updateProgressBar();
            }
            if (cleaningOptions.CleanWindowsUpdateFiles)
            {
                logProgressToList("Windows update files", cleaner.cleanWindowsUpdateFiles());
                updateProgressBar();
            }
            if (cleaningOptions.CleanTempFiles)
            {
                logProgressToList("Windows temp files", cleaner.cleanTempFiles());
                updateProgressBar();
            }
            if (cleaningOptions.CleanInternetCache)
            {
                logProgressToList("Internet Cache", cleaner.cleanInternetCache());
                updateProgressBar();
            }
            if (cleaningOptions.CleanHistory)
            {
                logProgressToList("Windows search history", cleaner.cleanHistory());
                updateProgressBar();
            }
            if (cleaningOptions.CleanInternetCookies)
            {
                logProgressToList("Internet cookies", cleaner.cleanInternetCookies());
                updateProgressBar();
            }
            if (cleaningOptions.CleanLocalTempFile)
            {
                logProgressToList("Local temp files", cleaner.cleanLocalTempFile());
                updateProgressBar();
            }
            if (cleaningOptions.CleanEventLogs)
            {
                cleaner.cleanEventLogs();
                logProgressToList("Event logs", 0);
                updateProgressBar();
            }
        }


        private void ActiveCleaningGui_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            listView1.Columns.Add("File deleted", 203);
            listView1.Columns.Add("Space cleaned", 203);
            listView1.View = View.Details;
            cleanerThread = new Thread(new ThreadStart(this.cleanerFunction));
            cleanerThread.Start();
        }


        private void updateProgressBar()
        {
            listView1.Invoke((MethodInvoker)delegate ()
            {
                if (this.totalNumberOfEnabledOptions > 0)
                {
                    string eddited = this.metroLabel2.Text.Remove(this.metroLabel2.Text.Length - 1);
                    int addToLabel = Int32.Parse(eddited) + (100 / this.totalNumberOfEnabledOptions);

                    this.progressBar1.Maximum = this.totalNumberOfEnabledOptions;
                    this.progressBar1.Value++;
                    this.metroLabel2.Text = addToLabel.ToString() + "%";
                    if (this.progressBar1.Value == this.totalNumberOfEnabledOptions)
                        this.metroLabel2.Text = "100%";
                }
            });
        }


        private void logProgressToList(string log, long sizeCleared)
        {
            listView1.Invoke((MethodInvoker)delegate ()
            {
                FontFamily family = new FontFamily("Arial");
                Font font = new Font(family, 10.0f);
                string sizeClearedString = converBytesToString(sizeCleared);
                ListViewItem item = new ListViewItem(new[] { log, sizeClearedString });
                item.Font = font;
                item.ForeColor = Color.Gray;
                this.listView1.Items.Add(item);
            });
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            cleanerThread.Abort();
            this.Dispose();
        }
    }
}
