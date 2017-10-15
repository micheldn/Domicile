using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using Domicile.Common;
using Domicile.Common.Extentions;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace Domicile.Core.Services.Web
{
    public class WebServerService : IService
    {
        private IServiceContext _serviceContext;
        private Thread _hostThread;
        private NancyHost _webServer;

        private ManualResetEvent _manualResetEvent;

        public Guid Id { get; private set; }
        public ConcurrentDictionary<string, object> ServiceVariables { get; private set; }
        public string Name => "WebServerService";
        public ServiceType ServiceType => ServiceType.WebServer;
        public bool IsActive => (bool)ServiceVariables["IsRunning"];

        public void OnRegistered(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;

            Id = Guid.NewGuid();

            _manualResetEvent = new ManualResetEvent(false);

            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("RunOnServiceRegistration", true);
            ServiceVariables.TryAdd("IsRunning", true);
            ServiceVariables.TryAdd("HostUri", new Uri("http://localhost:3579"));
        }

        public void OnStartup()
        {
            var systemService = _serviceContext.Services.SingleOrDefault(s => s.ServiceType == ServiceType.System);

            if (systemService == null)
            {
                _serviceContext.Log.Error("Failed finding SystemService");
            }

            ServiceVariables["IsRunning"] = true;

            _hostThread = new Thread(Run);
            _hostThread.Start();
        }

        private void Run()
        {
            // If false, don't bother
            if (!(bool)ServiceVariables["IsRunning"])
                return;

            var uri = (Uri)ServiceVariables["HostUri"];

            using (_webServer = new NancyHost(uri))
            {
                _webServer.Start();
                _serviceContext.Log.Informational("Web host running on: " + uri);

                while ((bool)ServiceVariables["IsRunning"])
                {
                    Thread.Sleep(1);
                }
            }

            _serviceContext.Log.Informational("Web host it no longer active");
        }

        public void OnShutdown()
        {
            ServiceVariables["IsRunning"] = false;
        }
    }
}
