namespace StockportWebapp.Controllers;

//[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ArticleController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IArticleRepository _articlerepository;
    private readonly ILogger<ArticleController> _logger;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;
    private readonly BusinessId _businessId;
    private readonly IFeatureManager _featureManager;
    private readonly bool _flatArticleToggle = true;
    private readonly bool _sectionArticleToggle = false;
    private readonly bool _isStockportGovArticle = true;

    public ArticleController(IProcessedContentRepository repository, ILogger<ArticleController> logger, IContactUsMessageTagParser contactUsMessageParser, IArticleRepository articlerepository, BusinessId businessId, IFeatureManager featureManager = null)
    {
        _repository = repository;
        _logger = logger;
        _contactUsMessageParser = contactUsMessageParser;
        _articlerepository = articlerepository;
        _businessId = businessId;
        _featureManager = featureManager;

        _isStockportGovArticle = _businessId.ToString().Equals("stockportgov");

        if (_featureManager is not null){
            _flatArticleToggle = _featureManager.IsEnabledAsync("Articles").Result;
            _sectionArticleToggle = _featureManager.IsEnabledAsync("ArticlesWithSections").Result;
        }
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

        if (ShouldReturnArticle2024(article))
            return View("Article2024", viewModel);
        else
            return View(viewModel);
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
            if (ShouldReturnArticle2024(article))
                return View("Article2024", viewModel);
            else
                return View("Article", viewModel);
        }
        catch (SectionDoesNotExistException)
        {
            return NotFound();
        }
    }

    private bool ShouldReturnArticle2024(ProcessedArticle article) => 
        _isStockportGovArticle &&
            ((_sectionArticleToggle && article.Sections?.Any() is true) ||
                (_flatArticleToggle && (article.Sections?.Any() is not true)));

    private void SetArticlesCanonicalUrl(string articleSlug, string sectionSlug, ProcessedArticle article)
    {
        if (article.Sections.Any() && article.Sections.First().Slug == sectionSlug)
            ViewData["CanonicalUrl"] = "/" + articleSlug;
    }
}