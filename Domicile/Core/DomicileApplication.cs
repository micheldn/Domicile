using System.Collections.Generic;

using Domicile.Common;
using Domicile.Common.Extentions;
using Domicile.Common.Logging;
using Domicile.Core.Services;
using Domicile.Core.Services.Web;
using Domicile.Core.Services.System;
using Domicile.Core.Services.Hardware;
using Nancy.TinyIoc;

namespace Domicile.Core
{
    public class DomicileApplication : IDomicileApplication
    {
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

            TinyIoCContainer.Current.Register<IDomicileApplication, DomicileApplication>(this, "DomicileApplication");
        }

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
            var systemService = new SystemService();
            var hardwareDeviceService = new HardwareDeviceService();
            var webServerService = new WebServerService();

            // TODO: 

            _services.Add(systemService);
            _services.Add(hardwareDeviceService);
            _services.Add(webServerService);

            foreach (var service in _services)
            {
                _log.Informational("Called -> OnRegistered() for Service:  " + service.Name);
                service.OnRegistered(_baseServiceContext);
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
                _log.Informational("Called -> OnStartup() for Service:  " + service.Name);
                service.OnStartup();
            }
        }

        /// <summary>
        /// Shutdown Domicile and all underlaying services.
        /// This will call <see cref="IService.OnShutdown"/> for registered services.
        /// </summary>
        public void Shutdown()
        {
            foreach (var service in _services)
            {
                _log.Informational("Called -> OnShutdown() for Service:  " + service.Name);
                service.OnShutdown();
            }
        }
    }
}
