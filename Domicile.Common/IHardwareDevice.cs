using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface IHardwareDevice
    {
        Dictionary<string, object> Configuration { get; }
        HardwareDeviceType HardwareDeviceType { get; }
        string IpAddress { get; }
        int Port { get; }

        // TODO: A method for sending messages to the hardware device
        // A NEW ONE
        //void Send(object data);
    }
}
