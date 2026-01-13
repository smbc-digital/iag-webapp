namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
[Route("publications/{publicationSlug}")]
public class PublicationTemplateController(IPublicationTemplateRepository repository) : Controller
{
    private readonly IPublicationTemplateRepository _repository = repository;

    [HttpGet("")]
    [HttpGet("{pageSlug}")]
    [HttpGet("{pageSlug}/{sectionSlug}")]
    public async Task<IActionResult> Index(string publicationSlug, string? pageSlug, string? sectionSlug)
    {
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

        PublicationSection? publicationSection = sectionSlug is null
            ? publicationPage.PublicationSections?.FirstOrDefault()
            : publicationPage.PublicationSections?.FirstOrDefault(section => section.Slug.Equals(sectionSlug, StringComparison.OrdinalIgnoreCase));

        return View("Index", new PublicationTemplateViewModel(publicationTemplate, publicationPage, publicationSection));
    }
}