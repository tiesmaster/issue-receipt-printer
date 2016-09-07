using System.Web.Http;

using IssuePrinter.Core;

namespace IssuePrinter.Web.Controllers
{
    public class PrintController : ApiController
    {
        private readonly PrintService _printService;

        public PrintController(PrintService printService)
        {
            _printService = printService;
        }

        // GET: /Print/Issue?key=FIN-1360
        [HttpGet]
        public IHttpActionResult Issue(string key)
        {
            _printService.PrintIssue(key);
            return Ok();
        }

        // GET: /Print/Sprint?key=488
        [HttpGet]
        public IHttpActionResult Sprint(string key)
        {
            _printService.PrintSprintIssues(key);
            return Ok();
        }
    }
}