using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemControl.ComputerPerformace.Performance.ProcessManager
{
    struct ProcessItem
    {
        public string processName, cpuUsage, diskUsage, networkUsage, gpuUsage, PID;
        public long ramUsage;
        public double oldCpu;
        public DateTime lastMessure;

    }
    class ProcessManagerHandeler
    {
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public string getCurrentCpuUsage()
        {
            int usage = (int)cpuCounter.NextValue();
            return "- " + usage.ToString()+"%"; 
        }

        public string getTotalRamUsage()
        {
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            if (memoryValues != null)
            {
                int proc = (int)(((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100);
                return proc.ToString();
            }
            return "0";
        }

         public string cpuProcessUsageTime(string p, ref ProcessItem old)
         {
            int usage = 0;
            try
            {
                Process test = Process.GetProcessById(Int32.Parse(p));
                test.Refresh();
                double cpuUsage = (test.TotalProcessorTime.TotalMilliseconds - old.oldCpu) / DateTime.Now.Subtract(old.lastMessure).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                old.lastMessure = DateTime.Now;
                old.oldCpu = test.TotalProcessorTime.TotalMilliseconds;
                usage = (int)(cpuUsage * 100);
            }
            catch (Exception e)
            {
          
            }
            return usage.ToString();
         }

    


        public List<ProcessItem> getProcessList()
        {
            Process[] processCollection = Process.GetProcesses();
            List<ProcessItem> processItems = new List<ProcessItem>();
            foreach (Process p in processCollection) {
                if (p.Id == 0)
                    continue;
                ProcessItem processItem = new ProcessItem();
                processItem.PID = p.Id.ToString();
                processItem.oldCpu = p.TotalProcessorTime.TotalMilliseconds;
                processItem.lastMessure = DateTime.Now;

                processItem.processName = p.ProcessName;
                processItem.ramUsage = p.PrivateMemorySize64;
                processItems.Add(processItem);
            }
            
            return processItems;
        }
    }
}
