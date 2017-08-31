using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Common;

namespace Domicile.Core.Events
{
    public class ServiceRegisteredEventArgs : EventArgs
    {
        public IService Service { get; }

        public ServiceRegisteredEventArgs(IService service)
        {
            Service = service;
        }
    }
}
