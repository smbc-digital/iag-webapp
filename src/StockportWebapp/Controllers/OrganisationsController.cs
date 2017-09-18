using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ProcessedModels;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class OrganisationsController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IViewRender _viewRender;
        private readonly ILogger<OrganisationsController> _logger;
        private readonly IApplicationConfiguration _configuration;

        public OrganisationsController(IProcessedContentRepository repository, IViewRender viewRender, ILogger<OrganisationsController> logger, IApplicationConfiguration configuration)
        {
            _repository = repository;
            _viewRender = viewRender;
            _logger = logger;
            _configuration = configuration;
        }
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("/organisations/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _repository.Get<Organisation>(slug);

            if (!response.IsSuccessful())
                return response;

            var organisation = response.Content as ProcessedOrganisation;

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(organisation);
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet]
        [Route("/organisations/exportpdf/{slug}")]
        [Route("/organisations/export/{slug}")]
        public async Task<IActionResult> Print([FromServices] INodeServices nodeServices, [FromServices] CurrentEnvironment environment, string slug, [FromQuery] bool returnHtml = false, [FromQuery] bool print = false)
        {
            ViewBag.CurrentUrl = Request?.GetUri();

            _logger.LogInformation(string.Concat("Exporting organisation ", slug, " to pdf"));

            var response = await _repository.Get<Organisation>(slug);

            if (!response.IsSuccessful()) return response;

            var organisation = response.Content as ProcessedOrganisation;

            var renderedExportStyles = _viewRender.Render("Shared/ExportStyles", _configuration.GetExportHost());
            var printScript = print ? _viewRender.Render("Shared/ExportPrint", organisation) : string.Empty;
            var renderedHtml = _viewRender.Render("Shared/Organisations/OrganisationsDetail", organisation);
            var joinedHtml = string.Concat(renderedExportStyles, printScript, renderedHtml);

            // if raw html is requested, simply return the html instead
            if (returnHtml || print) return Content(joinedHtml, "text/html");

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", new { data = joinedHtml, delay = 1000 });

            if (result == null) _logger.LogError(string.Concat("Failed to export organiation ", slug, " to pdf"));

            return new FileContentResult(result, "application/pdf");
        }
    }
}
