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
    }
    struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    class ProcessManagerHandeler
    {
        [DllImport("kernel32.dll")]
        private static extern bool GetProcessIoCounters(IntPtr ProcessHandle, out IO_COUNTERS IoCounters);

        public string getCurrentCpuUsage()
        {
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(200);
            return (int)cpuCounter.NextValue() + "%";
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

        private string cpuProcessUsageTime(Process p)
        {
            int usage = 0;
            try
            {
                var counter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName);
                counter.NextValue();
                Thread.Sleep(10);
                usage = (int)(counter.NextValue()/Environment.ProcessorCount);
            }
            catch (Exception e)
            {
          
            }
            if(usage != 0)
            {
                Console.WriteLine(p.ProcessName);
                Console.WriteLine(usage);


            }

            return usage.ToString();
        }

        public ProcessItem getcpuUsage(string procname)
        {
            Process[] processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                try
                {
                    if (procname == p.ProcessName)
                    {
                        ProcessItem processItem = new ProcessItem();
                        processItem.processName = p.ProcessName;
                        processItem.cpuUsage = this.cpuProcessUsageTime(p);
                        return processItem;
                    }

                } catch (Exception ex)
                {

                }
            }
            return new ProcessItem();
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
                processItem.processName = p.ProcessName;
                processItem.ramUsage = p.PrivateMemorySize64;
                processItems.Add(processItem);
            }
            
            return processItems;
        }
    }
}
