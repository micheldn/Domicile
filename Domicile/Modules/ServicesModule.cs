using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Plugin;
using Nancy;
using Nancy.Responses;

namespace Domicile.Modules
{
    public class ServicesModule : NancyModule
    {
        private IDomicileApplication _domicileApplication;

        public ServicesModule(IDomicileApplication domicileApplication) : base("/api")
        {
            _domicileApplication = domicileApplication;

            Get["/services"] = _ =>
            {
                return domicileApplication.Services == null
                    ? HttpStatusCode.NotFound
                    : Response.AsJson(domicileApplication.Services);
            };

            Get["/services/{name}"] =
                parameters => Response.AsJson(domicileApplication.Services.Single(s => s.Name == parameters.name));

            Put["/services/{name}"] = parameters => null;
        }
    }
}
