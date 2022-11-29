using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    public class PrivacyNoticeController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public PrivacyNoticeController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("/privacy-notices/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var result = await _repository.Get<PrivacyNotice>(slug);

            if (!result.IsSuccessful())
                return result;

            var viewModel = new PrivacyNoticeViewModel(result.Content as ProcessedPrivacyNotice);

            return View(viewModel);
        }
    }
}
