using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.SystemSettings.Networking
{
    struct AdapterProperties
    {
        public string adapterDescription;
        public string ipv4Address;
        public string ipv6Address;
        public string subnetMask;
        public string subnetMaskV6;
        public string defaultGateway;
        public string Ipv6defaultGateway;

        public string mac;
        public string dns1;
        public string dns2;
        public bool dynamicDns;
        public bool dhcp;
    }


    class NetworkHandaler
    {
        public NetworkInterface[] getNetworkAdapters()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        public NetworkInterface getNetworkAdapterByName(string adapterName)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Name == adapterName)
                {
                    return adapter;
                }
            }
            return null;
        }

        public List<String> getAdapterNames()
        {
            List<string> adapterNameList = new List<string>();

            foreach (NetworkInterface adapter in this.getNetworkAdapters())
            {
                adapterNameList.Add(adapter.Name);
            }
            return adapterNameList;
        }
        private bool DNSAutoOrStatic(string NetworkAdapterGUID)
        {
            string path = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + NetworkAdapterGUID;
            string ns = (string)Registry.GetValue(path, "NameServer", null);
            if (String.IsNullOrEmpty(ns))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AdapterProperties getAdapterProperties(NetworkInterface adapter) {
            AdapterProperties adapterProperties = new AdapterProperties();
            adapterProperties.ipv4Address = getAdapterIpv4Adress(adapter);
            adapterProperties.ipv6Address = getAdapterIpv6Adress(adapter);
            adapterProperties.defaultGateway = getAdapterGateWay(adapter, true);
            adapterProperties.subnetMask = getAdapterSubnetMask(adapter);
            adapterProperties.dhcp = adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
            adapterProperties.adapterDescription = adapter.Description;
            adapterProperties.subnetMaskV6 = getIPV6Prefix(adapter.Description);
            adapterProperties.Ipv6defaultGateway = getAdapterGateWayV6(adapter, false);
            if (getDns(adapter, false).Length > 0)
            {
                adapterProperties.dns1 = getDns(adapter, false)[0];
            }
            if (getDns(adapter, false).Length > 1)
            {
                adapterProperties.dns2 = getDns(adapter, false)[1];
            }

            adapterProperties.dynamicDns = DNSAutoOrStatic(adapter.Id);
            return adapterProperties;
        }


        private string[] getDns(NetworkInterface adapter, bool ipv4)
        {
            List<string> ipAdrres = new List<string>();
            foreach (IPAddress ip in adapter.GetIPProperties().DnsAddresses)
            {
                ipAdrres.Add(ip.ToString());
            }
            return ipAdrres.ToArray();
        }

        private string getAdapterGateWay(NetworkInterface adapter, bool ipv4)
        {
            foreach (GatewayIPAddressInformation ip in adapter.GetIPProperties().GatewayAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.Address.ToString();
                }
            }
            return null;
        }

        private string getAdapterGateWayV6(NetworkInterface adapter, bool ipv4)
        {
            IPv6InterfaceProperties p = adapter.GetIPProperties().GetIPv6Properties();
            Console.WriteLine(p.Mtu);
            return null;
        }

        private string getAdapterSubnetMask(NetworkInterface adapter)
        {
            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.IPv4Mask.ToString();
                }
            }
            return null;
        }

        private string getIPV6Prefix(string nicDescription)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(mo => (bool)mo["IPEnabled"] && (string)mo["Description"] == nicDescription))
                    {
                        string[] subnets = (string[])managementObject["IPSubnet"];
                        return subnets[1];
                    }
                }
            }
            return null;
        }

        private string getAdapterIpv4Adress(NetworkInterface adapter)
        {
            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.Address.ToString();
                }
            }
            return null;
        }

        private string getAdapterIpv6Adress(NetworkInterface adapter)
        {
            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    return ip.Address.ToString();
                }
            }
            return null;
        }

        public bool verifyIpAdress(string ipToVerify)
        {
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(ipToVerify, out ip);
            if (ValidateIP)
                return true;
            else
                return false;
        }

        public void setIpForAdapter(string nicDescription, string[] ipAddresses, string subnetMask, string gateway)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(mo => (bool)mo["IPEnabled"] && (string)mo["Description"] == nicDescription))
                    {
                        using (var newIP = managementObject.GetMethodParameters("EnableStatic"))
                        {
                            if (ipAddresses != null || !String.IsNullOrEmpty(subnetMask))
                            {
                                if (ipAddresses != null)
                                {
                                    newIP["IPAddress"] = ipAddresses;
                                }

                                if (!String.IsNullOrEmpty(subnetMask))
                                {
                                    newIP["SubnetMask"] = Array.ConvertAll(ipAddresses, _ => subnetMask);
                                }

                                managementObject.InvokeMethod("EnableStatic", newIP, null);
                            }

                            if (!String.IsNullOrEmpty(gateway))
                            {
                                using (var newGateway = managementObject.GetMethodParameters("SetGateways"))
                                {
                                    newGateway["DefaultIPGateway"] = new[] { gateway };
                                    newGateway["GatewayCostMetric"] = new[] { 1 };
                                    managementObject.InvokeMethod("SetGateways", newGateway, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void setDNS(string nicDescription, string[] dnsServers)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(mo => (bool)mo["IPEnabled"] && (string)mo["Description"] == nicDescription))
                    {
                        using (var newDNS = managementObject.GetMethodParameters("SetDNSServerSearchOrder"))
                        {
                            newDNS["DNSServerSearchOrder"] = dnsServers;
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        }
                    }
                }
            }
        }

    }

}

