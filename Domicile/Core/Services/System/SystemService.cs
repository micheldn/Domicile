using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Timers;
using Domicile.Common;
using Nancy.TinyIoc;

namespace Domicile.Core.Services.System
{
    public class SystemService : IService
    {
        private ILog _log;
        private readonly IDomicileApplication _domicileApplication;
        private bool _isActive;

        private Timer _systemTimer;

        public Guid Id { get; private set; }
        public ConcurrentDictionary<string, object> ServiceVariables { get; private set; }
        public string Name => "SystemService";
        public ServiceType ServiceType => ServiceType.System;

        public SystemService()
        {
            _domicileApplication = TinyIoCContainer.Current.Resolve<IDomicileApplication>("DomicileApplication");
        }

        public bool IsActive => _isActive;

        public void OnRegistered(IServiceContext serviceContext)
        {
            _log = serviceContext.Log;

            Id = Guid.NewGuid();

            _systemTimer = new Timer(1000);

            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("shutdown", false);
            ServiceVariables.TryAdd("platform", "");
        }

        public void OnStartup()
        {
            _isActive = true;
        }

        public void OnShutdown()
        {
            _isActive = false;
        }

        public void Shutdown()
        {
            _domicileApplication.Shutdown();
        }
    }
}
