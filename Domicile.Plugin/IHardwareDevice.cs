using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface IHardwareDevice
    {
        HardwareDeviceType HardwareDeviceType { get; }
        IPAddress Address { get; }
        int Port { get; }

        // TODO: A method for sending messages to the hardware device
        // A NEW ONE
        //void Send(object data);
    }
}
