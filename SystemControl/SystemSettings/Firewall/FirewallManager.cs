using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.SystemSettings.Firewall
{
    class FirewallManager
    {
        public int getActiveProfile()
        {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            return fwPolicy2.CurrentProfileTypes();
        }

        public bool isFireWallEnabled(int profileType) {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            return fwPolicy2.FirewallEnabled(profileType);
        }
        public void changeFireWallSettingsFireWall(int profileType,bool enable)
        {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            fwPolicy2.FirewallEnabled[profileType] = enable;
        }

        public void manageFireWallRule(bool enable, string name, int protocol, string port, int direction) {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            IEnumerable Rules = fwPolicy2.Rules as IEnumerable;
            foreach (dynamic rule in Rules)
            {
                try
                {
                    if (rule.Name == name && rule.Direction == direction && rule.LocalPorts == port && rule.Direction == direction)
                    {
                        if (protocol != 0)
                        {
                            if (rule.Protocol == protocol)
                            {
                                rule.Enabled = enable;
                                break;
                            }
                            continue;
                        }
                        else
                        {
                            rule.Enabled = enable;
                            break;
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }

        }


        public void deleteFireWallRule(string name, int protocol, string port, int direction) {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            IEnumerable Rules = fwPolicy2.Rules as IEnumerable;
            string randomName = "LOODQERTZS";
            foreach (dynamic rule in Rules)
            {
                try
                {
                    if (rule.Name == name && rule.Direction == direction && rule.LocalPorts == port && rule.Direction == direction)
                    {
                        if (protocol != 0) {
                            if (rule.Protocol == protocol) {
                                Console.WriteLine(protocol);
                                rule.Name = randomName;
                                break;
                            }
                            continue;
                        }
                        else
                        {
                            rule.Name = randomName;
                            break;
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            fwPolicy2.Rules.Remove(randomName);
        }


        public void addFireWallRule(string name,string description ,string applicationname,int port,int protocol, int direction)
        {
            int currentProfile = this.getActiveProfile();
            Type fWRuleType = Type.GetTypeFromProgID("HNetCfg.fWRule");
            dynamic fwRule = Activator.CreateInstance(fWRuleType) as dynamic;
            fwRule.Name = name;
            if(description != "")
                fwRule.Description = description;
            if(applicationname != "")
                fwRule.Applicationname = applicationname;
            fwRule.Protocol = protocol;
            fwRule.Enabled = true;
            fwRule.Profiles = currentProfile;
            fwRule.Action = 1;
            if(port != 0)
                fwRule.LocalPorts = port;
            fwRule.Direction = direction;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            fwPolicy2.Rules.Add(fwRule);
        }



        public IEnumerable<FireWallItem> getAllFireWallRules(int direction)
        {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as dynamic;
            List<FireWallItem> fireWallItems = new List<FireWallItem>();
            IEnumerable Rules = fwPolicy2.Rules as IEnumerable;
            foreach (dynamic rule in Rules)
            {
                try
                {
                    if (rule.Direction == direction)
                    {
                        fireWallItems.Add(new FireWallItem(rule.Enabled, rule.Name, rule.ApplicationName, rule.LocalAddresses, rule.RemoteAddresses, getProtocoltringFoProtocolNumber(rule.Protocol), rule.LocalPorts, rule.RemotePorts));
                    }
                    else if (rule.Direction == direction) {
                        fireWallItems.Add(new FireWallItem(rule.Enabled, rule.Name, rule.ApplicationName, rule.LocalAddresses, rule.RemoteAddresses, getProtocoltringFoProtocolNumber(rule.Protocol), rule.LocalPorts, rule.RemotePorts));
                    }
                }
                catch (Exception ex) { 
                
                }
            }
            return fireWallItems;
        }

        private string getProtocoltringFoProtocolNumber(int portNumber) {
            switch (portNumber)
            {
                case 6:
                    return "TCP";
                case 17:
                    return "UDP";
                case 2:
                    return "IGMP";
                case 58:
                    return "ICMPv6";
                case 1:
                    return "ICMPv4";
                default:
                    return "ANY";
            }
        }
        
    }
}
