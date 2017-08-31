using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domicile.Common;

namespace Domicile.Core.HardwareDevices
{
    public class BaseHardwareDevice : IHardwareDevice
    {
        public HardwareDeviceType HardwareDeviceType { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public Queue<object> SendQueue { get; set; }
        public bool IsEnabled { get; set; }

        public BaseHardwareDevice(IPAddress address, int port)
        {
            Address = address;
            Port = port;
            SendQueue = new Queue<object>();
            IsEnabled = false;
        }

        public BaseHardwareDevice()
        {
            
        }
    }
}
