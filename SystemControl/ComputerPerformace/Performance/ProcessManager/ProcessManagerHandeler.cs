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
        public string processName, cpuUsage, diskUsage, networkUsage, gpuUsage;
        public int ramUsage;
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

        private string cpuProcessUsageTime(Process p)
        {
            CpuUsage cpuUsage1 = new CpuUsage();
            short usage = 0;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    short internalUsage = cpuUsage1.GetUsage(Process.GetProcessById(p.Id));
                    if (internalUsage != -1)
                    {
                        usage = internalUsage;
                        break;
                    }

                }
            } catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(p.ProcessName);
            }
            if(usage != 0)
            {
                Console.WriteLine(p.ProcessName);
                Console.WriteLine(usage);


            }

            return usage.ToString();
        }

        private int getRamUsage(Process p) {
            int usage = 0;
            try
            {
                usage = Convert.ToInt32(new PerformanceCounter("Process", "Working Set - Private", p.ProcessName, true).NextValue());
            } catch(Exception e)
            {

            }
            return usage;
        }



        public ProcessItem getcpuUsage(string procname)
        {
            Process[] processCollection = Process.GetProcesses();
            List<ProcessItem> processItems = new List<ProcessItem>();

            foreach (Process p in processCollection)
            {
                try
                {
                    if (procname == p.ProcessName)
                    {
                        ProcessItem processItem = new ProcessItem();
                        processItem.processName = p.ProcessName;
                        processItem.ramUsage = getRamUsage(p);
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

            Parallel.ForEach(processCollection, (p) =>{
                ProcessItem processItem = new ProcessItem();
                
                processItem.processName = p.ProcessName;
                processItem.ramUsage = getRamUsage(p);
                processItems.Add(processItem);
            });
            
            return processItems;
        }
    }
}
