using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemControl.ComputerPerformace.Performance.Analysis;

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
            System.Threading.Thread.Sleep(600);

            return (int)cpuCounter.NextValue() + "%";
        }

        private string cpuProcessUsageTime(Process p)
        {
            CpuUsage cpuUsage1 = new CpuUsage();
            short usage = 0;
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    short internalUsage = cpuUsage1.GetUsage(Process.GetProcessById(p.Id));
                    if (internalUsage != -1)
                    {
                        usage = internalUsage;
                        break;
                    }
                }
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
