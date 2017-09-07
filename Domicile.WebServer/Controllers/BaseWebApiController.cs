using System;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Net;

namespace Domicile.WebServer.Controllers
{
    public class BaseWebApiController : WebApiController
    {
        protected bool HandleError(HttpListenerContext context, Exception ex, int statusCode = 500)
        {
            var errorResponse = new
            {
                Title = "Unexpected Error",
                ErrorCode = ex.GetType().Name,
                Description = ex.Message
            };

            context.Response.StatusCode = statusCode;
            return context.JsonResponse(errorResponse);
        }
    }
}
