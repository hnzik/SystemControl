using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemControl.DiskControl
{
    struct HardDriveInfo
    {
        public string driveName, driveType, volumeLabel, driveFormat, totalFreeSpace, totalSize,serialNumber,model,InterfaceType;
    }
    class DiskHandeler
    {
        private List<HardDriveInfo> driveInfo;
        private List<String> formatBuffer;
        private Process defragProcess;
        private bool hasDefragStarted;


        public void getWin32DiskDriveData(ref HardDriveInfo info,int cnt) {
            ManagementObjectSearcher moSearcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            int count = 0;
            foreach (ManagementObject wmi_HD in moSearcher.Get())
            {
                if(count == cnt)
                {
                    info.model = wmi_HD["Model"].ToString();
                    info.InterfaceType = wmi_HD["InterfaceType"].ToString();
                    info.serialNumber = wmi_HD["SerialNumber"].ToString();
                    return;
                }
                count++;
            }
        }

        public HardDriveInfo getHardDriveInfoSpecific(string label)
        {
            foreach (HardDriveInfo d in driveInfo)
            {
                if (d.driveName == label)
                    return d;
            }
            return new HardDriveInfo();
        }

        private Process startPowerShell(string args,bool redirect)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "defrag",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = redirect,
                    RedirectStandardInput = redirect,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            this.hasDefragStarted = proc.Start();
            return proc;
        }

        public void stopDefrag()
        {
            if (this.defragProcess != null)
            {
                if(!this.defragProcess.HasExited)
                    this.defragProcess.Kill();
            }
        }

        public bool startDiskDefrag(string disk) {
            Console.WriteLine(disk);
            this.defragProcess = startPowerShell(disk + " /O /V /U", true);
            return this.hasDefragStarted;
        }

        public string getConsoleBuffer(ref bool killThread)
        {
            if (this.hasDefragStarted && this.defragProcess.StandardOutput.EndOfStream && this.defragProcess.HasExited)
                killThread = true;

            if (this.hasDefragStarted)
            {
                return this.defragProcess.StandardOutput.ReadLine();
            }

            return null;
        }

        public IEnumerable<HardDriveInfo> getHardDriveInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            this.driveInfo = new List<HardDriveInfo>();
            int count = 0;

            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable)
                {
                    HardDriveInfo hardDriveInfo = new HardDriveInfo();
                    hardDriveInfo.driveName = d.Name;
                    hardDriveInfo.driveType = d.DriveType == DriveType.Fixed ? "Fixed" : "Removable";
                    hardDriveInfo.volumeLabel = d.VolumeLabel;
                    hardDriveInfo.totalFreeSpace = d.TotalFreeSpace.ToString();
                    hardDriveInfo.totalSize = d.TotalSize.ToString();
                    getWin32DiskDriveData(ref hardDriveInfo, count);
                    count++;
                    driveInfo.Add(hardDriveInfo);
               }
            }
            return this.driveInfo;
        }
    }
}
