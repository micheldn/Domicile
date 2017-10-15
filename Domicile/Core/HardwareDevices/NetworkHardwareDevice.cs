using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Core.HardwareDevices
{
    public class NetworkHardwareDevice
    {
        public BaseHardwareDevice HardwareDevice { get; private set; }
        public TcpClient Client { get; private set; }

        public NetworkHardwareDevice(TcpClient client, BaseHardwareDevice hardwareDevice)
        {
            HardwareDevice = hardwareDevice;
            Client = client;
        }
    }
}
