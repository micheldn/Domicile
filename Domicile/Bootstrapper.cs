using System;
using System.Collections;
#if DEBUG
using System.Diagnostics;
#endif
using Domicile.Core;
using Domicile.Common;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Domicile
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register<IDomicileApplication>(DomicileApplication.GetInstance());

            pipelines.OnError.AddItemToEndOfPipeline((context, error) =>
            {
                Console.WriteLine(error);
#if DEBUG
                Debug.WriteLine(error);
#endif
                return error;
            });
        }
    }
}