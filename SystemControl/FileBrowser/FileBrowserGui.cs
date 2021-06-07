using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SystemControl.Utils;
namespace SystemControl.FileBrowser
{
    public partial class FileBrowserGui : Form
    {

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static bool ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }

        private Size oldSize;
        private string currentPath;
        private List<String> oldPaths = new List<String>();
        private List<String> forwardPaths = new List<String>();

        private FileSystemWatcher fileSystemWatcher;
        public FileBrowserGui()
        {
            InitializeComponent();
        }


        private void setupButton(Button button)
        {
            if (button.Image != null)
            {
                var pic = new Bitmap(button.Image, new Size(15, 15));
                button.Image = pic;
            }
            Color newBackColor = Color.FromArgb(255, 255, 255);
            button.BackColor = newBackColor;
            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }

        private List<object> recurseFindAllControls(Control baseControl, Type type)
        {
            List<object> reurnList = new List<object>();
            if (baseControl.HasChildren)
            {
                foreach (Control childControl in baseControl.Controls)
                {

                    if (childControl.HasChildren)
                    {
                        reurnList = reurnList.Concat(this.recurseFindAllControls(childControl, type)).ToList();
                    }
                    if (childControl.GetType() == type)
                    {
                        reurnList.Add(childControl);
                    }
                }
            }
            return reurnList;
        }



