using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domicile.Common;
using System.Net.Sockets;
using Domicile.Core.Events;

namespace Domicile.Core.HardwareDevices
{
    public class LEDHardwareDevice : BaseHardwareDevice
    {
        public LEDHardwareDevice(string name, string version) : base(name, version)
        {
            HardwareDeviceType = HardwareDeviceType.Lamp;
        }
    }
}
