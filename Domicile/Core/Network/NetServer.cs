using Domicile.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domicile.Core.Network
{
    public class NetServer
    {
        private TcpListener _internalServerListener;
        private ManualResetEvent _blocking;
        private Thread _serverLoop;
        private bool _isRunning;
        private bool _started;
        private List<NetClient> _connectedClients;

        public int MaximumConnections { get; set; }
        public List<NetClient> ConnectedClients => _connectedClients;

        public delegate void ClientEventHandler(object sender, ClientEventArgs e);

        public ClientEventHandler ClientConnected;
        public ClientEventHandler ClientDisconnected;

        /// <summary>
        /// Initializes a new NetServe instance with default MaximumConnections is 100
        /// </summary>
        public NetServer(int port)
        {
            MaximumConnections = 100;

            _connectedClients = new List<NetClient>();

            _blocking = new ManualResetEvent(false);

            _internalServerListener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;

            _serverLoop = new Thread(Run);
            _serverLoop.Start();

            _internalServerListener.Start();

            _started = true;
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;

            _serverLoop = null;

            _internalServerListener.Stop();
            _connectedClients.Clear();

            _started = false;
        }

        private void Run()
        {
            while (_isRunning)
            {
                if (_started)
                {
                    _blocking.Reset();
                    _internalServerListener.BeginAcceptTcpClient(AcceptClient, _internalServerListener);
                    _blocking.WaitOne();
                }
            }
        }

        private void AcceptClient(IAsyncResult asyncResult)
        {
            _blocking.Set();

            var server = (TcpListener)asyncResult.AsyncState;
            var client = server.EndAcceptTcpClient(asyncResult);

            var netClient = new NetClient(client);
            _connectedClients.Add(netClient);

            ClientConnected?.Invoke(this, new ClientEventArgs(netClient));
        }
    }
}
