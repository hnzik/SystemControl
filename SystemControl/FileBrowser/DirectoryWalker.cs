using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemControl.FileBrowser
{
    class DirectoryWalker
    {
        private DirectoryInfo fileList;
        private string path;


        public DirectoryWalker(string path)
        {
            this.path = path;
        }

        private string GetFileTypeDisplayName(string extension)
        {
            if (extension != null && extension.Length > 1)
            {
                if (Registry.ClassesRoot.OpenSubKey(extension) != null)
                {
                    Object exetnstion = Registry.ClassesRoot.OpenSubKey(extension).GetValue(null);
                    if (exetnstion is string)
                    {
                        Object fileName = Registry.ClassesRoot.OpenSubKey((string)exetnstion).GetValue(null);
                        if (fileName != null)
                        {
                            return fileName.ToString();
                        }
                    }
                }
                return (extension.ToUpper().Remove(0, 1)) + " File";

            }
            return null;
        }

        public ListViewItem[] findAllSubDirecotries()
        {
            List<ListViewItem> listViewItemList = new List<ListViewItem>();

            try
            {
                this.fileList = new DirectoryInfo(path);
                DirectoryInfo[] directoryInfo = fileList.GetDirectories();
                foreach (var dir in directoryInfo)
                {
                    if ((dir.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        var item1 = new ListViewItem(new[] { dir.Name, dir.LastWriteTime.ToString(), "Folder" });
                        item1.ImageIndex = 0;
                        listViewItemList.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
            return listViewItemList.ToArray();

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

        public IEnumerable<ListViewItem> findAllFilesInDirectory(ref ImageList imgList)
        {
            List<ListViewItem> listViewItemList = new List<ListViewItem>();
            try
            {
                this.fileList = new DirectoryInfo(path);
                FileInfo[] tempFileInfo = fileList.GetFiles();
                var imageList1 = imgList;
                var partitioner = Partitioner.Create(tempFileInfo);

                Parallel.ForEach(tempFileInfo, (file) =>
                {
                    if ((file.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        var item1 = new ListViewItem(new[] { file.Name, file.LastWriteTime.ToString(), GetFileTypeDisplayName(file.Extension), converBytesToString(file.Length) });
                        if (item1.Text != null) {
                            Icon ico = Icon.ExtractAssociatedIcon(file.FullName);
                            imageList1.Images.Add(file.Name, ico);
                            item1.ImageKey = file.Name;
                            listViewItemList.Add(item1);
                        }
                    }
                });
                return listViewItemList.OrderBy(o => o.Text).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
