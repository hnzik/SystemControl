using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.SystemSettings.Firewall
{
    class FireWallItem
    {
        public bool enabled;
        public string name;
        public string programPath;
        public string localAddress;
        public string remoteAddress;
        public string protocol;
        public string localPort;
        public string remotePort;

        public FireWallItem(bool enabled, string name,  string programPath, string localAddress, string remoteAddress, string protocol, string localPort, string remotePort)
        {
            this.enabled = enabled;
            this.name = name;
            this.programPath = programPath;
            this.localAddress = localAddress;
            this.remoteAddress = remoteAddress;
            this.protocol = protocol;
            this.localPort = localPort;
            this.remotePort = remotePort;
        }
    }
}
