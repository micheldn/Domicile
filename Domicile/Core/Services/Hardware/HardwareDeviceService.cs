using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Timers = System.Timers;
using System.Threading.Tasks;
using Domicile.Core.HardwareDevices;
using Domicile.Common;
using Domicile.Common.Extentions;
using System.Net.Sockets;
using Domicile.Core.Events;
using Domicile.Core.Network;

namespace Domicile.Core.Services.Hardware
{
    public class HardwareDeviceService : IService
    {
        private IServiceContext _serviceContext;
        private Thread _hardareDeviceThread;
        private bool _isRunning;
        private NetServer _server;
        private Dictionary<NetClient, BaseHardwareDevice> _connectedDeviceClients;
        private Timers.Timer _heartbeatService; 

        public Guid Id { get; private set; }
        public ConcurrentDictionary<string, object> ServiceVariables { get; set; }
        public string Name { get; set; }
        public ServiceType ServiceType => ServiceType.HardwareDevice;
        public bool IsActive => _isRunning;
        public List<BaseHardwareDevice> ConnectedDevices => _connectedDeviceClients.Values.ToList();

        public HardwareDeviceService()
        {

        }

        public void OnRegistered(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
            _isRunning = false;

            Id = Guid.NewGuid();
            Name = "HardwareDeviceService";
            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("IsRunning", _isRunning);
            //ServiceVariables.TryAdd("HeartbeatInterval", ); //TODO

            _connectedDeviceClients = new Dictionary<NetClient, BaseHardwareDevice>();

            _server = new NetServer(12500);

            _server.ClientConnected += (object o, ClientEventArgs e) =>
            {
                Console.WriteLine($"Client connected from {e.Client.IPAddress.ToString()}:{e.Client.Port}");

                _connectedDeviceClients.Add(e.Client, new BaseHardwareDevice("", ""));

                if(_connectedDeviceClients.Keys.Contains(e.Client))
                {

                }
                else
                {
                    // Start Authorization and Identification process
                }

                e.Client.DataReceived += (object sender, DataEventArgs dataReceived) =>
                {
                    if(dataReceived.DataAsString.Contains("Name") && dataReceived.DataAsString.Contains("Version"))
                    {
                        var info = dataReceived.DataAsString.Trim().Replace("\r", "");
                        string[] information = info.Split(';');

                        _connectedDeviceClients.Add(e.Client, new BaseHardwareDevice(
                            information[1].Split('=')[1], 
                            information[2].Split('=')[1]));
                    }

                    if(dataReceived.DataAsString.Contains("Heartbeat"))
                    {

                    }
                };
            };

            _server.ClientDisconnected += (object o, ClientEventArgs e) =>
            {
                _connectedDeviceClients.Remove(e.Client);
            };

            _heartbeatService = new Timers.Timer();
            _heartbeatService.Elapsed += (object sender, Timers.ElapsedEventArgs e) =>
            {
                if(_connectedDeviceClients.Count > 0)
                {
                    foreach(var client in _connectedDeviceClients.Keys)
                    {
                        client.Send("Type=Heartbeat;");
                    }                    
                }
            };
            _heartbeatService.Interval = 10000;
            _heartbeatService.Start();
        }

        public void OnStartup()
        { 
            _isRunning = true;

            UpdateServiceVariable("IsRunning", _isRunning);


            _hardareDeviceThread = new Thread(Run);
            _hardareDeviceThread.Start();

            _server.Start();

            _serviceContext.Log.Informational(">>> Started internal communication server");
        }

        public void OnShutdown()
        {
            _isRunning = false;

            UpdateServiceVariable("IsRunning", _isRunning);

            _hardareDeviceThread = null;

            _server.Stop();

            _serviceContext.Log.Informational(">>> Stopped internal communication server");
        }

        private void Run()
        {
            try
            {
                // How does device discovery work?
                // Is the server passively waiting for connections?
                // Is the device passively waiting for dicovery pings?

                // TODO: Test methods for passive/active discovery of hardware devices             

                while (_isRunning)
                {
                    // TODO: Discover Arduino devices on the same network as this application
                    // listen/check for devices on the same network
                    //     if new device
                    //         add to list
                }
            }
            catch(Exception ex)
            {
                _serviceContext.Log.Error(">>> Failed to start internal communication server");
                _serviceContext.Log.Error(ex.ToString());
            }
        }

        private bool UpdateServiceVariable(string variableName, object newVariableValue)
        {
            // TODO: Come up with a way to show exceptions, without stopping the application to a halt
            // Possible: Catch the exception and log it
            if (variableName == null)
            {
                _serviceContext.Log.Error("HardwareDeviceService: Variable name cannot be null when updating");
                return false;
            }

            if (newVariableValue == null)
            {
                _serviceContext.Log.Error("HardwareDeviceService: Variable value cannot be null when updating");
                return false;
            }

            if (ServiceVariables != null)
            {
                return ServiceVariables.TryUpdate(variableName, newVariableValue, ServiceVariables[variableName]);
            }

            return false;
        }
    }
}
