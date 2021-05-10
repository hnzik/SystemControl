using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SystemControl.SystemSettings.Windows_settings
{
    public partial class AppXPanel : UserControl
    {
        private AppX appXManager = null;
        public AppXPanel()
        {
            InitializeComponent();
            this.appXManager = new AppX();
        }

        private void fillListBox() {
            IEnumerable<AppXItem> appXItems = appXManager.GetAppXItems();
            var imageList1 = new ImageList();
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(20, 20);
            listView1.SmallImageList = imageList1;
            foreach (var item in appXItems) {
                Image ico = null;
                ListViewItem listItem = new ListViewItem(item.DisplayName);
                listItem.Tag = item;
                if (item.Logo != null)
                {
                    ico = Bitmap.FromFile(item.Logo);                    
                    imageList1.Images.Add(ico);
                    listItem.ImageIndex = imageList1.Images.Count - 1;
                }
          
                this.listView1.Items.Add(listItem);
            }   
        }

        private void AppXPanel_Load(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Items",this.listView1.Width);
            fillListBox();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0) {
                foreach (ListViewItem selectedItem in this.listView1.SelectedItems) {
                    AppXItem appXItem = (AppXItem)selectedItem.Tag;
                    this.appXManager.RemovePackage(appXItem.FullName);
                    selectedItem.Remove();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
