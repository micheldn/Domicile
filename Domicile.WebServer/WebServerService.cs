using Domicile.Common;
using Domicile.Common.Extentions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using EmbedIO = Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Labs.EmbedIO;
using Unosquare.Swan;
using Domicile.WebServer.Controllers;

namespace Domicile.WebServer
{
    public class WebServerService : IService
    {
        private IServiceContext _serviceContext;
        private Thread _hostThread;
        private EmbedIO.WebServer _webServer;

        private ManualResetEvent _manualResetEvent;

        public ConcurrentDictionary<string, object> ServiceVariables { get; private set; }
        public string Name => "WebServerService";
        public ServiceType ServiceType => ServiceType.WebServer;
        public bool IsActive => (bool)ServiceVariables["IsRunning"];

        public void OnRegistered(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;

            _manualResetEvent = new ManualResetEvent(false);

            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("RunOnServiceRegistration", true);
            ServiceVariables.TryAdd("IsRunning", true);
            ServiceVariables.TryAdd("HostUri", new Uri("http://localhost:3579"));
        }

        public void OnStartup()
        {
            var systemService = _serviceContext.Services.Single(s => s.ServiceType == ServiceType.System);

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

            // https://github.com/unosquare/embedio
            using (_webServer = new EmbedIO.WebServer(uri.AbsoluteUri, EmbedIO.Constants.RoutingStrategy.Regex))
            {
#if DEBUG
                Terminal.Settings.DisplayLoggingMessageType = LogMessageType.Debug;
#else
                Terminal.Settings.DisplayLoggingMessageType = LogMessageType.None;
#endif

                // First, we will configure our web server by adding Modules.
                // Please note that order DOES matter.
                // ================================================================================================
                // If we want to enable sessions, we simply register the LocalSessionModule
                // Beware that this is an in-memory session storage mechanism so, avoid storing very large objects.
                // You can use the server.GetSession() method to get the SessionInfo object and manupulate it.
                // You could potentially implement a distributed session module using something like Redis
                _webServer.RegisterModule(new LocalSessionModule());

                // Here we setup serving of static files
                _webServer.RegisterModule(new StaticFilesModule("Content"));
                // The static files module will cache small files in ram until it detects they have been modified.
                _webServer.Module<StaticFilesModule>().UseRamCache = true;
                _webServer.Module<StaticFilesModule>().DefaultExtension = ".html";
                // We don't need to add the line below. The default document is always index.html.
                _webServer.Module<StaticFilesModule>().DefaultDocument = "index.html";

                // Enable CORS to enable CRUD operations over http
                _webServer.EnableCors();

                // Register Api module
                _webServer.RegisterModule(new WebApiModule());

                // Register the Service controller
                //var servicesController = new ServicesController(_serviceContext);
                _webServer.Module<WebApiModule>().RegisterController<ServicesController>();

                // Once we've registered our modules and configured them, we call the RunAsync() method.
                _webServer.RunAsync();

                // 
                _serviceContext.Log.Informational("The WebServer is running on: " + uri.AbsoluteUri);

                while ((bool)ServiceVariables["IsRunning"])
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
