namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
[Route("publications/{publicationSlug}")]
public class PublicationTemplateController(IPublicationTemplateRepository repository, IFeatureManager featureManager) : Controller
{
    private readonly IPublicationTemplateRepository _repository = repository;
    private readonly IFeatureManager _featureManager = featureManager;

    [HttpGet("")]
    [HttpGet("{pageSlug}")]
    [HttpGet("{pageSlug}/{sectionSlug}")]
    public async Task<IActionResult> Index(string publicationSlug, string? pageSlug, string? sectionSlug)
    {
        if (!await _featureManager.IsEnabledAsync("PublicationTemplate"))
            return NotFound();
        
        HttpResponse response = await _repository.Get(publicationSlug);

        if (!response.IsSuccessful())
            return response;

        if (response.Content is not PublicationTemplate publicationTemplate)
            return NotFound();

        PublicationPage publicationPage = pageSlug is null
            ? publicationTemplate.PublicationPages.FirstOrDefault()
            : publicationTemplate.PublicationPages.FirstOrDefault(page => page.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));

        if (publicationPage is null)
            return NotFound();

        SetPublicationCanonicalUrl(publicationSlug, pageSlug, sectionSlug, publicationTemplate);

        PublicationSection? publicationSection = sectionSlug is null
            ? publicationPage.PublicationSections?.FirstOrDefault()
            : publicationPage.PublicationSections?.FirstOrDefault(section => section.Slug.Equals(sectionSlug, StringComparison.OrdinalIgnoreCase));

        return View("Index", new PublicationTemplateViewModel(publicationTemplate, publicationPage, publicationSection));
    }

    private void SetPublicationCanonicalUrl(string publicationSlug, string? pageSlug, string? sectionSlug, PublicationTemplate template)
    {
        if (!template.PublicationPages.Any() || string.IsNullOrEmpty(pageSlug))
            return;

        PublicationPage currentPage = template.PublicationPages.FirstOrDefault(page => page.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase));

        if (currentPage is null)
            return;

        PublicationPage firstPage = template.PublicationPages.First();
        if (firstPage.Slug.Equals(pageSlug, StringComparison.OrdinalIgnoreCase))
        {
            ViewData["CanonicalUrl"] = $"/publications/{publicationSlug}";
            return;
        }

        if (!string.IsNullOrEmpty(sectionSlug) &&
            currentPage.PublicationSections?.Any() is true &&
            currentPage.PublicationSections.First().Slug.Equals(sectionSlug, StringComparison.OrdinalIgnoreCase))
        {
            ViewData["CanonicalUrl"] = $"/publications/{publicationSlug}/{pageSlug}";
        }
    }
}