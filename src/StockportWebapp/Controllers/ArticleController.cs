using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Exceptions;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using StockportWebapp.Http;
using System.Linq;
using StockportWebapp.ProcessedModels;
using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace StockportWebapp.Controllers
{
    //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class ArticleController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IArticleRepository _articlerepository;
        private readonly ILogger<ArticleController> _logger;
        private readonly IContactUsMessageTagParser _contactUsMessageParser;

        public ArticleController(IProcessedContentRepository repository, ILogger<ArticleController> logger, IContactUsMessageTagParser contactUsMessageParser, IArticleRepository articlerepository)
        {
            _repository = repository;
            _logger = logger;
            _contactUsMessageParser = contactUsMessageParser;
            _articlerepository = articlerepository;
        }

        [Route("/map")]
        public IActionResult Map([FromQuery] string source, [FromQuery] string panels, [FromQuery] string layers)
        {
            return View(new Tuple<string, string, string>(source, panels, layers));
        }

        [Route("/{articleSlug}")]
        public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message, string SearchTerm, string SearchFolder)
        {
            var articleHttpResponse = await _articlerepository.Get(articleSlug, SearchTerm, SearchFolder, Request?.GetUri().ToString());

            if (!articleHttpResponse.IsSuccessful())
                return articleHttpResponse;

            var article = articleHttpResponse.Content as ProcessedArticle;

            _contactUsMessageParser.Parse(article, message, "");
            
            var viewModel = new ArticleViewModel(article);

            ViewBag.CurrentUrl = Request?.GetUri();


            return View(viewModel);
        }

        [Route("/{articleSlug}/{sectionSlug}")]
        public async Task<IActionResult> ArticleWithSection(string articleSlug, string sectionSlug, [FromQuery] string message, string SearchTerm, string SearchFolder)
        {
            var articleHttpResponse = await _articlerepository.Get(articleSlug, SearchTerm, SearchFolder, Request?.GetUri().ToString());

            if (!articleHttpResponse.IsSuccessful())
                return articleHttpResponse;

            var article = articleHttpResponse.Content as ProcessedArticle;

            _contactUsMessageParser.Parse(article, message, sectionSlug);

            SetArticlesCanonicalUrl(articleSlug, sectionSlug, article);

            try
            {
                var viewModel = new ArticleViewModel(article, sectionSlug);
                return View("Article", viewModel);
            }
            catch (SectionDoesNotExistException)
            {
                
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