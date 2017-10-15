using Domicile.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Core.Events
{
    public class DataEventArgs : EventArgs
    {
        public NetClient Client { get; private set; }
        public byte[] DataAsBytes { get; private set; }
        public string DataAsString { get; private set; }

        public DataEventArgs(NetClient client, byte[] data)
        {
            Client = client;
            DataAsBytes = data;
            DataAsString = Encoding.ASCII.GetString(data);
        }
    }
}
