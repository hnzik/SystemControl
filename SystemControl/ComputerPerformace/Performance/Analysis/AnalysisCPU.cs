using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.ComputerPerformace.Performance.Analysis
{
    public partial class AnalysisCPU : UserControl
    {
        public AnalysisCPU()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void AnalysisCPU_Load(object sender, EventArgs e)
        {
            this.chart1.Series[0]["PieLabelStyle"] = "Disabled";
        }

        private void metroLabel7_Click(object sender, EventArgs e)
        {

        }
    }
}
