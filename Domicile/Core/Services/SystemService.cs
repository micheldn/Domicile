using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Domicile.Common;

namespace Domicile.Core.Services
{
    public class SystemService : IService
    {
        private ILog _log;
        private readonly IDomicileApplication _domicileApplication;
        private bool _isActive;

        private Timer _systemTimer;

        public ConcurrentDictionary<string, object> ServiceVariables { get; private set; }
        public string Name => "SystemService";
        public ServiceType ServiceType => ServiceType.System;

        public SystemService(IDomicileApplication domicileApplication)
        {
            _domicileApplication = domicileApplication;
        }

        public bool IsActive => _isActive;

        public void OnRegistered(IServiceContext serviceContext)
        {
            _log = serviceContext.Log;

            _systemTimer = new Timer(1000);

            ServiceVariables = new ConcurrentDictionary<string, object>();
            ServiceVariables.TryAdd("Shutdown", false);
            ServiceVariables.TryAdd("CPUUsagePercentage", 0);
            ServiceVariables.TryAdd("RAMUsagePercentage", 0);
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
