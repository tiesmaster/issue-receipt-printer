using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

using IssuePrinter.Core;
using IssuePrinter.Web.Controllers;

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

            return new PrintController(new PrintService(CreatePrintServiceConfig()));
        }

        private static TicketPrintServiceConfig CreatePrintServiceConfig()
        {
            return new TicketPrintServiceConfig
            {
                JiraHost = ConfigurationManager.AppSettings["JiraHost"],
                JiraUsername = ConfigurationManager.AppSettings["JiraUsername"],
                JiraPassword = ConfigurationManager.AppSettings["JiraPassword"],
                PrinterName = ConfigurationManager.AppSettings["PrinterName"],
            };
        }
    }
}