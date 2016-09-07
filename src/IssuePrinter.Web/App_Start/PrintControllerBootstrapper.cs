using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

using IssuePrinter.Core;
using IssuePrinter.Web.Controllers;
using Atlassian.Jira;

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

            return CreatePrintController();
        }

        private static IHttpController CreatePrintController()
        {
            var config = CreatePrintServiceConfig();

            var jiraRestClient = Jira.CreateRestClient(config.JiraHost, config.JiraUsername, config.JiraPassword);
            jiraRestClient.Debug = true;
            jiraRestClient.MaxIssuesPerRequest = 100;

            var client = new JiraClient(jiraRestClient);
            var issuePrinter = new WindowsIssuePrinter(config.PrinterName);

            return new PrintController(new PrintService(client, issuePrinter));
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