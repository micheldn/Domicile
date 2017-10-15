using Domicile.Common;
using Nancy;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Modules
{
    public class ApplicationModule : NancyModule
    {
        private IDomicileApplication _domicileApplication;

        public ApplicationModule() : base("/api")
        {
            _domicileApplication = TinyIoCContainer.Current.Resolve<IDomicileApplication>("DomicileApplication");

            Get["/application/shutdown"] = _ =>
            {
                _domicileApplication.Shutdown();
                return true;
            };

            Get["/application/restart"] = _ =>
            {
                //_domicileApplication.Restart();
                return true;
            };
        }
    }
}
