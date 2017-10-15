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
    public class ServicesModule : NancyModule
    {
        private IDomicileApplication _domicileApplication;

        public ServicesModule() : base("/api")
        {
            _domicileApplication = TinyIoCContainer.Current.Resolve<IDomicileApplication>("DomicileApplication");

            Get["/services"] = parameters =>
            {
                return Response.AsJson(_domicileApplication.Services);
            };

            Get["/services/{id}"] = parameters =>
            {
                return Response.AsJson(_domicileApplication.Services.SingleOrDefault(s => s.Id == parameters.id));
            };
        }
    }
}
