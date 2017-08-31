using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Core.Events;
using Domicile.Core.Extentions;
using Domicile.Core.Logging;
using Domicile.Core.Services;
using Domicile.Common;

namespace Domicile.Core
{
    public class DomicileApplication : IDomicileApplication
    {
        private static IDomicileApplication _domicileApplicationInstance;

        private readonly List<IService> _services;
        private readonly ILog _log;
        private readonly BaseServiceContext _baseServiceContext;

        /// <summary>
        /// Create a new instance if current instance is null
        /// </summary>
        /// <returns>Singleton instance of <see cref="DomicileApplication"/></returns>
        public static IDomicileApplication GetInstance()
        {
            if(_domicileApplicationInstance == null)
                _domicileApplicationInstance = new DomicileApplication();

            return _domicileApplicationInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        private DomicileApplication()
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
            var systemService = new SystemService(this);
            var hardwareDeviceService = new HardwareDeviceService();
            var nancyWebService = new NancyWebService();

            // TODO: 

            _services.Add(systemService);
            _services.Add(hardwareDeviceService);
            _services.Add(nancyWebService);

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
