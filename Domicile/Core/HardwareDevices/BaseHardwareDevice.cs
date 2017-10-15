using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domicile.Common;
using System.Net.Sockets;
using Domicile.Core.Events;

namespace Domicile.Core.HardwareDevices
{
    public class BaseHardwareDevice : IHardwareDevice
    {
        public Dictionary<string, object> Configuration { get; private set; }
        public HardwareDeviceType HardwareDeviceType { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public bool IsEnabled { get; set; }

        public BaseHardwareDevice(string name, string version)
        {
            IsEnabled = false;
            Configuration = new Dictionary<string, object>();
            Configuration.Add("Name", name);
            Configuration.Add("Version", version);
            Configuration.Add("Enabled", IsEnabled);
            Configuration.Add("Address", IpAddress);
            Configuration.Add("Port", Port);
        }
    }
}
