using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

using IssuePrinter.Core;

using IssuePrinter.Web.Controllers;

namespace IssuePrinter.Web.Test
{
    internal class PrintControllerTestBootstrapper : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType != typeof(PrintController))
            {
                throw new ArgumentException(
                    $"{nameof(PrintControllerTestBootstrapper)} is only capable of bootstrapping the {nameof(PrintController)}",
                    nameof(controllerType));
            }

            return CreatePrintController();
        }

        private static IHttpController CreatePrintController()
        {
            var client = new TestJiraClient();
            var issuePrinter = new WindowsIssuePrinter("Microsoft Print to PDF");

            return new PrintController(new PrintService(client, issuePrinter));
        }
    }
}