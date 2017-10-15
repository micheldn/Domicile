using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface IDomicileApplication
    {
        IReadOnlyCollection<IService> Services { get; }
        void Setup();
        void Run();
        void Shutdown();
    }
}
