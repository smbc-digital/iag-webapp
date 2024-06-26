namespace StockportWebapp.Controllers;

//[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ArticleController : Controller
{
    private readonly IRepository _repository;
    private readonly IProcessedContentRepository _processedRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser;
    private readonly BusinessId _businessId;
    private readonly IFeatureManager _featureManager;
    private readonly bool _isStockportGovArticle;
    private readonly bool _flatArticleToggle = true;
    private readonly bool _sectionArticleToggle = false;

    public ArticleController(IRepository repository, IProcessedContentRepository processedRepository, IContactUsMessageTagParser contactUsMessageParser, BusinessId businessId, IFeatureManager featureManager = null)
    {
        _repository = repository;
        _processedRepository = processedRepository;
        _contactUsMessageParser = contactUsMessageParser;
        _businessId = businessId;
        _featureManager = featureManager;

        _isStockportGovArticle = _businessId.ToString().Equals("stockportgov");

        if (_featureManager is not null)
        {
            _flatArticleToggle = _featureManager.IsEnabledAsync("Articles").Result;
            _sectionArticleToggle = _featureManager.IsEnabledAsync("ArticlesWithSections").Result;
        }
    }

    [Route("/{articleSlug}")]
    public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message)
    {
        HttpResponse articleHttpResponse = await _processedRepository.Get<Article>(articleSlug);
        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        ProcessedArticle article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, "");

        ArticleViewModel viewModel = new(article);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        if (ShouldReturnArticle2024(article))
            return View("Article2024", viewModel);
        else if (ShouldReturnFlatArticle2024(article))
            return View("FlatArticle2024", viewModel);
        else
            return View(viewModel);
    }

    [Route("/{articleSlug}/{sectionSlug}")]
    public async Task<IActionResult> ArticleWithSection(string articleSlug, string sectionSlug, [FromQuery] string message)
    {
        HttpResponse articleHttpResponse = await _processedRepository.Get<Article>(articleSlug);
        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        ProcessedArticle article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, sectionSlug);

        SetArticlesCanonicalUrl(articleSlug, sectionSlug, article);

        try
        {
            ArticleViewModel viewModel = new(article, sectionSlug);

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
            _sectionArticleToggle && (article.Sections?.Any() is true);

    private bool ShouldReturnFlatArticle2024(ProcessedArticle article) =>
        _isStockportGovArticle &&
            _flatArticleToggle && (article.Sections?.Any() is not true);

    private void SetArticlesCanonicalUrl(string articleSlug, string sectionSlug, ProcessedArticle article)
    {
        if (article.Sections.Any() && article.Sections.First().Slug.Equals(sectionSlug))
            ViewData["CanonicalUrl"] = $"/{articleSlug}";
    }
}