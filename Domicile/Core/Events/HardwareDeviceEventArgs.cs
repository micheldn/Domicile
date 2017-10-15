using Domicile.Core.HardwareDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Core.Events
{
    public class HardwareDeviceEventArgs : EventArgs
    {
        public BaseHardwareDevice HardwareDevice { get; private set; }

        public HardwareDeviceEventArgs(BaseHardwareDevice hardwareDevice)
        {
            HardwareDevice = hardwareDevice;
        }
    }
}
