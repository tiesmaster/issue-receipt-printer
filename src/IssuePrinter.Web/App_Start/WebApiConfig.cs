using System.Web.Http;
using System.Web.Http.Dispatcher;

using IssuePrinter.Web.Test;

namespace IssuePrinter.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Replace(typeof(IHttpControllerActivator), new PrintControllerTestBootstrapper());
        }
    }
}