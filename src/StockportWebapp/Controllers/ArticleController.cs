namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ArticleController(IRepository repository,
                            IProcessedContentRepository processedRepository,
                            IContactUsMessageTagParser contactUsMessageParser) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IProcessedContentRepository _processedRepository = processedRepository;
    private readonly IContactUsMessageTagParser _contactUsMessageParser = contactUsMessageParser;

    [Route("/{articleSlug}")]
    public async Task<IActionResult> Article(string articleSlug, [FromQuery] string message)
    {        
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

    private void SetArticlesCanonicalUrl(string articleSlug, string sectionSlug, ProcessedArticle article)
    {
        if (article.Sections.Any() && article.Sections.First().Slug.Equals(sectionSlug))
            ViewData["CanonicalUrl"] = $"/{articleSlug}";
    }
}