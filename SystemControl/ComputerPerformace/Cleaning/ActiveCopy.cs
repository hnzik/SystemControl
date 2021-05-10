using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.ComputerPerformace.Cleaning
{
    public partial class ActiveCopy : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public ActiveCopy(String file)
        {
            InitializeComponent();
            this.metroLabel2.Text = file;
        }

        public void udpateProgressBar(int valueToAdd) {
            int curentVal = this.progressBar1.Value;
            if (this.progressBar1.Maximum <= curentVal++) {
                this.progressBar1.Maximum+=100;
            }
            this.progressBar1.Value++;
            this.Update();
        }

        public void setMaxVal(int maxVal)
        {
            this.progressBar1.Maximum = maxVal;
        }
        private void ActiveCopy_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}
