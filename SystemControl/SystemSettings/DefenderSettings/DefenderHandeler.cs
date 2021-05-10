using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.SystemSettings.DefenderSettings
{
    struct DefenderSettings
    {
        public bool realtimeMonitoring, behaviorMonitoring, blockAtFirstSeen, IOAVProtection, signatureDisableUpdateOnStartupWithoutEngine, archiveScanning, intrusionPreventionSystem, scriptScanning, MAPSReporting, highThreatDefaultAction, moderateThreatDefaultAction;
    }

    class DefenderHandeler
    {
        //Bool vals representing defender settings
        public DefenderSettings settings;

        public DefenderHandeler()
        {
            settings = new DefenderSettings();
            this.parseDefenderSettings();
        }   

        public IEnumerable<String> getExistingExclusions()
        {
            List<String> exclusions = new List<String>();
            Process proc = startPowerShellWithParam("Get-MpPreference | Select-Object -Property ExclusionPath -ExpandProperty ExclusionPath");
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                exclusions.Add(line);
            }
            return exclusions;
        }

        public void addExclusion(String path)
        {
            Process proc = startPowerShellWithParam("Add-MpPreference -ExclusionPath " + path);
            
        }

        public void deleteExclusion(String path)
        {
            Process proc = startPowerShellWithParam("Remove-MpPreference -ExclusionPath " + path);
        }

        private void parseDefenderSettings()
        {
            Process proc = startPowerShellWithParam("Get-MpPreference -verbose");
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line.StartsWith(@"DisableRealtimeMonitoring") && line.EndsWith("False"))
                    settings.realtimeMonitoring = true;
                else if (line.StartsWith(@"DisableBehaviorMonitoring") && line.EndsWith("False"))
                    settings.behaviorMonitoring = true;

                else if (line.StartsWith(@"DisableBlockAtFirstSeen") && line.EndsWith("False"))
                    settings.blockAtFirstSeen = true;

                else if (line.StartsWith(@"DisableIOAVProtection") && line.EndsWith("False"))
                    settings.IOAVProtection = true;

                else if (line.StartsWith(@"SignatureDisableUpdateOnStartupWithoutEngine") && line.EndsWith("False"))
                    settings.signatureDisableUpdateOnStartupWithoutEngine = true;

                else if (line.StartsWith(@"DisableArchiveScanning") && line.EndsWith("False"))
                    settings.archiveScanning = true;

                else if (line.StartsWith(@"DisableIntrusionPreventionSystem") && line.EndsWith("False"))
                    settings.intrusionPreventionSystem = true;

                else if (line.StartsWith(@"DisableScriptScanning") && line.EndsWith("False"))
                    settings.scriptScanning = true;

                else if (line.StartsWith(@"MAPSReporting") && !line.EndsWith("0"))
                    settings.MAPSReporting = true;

                else if (line.StartsWith(@"HighThreatDefaultAction") && !line.EndsWith("6"))
                    settings.highThreatDefaultAction = true;

                else if (line.StartsWith(@"ModerateThreatDefaultAction") && !line.EndsWith("6"))
                    settings.moderateThreatDefaultAction = true;
            }
        }

        public void invertDefenderSettings(string command, ref bool currentState)
        {
            string option = "$false";
            if (currentState)
                option = "$true";
            startPowerShell("Set-MpPreference -" + command + " " + option);
            currentState = !currentState;
        }

        private Process startPowerShellWithParam(string param)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = param,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            return proc;
        }

        public void changeDefenderSettings(int option)
        {
            switch (option)
            {
                case 1:
                    invertDefenderSettings("DisableRealtimeMonitoring", ref settings.realtimeMonitoring);
                    break;
                case 2:
                    invertDefenderSettings("DisableBehaviorMonitoring",ref settings.behaviorMonitoring);
                    break;
                case 3:
                    invertDefenderSettings("DisableBlockAtFirstSeen", ref settings.blockAtFirstSeen);
                    break;
                case 4:
                    invertDefenderSettings("DisableIOAVProtection", ref settings.IOAVProtection);
                    break;
                case 5:
                    invertDefenderSettings("SignatureDisableUpdateOnStartupWithoutEngine", ref settings.signatureDisableUpdateOnStartupWithoutEngine);
                    break;
                case 6:
                    invertDefenderSettings("DisableArchiveScanning", ref settings.archiveScanning);
                    break;
                case 7:
                    invertDefenderSettings("DisableIntrusionPreventionSystem", ref settings.intrusionPreventionSystem);
                    break;
                case 8:
                    invertDefenderSettings("DisableScriptScanning", ref settings.scriptScanning);
                    break;
                case 9:
                    invertDefenderSettings("MAPSReporting", ref settings.MAPSReporting);
                    break;
                case 10:
                    invertDefenderSettings("HighThreatDefaultAction", ref settings.highThreatDefaultAction);
                    break;
                case 11:
                    invertDefenderSettings("ModerateThreatDefaultAction", ref settings.moderateThreatDefaultAction);
                    break;

            }

        }

        public DefenderSettings getDefenderSettings() {
            return settings;
        }
        
        private void startPowerShell(string args)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            proc.Start();
        }
    }
}
