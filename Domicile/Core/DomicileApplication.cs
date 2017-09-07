using System.Collections.Generic;
using Domicile.Core.Services;
using Domicile.Common;
using Domicile.Common.Extentions;
using Domicile.WebServer;
using Domicile.Common.Logging;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System;

namespace Domicile.Core
{
    public class DomicileApplication : IDomicileApplication
    {
        private static IDomicileApplication _domicileApplicationInstance;

        [ImportMany]
        private readonly List<IService> _services;
        private readonly ILog _log;
        private readonly BaseServiceContext _baseServiceContext;

        /// <summary>
        /// 
        /// </summary>
        public DomicileApplication()
        {
            _log = new ConsoleLog();
            _services = new List<IService>();

            _baseServiceContext = new BaseServiceContext(
                _log,
                _services);
        }

        /// <summary>
        /// Return wether the application is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Return the Services list as Read Only list.
        /// </summary>
        public IReadOnlyCollection<IService> Services => _services.AsReadOnly();

        /// <summary>
        /// Setup services required by Domicile to function properly. 
        /// This will call <see cref="IService.OnRegistered"/> on all available services.
        /// </summary>
        public void Setup()
        {
            //var systemService = new SystemService(this);
            //var hardwareDeviceService = new HardwareDeviceService();
            //var webServerService = new WebServerService();

            //// TODO: 

            //_services.Add(systemService);
            //_services.Add(hardwareDeviceService);
            //_services.Add(webServerService);

            _log.Informational("Checking for services...");

            // AggregateCatalog combines all catalogs
            var serviceCatalog = new AggregateCatalog();
            var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            serviceCatalog.Catalogs.Add(directoryCatalog);
            var container = new CompositionContainer(serviceCatalog);

            try
            {
                _log.Informational("Composing Services");
                container.ComposeParts(this);
            }
            catch(CompositionException compositionException)
            {
                _log.Error("Failed composing services");
                _log.Error(compositionException.Message);
            }

            // Ensure that all the folders that are necessary are created/exist
            Directory.CreateDirectory("Services");

            foreach (var service in _services)
            {
                service.OnRegistered(_baseServiceContext);
                _log.Informational("Called -> OnRegistered() for Service:  " + service.Name);
            }
        }

        /// <summary>
        /// Start and run all services for Domicile.
        /// This will call <see cref="IService.OnStartup"/> for registered services.
        /// </summary>
        public void Run()
        {
            foreach (var service in _services)
            {
                service.OnStartup();
                _log.Informational("Called -> OnStartup() for Service:  " + service.Name);
            }

            IsActive = true;
        }

        /// <summary>
        /// Shutdown Domicile and all underlaying services.
        /// This will call <see cref="IService.OnShutdown"/> for registered services.
        /// </summary>
        public void Shutdown()
        {
            foreach (var service in _services)
            {
                service.OnShutdown();
                _log.Informational("Called -> OnShutdown() for Service:  " + service.Name);
            }

            IsActive = false;
        }
    }
}
