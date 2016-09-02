using System.Configuration;
using System.Web.Http;

using IssuePrinter.Core;

namespace IssuePrinter.Web.Controllers
{
    public class PrintController : ApiController
    {
        private readonly PrintService _printService;

        public PrintController()
        {
            var ticketPrintServiceConfig = new TicketPrintServiceConfig
            {
                JiraHost = ConfigurationManager.AppSettings["JiraHost"],
                JiraUsername = ConfigurationManager.AppSettings["JiraUsername"],
                JiraPassword = ConfigurationManager.AppSettings["JiraPassword"],
                PrinterName = ConfigurationManager.AppSettings["PrinterName"],
            };

            _printService = new PrintService(ticketPrintServiceConfig);
        }

        // GET: /Print/Issue/MF-800
        [HttpGet]
        public IHttpActionResult Issue(string key)
        {
            _printService.PrintIssue(key);
            return Ok();
        }

        // GET: /Print/Sprint/488
        [HttpGet]
        public IHttpActionResult Sprint(string key)
        {
            _printService.PrintSprintIssues(key);
            return Ok();
        }
    }
}