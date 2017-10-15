using Domicile.Core.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Core.Network
{
    public class NetClient
    {
        private int _bufferSize;
        private byte[] _buffer;
        private TcpClient _tcpClient;
        private StringBuilder _stringBuilder;

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }

        public delegate void DataEventHandler(object sender, DataEventArgs e);

        public DataEventHandler DataReceived;

        public NetClient(TcpClient client)
        {
            _bufferSize = 1024;
            _buffer = new byte[1024];
            _stringBuilder = new StringBuilder();

            _tcpClient = client;
            _tcpClient.GetStream().BeginRead(_buffer, 0, _bufferSize, ReadData, client);

            IPAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
            Port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
        }

        public void Send(byte[] data)
        {
            if (!CheckConnection())
                return;

            data[data.Length] = 0x13; // byte value for \r
            _tcpClient.GetStream().BeginWrite(data, 0, data.Length, WriteDone, _tcpClient);
        }

        public void Send(string data)
        {
            if (!CheckConnection())
                return;

            using (var writer = new StreamWriter(_tcpClient.GetStream(), Encoding.ASCII))
            {
                writer.Write(data);
            }
        }

        private void ReadData(IAsyncResult asyncResult)
        {
            try
            {
                if (!CheckConnection())
                    return;

                var content = "";

                var client = (TcpClient)asyncResult.AsyncState;

                int bytesRead = client.GetStream().EndRead(asyncResult);

                if (bytesRead > 0)
                {
                    _stringBuilder.Append(Encoding.ASCII.GetString(_buffer, 0, bytesRead));

                    Console.WriteLine("Hello?");

                    content = _stringBuilder.ToString();
                    if (content.IndexOf("\r") > -1)
                    {
                        DataReceived?.Invoke(this, new DataEventArgs(this, Encoding.ASCII.GetBytes(content)));
                    }
                    else
                    {
                        client.GetStream().BeginRead(_buffer, 0, _bufferSize, ReadData, client);
                    }
                }
            }
            catch(ObjectDisposedException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                ReadData(asyncResult);
            }
        }

        private void WriteDone(IAsyncResult asyncResult)
        {
            if (!CheckConnection())
                return;

            var client = (TcpClient)asyncResult.AsyncState;

            client.GetStream().EndWrite(asyncResult);
        }

        private bool CheckConnection()
        {
            bool connected = _tcpClient.Connected;
            if(!connected )
            {
                _tcpClient.Close();
            }
            return connected;
        }
    }
}
