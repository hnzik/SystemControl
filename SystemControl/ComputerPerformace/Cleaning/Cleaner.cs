using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SystemControl.Utils;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SystemControl.ComputerPerformace.Cleaning
{
    class Cleaner
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath,
        RecycleFlags dwFlags);

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000001,
            SHERB_NOSOUND = 0x00000004
        }

        private long cleanDirectoryAndReturnAmoutCleared(string path)
        {
            if (Directory.Exists(path))
            {
                long cleanedSize = fileManger.GetDirectorySize(path);
                fileManger.fileDelete(path,true);
                cleanedSize -= fileManger.GetDirectorySize(path);
                return cleanedSize;

            }
            return 0;
        }

        private FileManger fileManger;
        public Cleaner()
        {
            fileManger = new FileManger();
        }

        public void cleanEventLogs() {
            foreach (var eventLog in EventLog.GetEventLogs())
            {
                eventLog.Clear();
                eventLog.Dispose();
            }
        }

        public void emptyRecycleBin()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION);
        }

        public long cleanHistory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.History);
            return this.cleanDirectoryAndReturnAmoutCleared(path);
        }

        public long cleanInternetCookies()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            return this.cleanDirectoryAndReturnAmoutCleared(path);
        }

        public long cleanInternetCache()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            return this.cleanDirectoryAndReturnAmoutCleared(path);
        }

        public long cleanWindowsUpdateFiles()
        {
            string windouwsUpdateDirecotryPath = "C://Windows//SoftwareDistribution//Download";
            return this.cleanDirectoryAndReturnAmoutCleared(windouwsUpdateDirecotryPath);

        }

        public long cleanTempFiles()
        {
            string tempDirectoryPath = "C://Windows//Temp";
            return this.cleanDirectoryAndReturnAmoutCleared(tempDirectoryPath);
        }

        public long cleanLocalTempFile() {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += "//Temp";
            return this.cleanDirectoryAndReturnAmoutCleared(path);
        }
    }
}
