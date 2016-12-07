using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Exceptions;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using StockportWebapp.Http;
using System.Linq;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.ArticleStartPageNewsDuration)]
    public class ArticleController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ArticleController> _logger;
        private readonly IContactUsMessageTagParser _contactUsMessageParser;
        private readonly FeatureToggles _featureToggles;

        public ArticleController(IProcessedContentRepository repository, ILogger<ArticleController> logger, IContactUsMessageTagParser contactUsMessageParser, FeatureToggles featureToggles)
        {
            _repository = repository;
            _logger = logger;
            _contactUsMessageParser = contactUsMessageParser;
            _featureToggles = featureToggles;
        }

        [Route("/{articleSlug}")]
        public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message)
        { 
            var articleHttpResponse = await _repository.Get<Article>(articleSlug);

            if (!articleHttpResponse.IsSuccessful())
                return articleHttpResponse;

            var article = articleHttpResponse.Content as ProcessedArticle;

            if (_featureToggles.DynamicContactUsForm) _contactUsMessageParser.Parse(article, message, "");

            var viewModel = new ArticleViewModel(article);

            return View(viewModel);
        }

        [Route("/{articleSlug}/{sectionSlug}")]
        public async Task<IActionResult> ArticleWithSection(string articleSlug, string sectionSlug, [FromQuery] string message)
        {
            var articleHttpResponse = await _repository.Get<Article>(articleSlug);

            if (!articleHttpResponse.IsSuccessful())
                return articleHttpResponse;

            var article = articleHttpResponse.Content as ProcessedArticle;

            if (_featureToggles.DynamicContactUsForm) _contactUsMessageParser.Parse(article, message, sectionSlug);

            SetArticlesCanonicalUrl(articleSlug, sectionSlug, article);

            try
            {
                var viewModel = new ArticleViewModel(article, sectionSlug);
                return View("Article", viewModel);
            }
            catch (SectionDoesNotExistException)
            {
                _logger.LogWarning("Section does not exist, returning 404.");
                return NotFound();
            }
        }

        private void SetArticlesCanonicalUrl(string articleSlug, string sectionSlug, ProcessedArticle article)
        {
            if (article.Sections.Any() && article.Sections.First().Slug == sectionSlug)
            {
                ViewData["CanonicalUrl"] = "/" + articleSlug;
            }
        }
    }
}