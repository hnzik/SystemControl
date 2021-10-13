using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace SystemControl.Utils
{
    class FileManger
    {
        private IntPtr handle;


        public long GetDirectorySize(string p)
        {
            if (Directory.Exists(p))
            {
                string[] a = Directory.GetFiles(p, "*.*");
                long b = 0;
                foreach (string name in a)
                {
                    FileInfo info = new FileInfo(name);
                    b += info.Length;
                }
                return b;
            }
            return 0;
        }

        private void DeleteDirectory(string target_dir, bool shouldWeDeleteDirecotryFolder = true)
        {
            string[] files = null;
            string[] dirs = null;
            try
            {
                files = Directory.GetFiles(target_dir);
                dirs = Directory.GetDirectories(target_dir);
            }
            catch
            {


            }
            if (files != null)
            {
                foreach (string file in files)
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }

            if (dirs != null)
            {

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir, shouldWeDeleteDirecotryFolder);
                }
            }
            if (shouldWeDeleteDirecotryFolder)
            {
                try
                {
                    Directory.Delete(target_dir, false);

                }
                catch
                {

                }
            }

        }

        public void copyEntireDirectory(string path, string newPath, bool silent)
        {
            SystemControl.ComputerPerformace.Cleaning.ActiveCopy activeCopy = null;
            int counter = 0;
            System.Timers.Timer aTimer;
            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += (source, e) => {

                if (activeCopy != null)
                {
                    activeCopy.Invoke((MethodInvoker)delegate ()
                    {
                        for (int i = 0; i < counter; i++) {
                            activeCopy.udpateProgressBar(counter);
                        }
                        counter = 0; 

                    });
                }
            };
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            Thread t = new Thread(() =>
            {
                activeCopy = new SystemControl.ComputerPerformace.Cleaning.ActiveCopy(Path.GetFileName(path) + "   to  "+ Path.GetFileName(Path.GetDirectoryName(newPath)));
                activeCopy.setMaxVal(Directory.GetFiles(path, "*", SearchOption.AllDirectories).Count());
                activeCopy.ShowDialog();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            aTimer.Start();
            Parallel.ForEach(Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories)
                , (fileName) =>
                {
                    string output = Regex.Replace(fileName, "^" + Regex.Escape(path), newPath);
                    if (File.Exists(fileName))
                    {
                        counter++;
                        Directory.CreateDirectory(Path.GetDirectoryName(output));
                        File.Copy(fileName, output, true);
                    }
                    else
                        Directory.CreateDirectory(output);

                });
            aTimer.Stop(); 
            activeCopy.Invoke((MethodInvoker)delegate () {
                activeCopy.Dispose();
            });
        }

        private string renameIfFileExists(string name)
        {
            int currentIter = 1;
            int editIndex = name.Length;
            if (File.Exists(name))
            {
                editIndex = name.LastIndexOf(".");
            }

            while (File.Exists(name) || Directory.Exists(name))
            {
                name = name.Insert(editIndex, currentIter.ToString());
                currentIter++;
            }
            return name;
        }

        public void fileRename(string what, string newName)
        {
            newName = renameIfFileExists(newName);
            if (File.Exists(what)) {
                File.Move(what, newName);
            }
            else if (Directory.Exists(what))
            {
                Directory.Move(what, newName);
            }
        }

        public void fileDelete(string what, bool shouldWeDeleteDirecotryFolder = true)
        {
            if (File.Exists(what))
            {
                try
                {
                    File.Delete(what);

                }
                catch (Exception ex)
                {

                }
            }
            else if (Directory.Exists(what))
            {
                this.DeleteDirectory(what, shouldWeDeleteDirecotryFolder);
            }
        }


        public void fileCreate(string what, bool isDirectory)
        {
            what = renameIfFileExists(what);
            if (!File.Exists(what) && !Directory.Exists(what))
            {
                if (isDirectory)
                {
                    Directory.CreateDirectory(what);
                }
                else
                {
                     var myFile =  File.Create(what);
                     myFile.Close();
                }
            }
        }

        public void fileCopy(string what, string where)
        {
            if (Directory.Exists(what) || File.Exists(what))
            {
                where += "/" + Path.GetFileName(what);
                where = renameIfFileExists(where);
                Console.WriteLine(where);
                FileAttributes fileAttributes = File.GetAttributes(what);
                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    this.copyEntireDirectory(what, where, false);
                }
                else
                {
                    File.Copy(what, where);
                }
            }
        }
    }
}
