using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domicile.Common;

namespace Domicile.Core.HardwareDevices
{
    public class LEDHardwareDevice : BaseHardwareDevice
    {
        private LEDHardwareDevice()
        {
            
        }
        public LEDHardwareDevice(IPAddress address, int port) : base(address, port)
        {
            HardwareDeviceType = HardwareDeviceType.Lamp;
        }
    }
}
