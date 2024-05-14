namespace StockportWebapp.Controllers;

//[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ArticleController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IArticleRepository _articlerepository;
    private readonly ILogger<ArticleController> _logger;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;
    private readonly IFeatureManager _featureManager;
    private readonly bool _isToggledOn = true;

    public ArticleController(IProcessedContentRepository repository, ILogger<ArticleController> logger, IContactUsMessageTagParser contactUsMessageParser, IArticleRepository articlerepository, IFeatureManager featureManager = null)
    {
        _repository = repository;
        _logger = logger;
        _contactUsMessageParser = contactUsMessageParser;
        _articlerepository = articlerepository;
        _featureManager = featureManager;

        if (_featureManager is not null)
            _isToggledOn = _featureManager.IsEnabledAsync("Articles").Result;
    }

    [Route("/{articleSlug}")]
    public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message, string SearchTerm, string SearchFolder)
    {        
        var articleHttpResponse = await _articlerepository.Get(articleSlug, SearchTerm, SearchFolder, Request?.GetDisplayUrl().ToString());

        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        var article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, "");

        var viewModel = new ArticleViewModel(article);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        if (!_isToggledOn)
            return View(viewModel);
        else
            return View("Article2024", viewModel);
    }

    [Route("/{articleSlug}/{sectionSlug}")]
    public async Task<IActionResult> ArticleWithSection(string articleSlug, string sectionSlug, [FromQuery] string message, string SearchTerm, string SearchFolder)
    {
        var articleHttpResponse = await _articlerepository.Get(articleSlug, SearchTerm, SearchFolder, Request?.GetDisplayUrl().ToString());

        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        var article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, sectionSlug);

        SetArticlesCanonicalUrl(articleSlug, sectionSlug, article);

        try
        {
            var viewModel = new ArticleViewModel(article, sectionSlug);
            if (!_isToggledOn)
                return View("Article", viewModel);
            else
                return View("Article2024", viewModel);
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