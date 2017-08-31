using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domicile.Core.Extentions;
using Domicile.Plugin;
using Nancy.Hosting.Self;

namespace Domicile.Core.Services
{
    public class NancyWebService : IService
    {
        private IServiceContext _serviceContext;
        private NancyHost _nancyHost;
        private Thread _hostThread;

        public ConcurrentDictionary<string, object> ServiceVariables { get; private set; }
        public string Name => "NancyWebService";
        public bool IsActive => (bool)ServiceVariables["IsRunning"];

        public void OnRegistered(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;

            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("RunOnServiceRegistration", true);
            ServiceVariables.TryAdd("IsRunning", true);
            ServiceVariables.TryAdd("HostUri", new Uri("http://localhost:3579"));
        }

        public void OnStartup()
        {
            var systemService = _serviceContext.Services.Single(s => s is SystemService);

            if (systemService == null)
            {
                _serviceContext.Log.Error("Failed finding SystemService");
                return;
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

            using (_nancyHost = new NancyHost(uri))
            {
                _nancyHost.Start();

                _serviceContext.Log.Informational("Your application is running on " + uri);

                while ((bool) ServiceVariables["IsRunning"])
                {
                    Thread.Sleep(1);
                }
            }

            _serviceContext.Log.Informational("NancyWeb host it no longer active");
        }

        public void OnShutdown()
        {
            ServiceVariables["IsRunning"] = false;
        }
    }
}
