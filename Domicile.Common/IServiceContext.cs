using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface IServiceContext
    {
        ILog Log { get; }
        IReadOnlyCollection<IService> Services { get; }
    }
}
