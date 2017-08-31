using System;
using System.Threading;
using Domicile.Core;
using Domicile.Core.Services;
using Nancy.Hosting.Self;
using Nancy.Routing;

namespace Domicile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Get DomicileApplication instance
            var app = DomicileApplication.GetInstance();

            // Do Setup
            app.Setup();

            // Do Run
            app.Run();

#if DEBUG
            Console.ReadKey();
#endif

            app.Shutdown();

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
