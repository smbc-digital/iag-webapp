using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class OrganisationsController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public OrganisationsController(IProcessedContentRepository repository)
        {
            _repository = repository;
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
    }
}