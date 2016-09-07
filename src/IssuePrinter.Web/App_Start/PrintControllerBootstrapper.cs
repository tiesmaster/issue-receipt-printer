using IssuePrinter.Web.Controllers;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace IssuePrinter.Web
{
    public class PrintControllerBootstrapper : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType != typeof(PrintController))
            {
                throw new ArgumentException(
                    $"{nameof(PrintControllerBootstrapper)} is only capable of bootstrapping the {nameof(PrintController)}",
                    nameof(controllerType));
            }

            return new PrintController();
        }
    }
}