using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Plugin;

namespace Domicile.Core.Services
{
    internal class BaseServiceContext : IServiceContext
    {
        private readonly List<IService> _services;

        public BaseServiceContext(ILog log, List<IService> services)
        {
            Log = log;
            _services = services;
        }

        public ILog Log { get; }
        public IReadOnlyCollection<IService> Services => _services.AsReadOnly();
    }
}
