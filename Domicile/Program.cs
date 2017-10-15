using System;
using System.Threading;
using Domicile.Core;
using Domicile.Core.Services;

namespace Domicile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Get DomicileApplication instance
            var app = new DomicileApplication();

            // Do Setup
            app.Setup();

            // Do Run
            app.Run();
        }
    }
}
