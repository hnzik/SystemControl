using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.ComputerPerformace.Cleaning
{
    public class CleaningOption
    {
        private bool cleanLocalTempFile;
        private bool emptyRecycleBin;
        private bool cleanHistory;
        private bool cleanInternetCookies;
        private bool cleanInternetCache;
        private bool cleanWindowsUpdateFiles;
        private bool cleanTempFiles;
        private bool cleanEventLogs;
        public bool EmptyRecycleBin { get => emptyRecycleBin; set => emptyRecycleBin = value; }
        public bool CleanHistory { get => cleanHistory; set => cleanHistory = value; }
        public bool CleanInternetCookies { get => cleanInternetCookies; set => cleanInternetCookies = value; }
        public bool CleanInternetCache { get => cleanInternetCache; set => cleanInternetCache = value; }
        public bool CleanWindowsUpdateFiles { get => cleanWindowsUpdateFiles; set => cleanWindowsUpdateFiles = value; }
        public bool CleanTempFiles { get => cleanTempFiles; set => cleanTempFiles = value; }
        public bool CleanLocalTempFile { get => cleanLocalTempFile; set => cleanLocalTempFile = value; }
        public bool CleanEventLogs { get => cleanEventLogs; set => cleanEventLogs = value; }

        public int getAllEnabled() {
            int count = 0;
            if (cleanLocalTempFile)
                count++;
            if (emptyRecycleBin)
                count++; 
            if (cleanHistory)
                count++;
            if (cleanInternetCookies)
                count++;
            if (cleanInternetCache)
                count++;
            if (cleanWindowsUpdateFiles)
                count++;
            if (cleanTempFiles)
                count++;
            if (cleanEventLogs)
                count++;
            return count;
        }    
    }
}
