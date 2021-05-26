using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemControl.DiskControl;
using SystemControl.SystemSettings;

namespace SystemControl.ComputerPerformace.Performance.Analysis
{
    class AnalysisMain : UniversalSettingsGui
    {
        public AnalysisMain(int code) : base(code) { }


        public override void buildGui(int code)
        {
            setupWindowsSettings();
        }
        private void setupWindowsSettings()
        {
            this.metroLabel2.Text = "Disk select";
            this.metroLabel3.Text = "Disk info";
            this.metroLabel4.Text = "Disk defragmentation";

            //Setup AppXPanel
            DiskControlInfo diskInfo = new DiskControl.DiskControlInfo();

            pan1 = diskInfo;
            Point location = new Point(202, 0);
            this.Controls.Add(pan1);
            pan1.Location = location;
            pan1.Visible = false;

            //Setup StartUpItemsPanel
            pan2 = new AnalysisCPU();
            location = new Point(202, 0);
            this.Controls.Add(pan2);
            pan2.Location = location;
            activeControl = pan2;
            selectedMenuItem = this.panel2;
            setPanelActive(this.panel2);

            DiskDefragment diskDefragment = new DiskDefragment();
            //Setup privacyPanel
            pan3 = diskDefragment;
            location = new Point(202, 0);
            this.Controls.Add(pan3);
            pan3.Location = location;
            pan3.Visible = false;
        }
    }
}
