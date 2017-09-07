using Domicile.Common;
using System;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Net;
using Unosquare.Labs.EmbedIO;

namespace Domicile.WebServer.Controllers
{
    public class ServicesController : BaseWebApiController
    {
        [WebApiHandler(Unosquare.Labs.EmbedIO.Constants.HttpVerbs.Get, "/api/services")]
        public bool GetServices(Unosquare.Labs.EmbedIO.WebServer webServer, HttpListenerContext context)
        {
            try
            {
                return false;
            }
            catch(Exception ex)
            {
                return HandleError(context, ex, (int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [WebApiHandler(Unosquare.Labs.EmbedIO.Constants.HttpVerbs.Get, "/api/services/{name}")]
        public bool GetService(Unosquare.Labs.EmbedIO.WebServer webServer, HttpListenerContext context, string name)
        {
            try
            {
                return false;
            }
            catch(Exception ex)
            {
                return HandleError(context, ex, (int)System.Net.HttpStatusCode.InternalServerError);
            }
        }
        

    }
}
