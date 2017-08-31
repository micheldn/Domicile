using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domicile.Core.HardwareDevices;
using Domicile.Plugin;

namespace Domicile.Core.Services
{
    public class HardwareDeviceService : IService
    {
        private IServiceContext _serviceContext;
        private Thread _hardareDeviceThread;
        private Dictionary<Guid, BaseHardwareDevice> _hardwareDevices;
        private bool _isRunning;
        
        public ConcurrentDictionary<string, object> ServiceVariables { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<Guid, BaseHardwareDevice> ConnectedDevices { get; set; }

        public HardwareDeviceService()
        {

        }

        public void OnRegistered(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
            _isRunning = false;
            
            Name = "HardwareDeviceService";
            IsActive = _isRunning;
            ConnectedDevices = new Dictionary<Guid, BaseHardwareDevice>();
            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("IsRunning", _isRunning);

            // TODO: Find a solution to properly serialize BaseHardwareDevice
            // Currently, when you call {url}/services in the browser, it will return a Json Parse Error.
            // This error only seems to occur when you uncomment these additions to the list below.
            // Possible: BaseHardwareDevice or LEDHardwareDevice are not serializable
            // Possible: Issue trying to convert a List with multiple items to a single value for Json
            // Possible: BaseHardwareDevice to complex to serialize with default settings

            //ConnectedDevices.Add(Guid.NewGuid(), new LEDHardwareDevice(IPAddress.Parse("192.168.1.111"), 1234));
            //ConnectedDevices.Add(Guid.NewGuid(), new LEDHardwareDevice(IPAddress.Parse("192.168.1.112"), 532));
            //ConnectedDevices.Add(Guid.NewGuid(), new LEDHardwareDevice(IPAddress.Parse("192.168.1.113"), 4654));

            // Test variable to check if Lists are possible, they are
            //ServiceVariables.TryAdd("Test", new List<int>() {10, 10, 11, 12});

            _hardareDeviceThread = new Thread(Run);
            _hardareDeviceThread.Start();
        }

        public void OnStartup()
        { 
            _isRunning = true;
            UpdateServiceVariable("IsRunning", _isRunning);
        }

        public void OnShutdown()
        {
            _isRunning = false;
            UpdateServiceVariable("IsRunning", _isRunning);
        }

        private void Run()
        {
            // How does device discovery work?
            // Is the server passively waiting for connections?
            // Is the device passively waiting for dicovery pings?

            // TODO: Test methods for passive/active discovery of hardware devices

            while (_isRunning)
            {
                // TODO: Determine wether any variables need to be updated or not
                // Possible: Move the update to another thread/timer/not-in-this-while-loop
                UpdateServiceVariable("IsRunning", _isRunning);

                // TODO: Discover Arduino devices on the same network as this application
                // listen/check for devices on the same network
                //     if new device
                //         add to list


            }
        }

        private bool UpdateServiceVariable(string variableName, object newVariableValue)
        {
            // TODO: Come up with a way to show exceptions, without stopping the application to a halt
            // Possible: Catch the exception and log it
            if (variableName == null || newVariableValue == null)
                return false;

            if (ServiceVariables != null)
            {
                return ServiceVariables.TryUpdate(variableName, newVariableValue, ServiceVariables[variableName]);
            }

            return false;
        }
    }
}
