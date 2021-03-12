using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.FeatureToggling;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IDocumentPageRepository _documentPageRepository;
        private readonly IContactUsMessageTagParser _contactUsMessageParser;
        private readonly FeatureToggles _featureToggles;

        public DocumentController(
            IProcessedContentRepository repository,
            IContactUsMessageTagParser contactUsMessageParser,
            IDocumentPageRepository documentPageRepository,
            FeatureToggles featureToggles
            )
        {
            _repository = repository;
            _contactUsMessageParser = contactUsMessageParser;
            _documentPageRepository = documentPageRepository;
            _featureToggles = featureToggles;
        }

        [Route("/document-page/{documentPageSlug}")]
        public async Task<IActionResult> Index(string documentPageSlug)
        {
            if(!_featureToggles.DocumentPage)
            {
                RedirectToAction("Error", "Error", new { id = 404 });
            }

            var documentPageHttpResponse = await _documentPageRepository.Get(documentPageSlug);

            if (!documentPageHttpResponse.IsSuccessful())
                return documentPageHttpResponse;

            var documentPage = documentPageHttpResponse.Content as ProcessedDocumentPage;

            var viewModel = new DocumentPageViewModel(documentPage);

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(viewModel);
        }
    }
}