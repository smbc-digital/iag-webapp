using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using System.Linq;
using StockportWebapp.Utils;
using System;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class ShowcaseController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ShowcaseController> _logger;

        public ShowcaseController(IProcessedContentRepository repository, ILogger<ShowcaseController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Route("/showcase/{slug}")]
        public async Task<IActionResult> Showcase(string slug)
        { 
            var showcaseHttpResponse = await _repository.Get<Showcase>(slug);

            if (!showcaseHttpResponse.IsSuccessful())
                return showcaseHttpResponse;

            var showcase = showcaseHttpResponse.Content as ProcessedShowcase;

            return View(showcase);
        }

        [Route("/showcase/{slug}/previousconsultations")]
        public async Task<IActionResult> PreviousConsultations(string slug, [FromQuery]int Page)
        {
            var showcaseHttpResponse = await _repository.Get<Showcase>(slug);

            if (!showcaseHttpResponse.IsSuccessful())
                return showcaseHttpResponse;

            var showcase = showcaseHttpResponse.Content as ProcessedShowcase;

            var result = new PreviousConsultaion()
            {
                Title = showcase.Title,
                Slug = showcase.Slug,
                Consultations = showcase.Consultations.Where(i => i.ClosingDate <= DateTime.Now && i.ClosingDate > DateTime.Now.AddYears(-1)).OrderByDescending(i => i.ClosingDate).ToList(),
                Pagination = new Pagination()
            };

            DoPagination(Page, result);

            return View(result);
        }

        private void DoPagination(int currentPageNumber, PreviousConsultaion prevConsultation)
        {
            if (prevConsultation != null && prevConsultation.Consultations.Any())
            {
                int MaxNumberOfItemsPerPage = 10;

                var paginatedList = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    prevConsultation.Consultations.ToList(),
                    currentPageNumber,
                    "Consultations",
                    MaxNumberOfItemsPerPage);

                prevConsultation.Consultations = paginatedList.Items;
                prevConsultation.Pagination = paginatedList.Pagination;

                prevConsultation.Pagination.CurrentUrl = new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query);
            }
            else
            {
                prevConsultation.Pagination = new Pagination();
            }
        }
    }
}