        private void setupListView()
        {
            listView1.View = View.Details;
            this.listView1.Columns.Add("Name", 203, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Date modifed", 203, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Type", 203, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Size", 203, HorizontalAlignment.Left);
        }

        private void fillListView()
        {
            listView1.Invoke((MethodInvoker)delegate ()
            {
                this.currentPath = new DirectoryInfo(this.currentPath).FullName;
                this.bottomAlignTextBox1.Text = this.currentPath;
                this.listView1.Items.Clear();
                this.listView1.BeginUpdate();
                this.Text = Path.GetFileName(this.currentPath) == "" ? this.currentPath : Path.GetFileName(this.currentPath);
                DirectoryWalker dirWalker = new DirectoryWalker(this.currentPath);
                var imageList1 = new ImageList();
                imageList1.ColorDepth = ColorDepth.Depth32Bit;
                imageList1.ImageSize = new Size(20, 20);
                listView1.SmallImageList = imageList1;
                string project = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                imageList1.Images.Add(Image.FromFile(project + "\\Resources\\folder.png"));
                ListViewItem[] dirlistItemArray = dirWalker.findAllSubDirecotries();

                if (dirlistItemArray != null)
                {
                    this.listView1.Items.AddRange(dirlistItemArray);

                }

                IEnumerable<ListViewItem> listItemArray = dirWalker.findAllFilesInDirectory(ref imageList1);

                if (listItemArray != null)
                {
                    foreach (var item in listItemArray) {
                        if (item != null) {
                            try {
                                this.listView1.Items.Add(item);
                            }
                            catch { }
                        } 
                    }
                }
                this.listView1.EndUpdate();

            });
            setupFileSystemWatcher();
        }

        private void setupFileSystemWatcher()
        {
            if (this.fileSystemWatcher != null)
            {

                this.fileSystemWatcher.Dispose();
            }
            this.fileSystemWatcher = new FileSystemWatcher();
            this.fileSystemWatcher.Path = this.currentPath;
            this.fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            this.fileSystemWatcher.Created += OnChanged;
            this.fileSystemWatcher.Deleted += OnChanged;
            this.fileSystemWatcher.Renamed += OnChanged;
            
            
            
            
            this.fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileBrowserGui_Load(object sender, EventArgs e)
        {
            List<object> buttons = this.recurseFindAllControls(this, typeof(Button));
            foreach (Button button in buttons)
            {
                this.setupButton(button);
            }
            oldSize = this.Size;
            this.setupListView();
            this.currentPath = "C:/";
            this.bottomAlignTextBox1.keyDown += new KeyEventHandler(bottomAlignTextBox1_PreviewKeyDown);
            fillListView();
           
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
        }



        private void FileBrowserGui_Resize(object sender, EventArgs e)
        {

            Size difference = this.Size - oldSize;
            this.panel1.Width += difference.Width;
            this.listView1.Width += difference.Width;
            this.listView1.Height += difference.Height;
            oldSize = this.Size;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.FromArgb(217, 217, 217), ButtonBorderStyle.Solid);

        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            Button senderButton = (Button)sender;
            ControlPaint.DrawBorder(e.Graphics, senderButton.ClientRectangle,
             Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid,
             Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid,
             Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid,
             Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid);

        }


        private void bottomAlignTextBox1_Paint(object sender, PaintEventArgs e)
        {
            Panel senderPan = (Panel)sender;
            ControlPaint.DrawBorder(e.Graphics, senderPan.ClientRectangle,
            Color.Black, 0, ButtonBorderStyle.Solid,
            Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid,
            Color.FromArgb(217, 217, 217), 0, ButtonBorderStyle.Solid,
             Color.FromArgb(217, 217, 217), 1, ButtonBorderStyle.Solid);
        }

        private void bottomAlignTextBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Directory.Exists(this.bottomAlignTextBox1.Text.ToString())) {
                    this.oldPaths.Add(this.currentPath);
                    this.currentPath = this.bottomAlignTextBox1.Text.ToString();
                    this.fillListView();
                    this.bottomAlignTextBox1.Text = this.currentPath;
                }
                else if (File.Exists(this.bottomAlignTextBox1.Text.ToString())) {
                    Process.Start(this.bottomAlignTextBox1.Text.ToString());
                }
                else
                {
                    this.bottomAlignTextBox1.Text = Path.GetFullPath(this.currentPath);
                }
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Delete) {
                if (this.listView1.SelectedItems.Count > 0) {
                    FileManger fileManger = new FileManger();
                    fileManger.fileDelete(this.currentPath + "/" + this.listView1.SelectedItems[0].Text);
                    this.fillListView();
                }
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                string abosultePath = this.currentPath + "/" + this.listView1.SelectedItems[0].Text;
                FileAttributes fileAttributes = File.GetAttributes(abosultePath);
                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    this.oldPaths.Add(this.currentPath);
                    this.currentPath = abosultePath;
                    this.fillListView();
                }
                else
                {
                    try
                    {
                        Process.Start(abosultePath);
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.oldPaths.Count > 0)
            {
                this.forwardPaths.Add(this.currentPath);
                this.currentPath = this.oldPaths[this.oldPaths.Count - 1];
                this.oldPaths.RemoveAll(item => item == this.oldPaths[this.oldPaths.Count - 1]);
                this.fillListView();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.GetParent(this.currentPath) != null)
            {
                this.oldPaths.Add(this.currentPath);
                this.currentPath = System.IO.Directory.GetParent(this.currentPath).FullName;
                this.fillListView();

            }
        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (this.IsHandleCreated) {
                this.fillListView();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.fillListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.forwardPaths.Count > 0) {
                this.oldPaths.Add(this.currentPath);
                this.currentPath = this.forwardPaths[this.forwardPaths.Count - 1];
                this.forwardPaths.RemoveAt(this.forwardPaths.Count - 1);
                this.fillListView();
            }
        }

        private void fileMenuItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            FileManger fileManger = new FileManger();
            switch (e.ClickedItem.Text) {
                case "Copy":
                    if (this.listView1.SelectedItems.Count > 0) {
                        System.Windows.Forms.Clipboard.SetText(this.currentPath + "/" + this.listView1.SelectedItems[0].Text);
                    }
                    break;
                case "Paste":
                    string copyLocation = this.currentPath;
                    if (this.listView1.SelectedItems.Count > 0) {
                        string TempcopyLocation = copyLocation + "/" + this.listView1.SelectedItems[0].Text;
                        if (Directory.Exists(TempcopyLocation)) {
                            copyLocation = TempcopyLocation;
                        }
                    }
                    string whatToCopy = System.Windows.Forms.Clipboard.GetText();
                    Thread t = new Thread(() =>
                    {
                        fileManger.fileCopy(whatToCopy, copyLocation);  
                    });
                    t.Start();
                    break;
                case "Delete":
                    if (this.listView1.SelectedItems.Count > 0)
                    {
                        foreach (ListViewItem file in this.listView1.SelectedItems) {
                            fileManger.fileDelete(this.currentPath + "/" + file.Text);
                        }
                    }
                    break;
                case "Properties":
                    if (this.listView1.SelectedItems.Count > 0)
                    {
                        ShowFileProperties(this.currentPath + "/" + this.listView1.SelectedItems[0].Text);
                    }
                    break;
                case "Rename":
                    if (this.listView1.SelectedItems.Count > 0)
                    {
                        this.listView1.LabelEdit = true;
                        this.listView1.SelectedItems[0].BeginEdit();
                    }
                    break;
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null) {
                FileManger fileManger = new FileManger();
                string newText = this.currentPath + "/" + e.Label;
                e.CancelEdit = true;
                string path = this.currentPath + "/" + this.listView1.Items[e.Item].Text;
                fileManger.fileRename(path, newText);
            }

        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileManger fileManger = new FileManger();
            ToolStripItem clickedItem = (ToolStripItem)sender;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter name for new " + clickedItem.Text, "New " + clickedItem.Text);
            if (input != null) {
                if (clickedItem.Text == "Folder")
                {
                    fileManger.fileCreate(this.currentPath + "/" + input, true);
                }
                else {
                    fileManger.fileCreate(this.currentPath + "/" + input,false);
                }
               
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip2.Items.Clear();
            DriveInfo[] drvInfo = System.IO.DriveInfo.GetDrives();
            foreach (var drv in drvInfo) {
                this.contextMenuStrip2.Items.Add(drv.Name);
            }
            this.contextMenuStrip2.Show(this.button5, new Point(0, this.button5.Height));
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.currentPath = e.ClickedItem.Text;
            this.fillListView();
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Move;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            var copyPath = this.currentPath;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var point = this.listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem item = this.listView1.GetItemAt(point.X, point.Y);
            if (item != null) {
                if (Directory.Exists(this.currentPath + "//" + item.Text)) {
                    copyPath = this.currentPath + "//" + item.Text;
                }
                else
                {
                    this.listView1.SelectedItems.Clear();
                    return;
                }
            }
            foreach (string file in files)
            {

                if (Path.GetFullPath(copyPath) != file) {
                    FileManger fileManger = new FileManger();
                    fileManger.fileRename(file, copyPath + "//" + Path.GetFileName(file));
                }
            }
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var dataToCopy = new string[this.listView1.SelectedItems.Count];
            for (int i = 0; i <dataToCopy.Length; i++) {
                dataToCopy[i] = Path.GetFullPath(this.currentPath) +@"\" +this.listView1.SelectedItems[i].Text;
            }
            DataObject data = new DataObject(DataFormats.FileDrop, dataToCopy);
            data.SetData(dataToCopy);
            listView1.DoDragDrop(data, DragDropEffects.Move);
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
            {
                if (!files.Contains(Path.GetFullPath(this.currentPath) + @"\" + selectedItem.Text)) {
                    selectedItem.Selected = false;
                }
            }
            var point = this.listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem item = this.listView1.GetItemAt(point.X, point.Y);
            if (item != null) {
                item.Selected = true;
            }           
        }
    }
}
