using Domicile.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Core.Events
{
    public class ClientEventArgs : EventArgs
    {
        public NetClient Client { get; private set; }

        public ClientEventArgs(NetClient client)
        {
            Client = client;
        }
    }
}
