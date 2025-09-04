namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ArticleController(IRepository repository,
                            IProcessedContentRepository processedRepository,
                            IContactUsMessageTagParser contactUsMessageParser,
                            IFeatureManager featureManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IProcessedContentRepository _processedRepository = processedRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser = contactUsMessageParser;
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/{articleSlug}")]
    public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message)
    {
        await DisableResponseCaching();

        HttpResponse articleHttpResponse = await _processedRepository.Get<Article>(articleSlug);
        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        ProcessedArticle article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, string.Empty);

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(new ArticleViewModel(article));
    }

    [Route("/{articleSlug}/{sectionSlug}")]
    public async Task<IActionResult> ArticleWithSection(string articleSlug, string sectionSlug, [FromQuery] string message)
    {
        await DisableResponseCaching();

        HttpResponse articleHttpResponse = await _processedRepository.Get<Article>(articleSlug);
        if (!articleHttpResponse.IsSuccessful())
            return articleHttpResponse;

        ProcessedArticle article = articleHttpResponse.Content as ProcessedArticle;

        _contactUsMessageParser.Parse(article, message, sectionSlug);

        SetArticlesCanonicalUrl(articleSlug, sectionSlug, article);

        try
        {
            return View("Article", new ArticleViewModel(article, sectionSlug));
        }
        catch (SectionDoesNotExistException)
        {
            return NotFound();
        }
    }

    private async Task DisableResponseCaching()
    {
        if (await _featureManager.IsEnabledAsync("DisableResponseCaching"))
        {
            Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "no-store, no-cache, must-revalidate";
            Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Pragma] = "no-cache";
            Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Expires] = "-1";
        }
    }

    private void SetArticlesCanonicalUrl(string articleSlug, string sectionSlug, ProcessedArticle article)
    {
        if (article.Sections.Any() && article.Sections.First().Slug.Equals(sectionSlug))
            ViewData["CanonicalUrl"] = $"/{articleSlug}";
    }
}