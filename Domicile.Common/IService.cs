using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface IService
    {
        ConcurrentDictionary<string, object> ServiceVariables { get; }
        string Name { get; }
        bool IsActive { get; }
        void OnRegistered(IServiceContext serviceContext);
        void OnStartup();
        void OnShutdown();
    }
}
