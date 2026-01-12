namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class PublicationTemplateController(IPublicationTemplateRepository repository) : Controller
{
    private readonly IPublicationTemplateRepository _repository = repository;

    [Route("/publications/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        HttpResponse response = await _repository.Get(slug);

        if (!response.IsSuccessful())
            return response;

        PublicationTemplate publicationTemplate = response.Content as PublicationTemplate;

        return View(new PublicationTemplateViewModel(publicationTemplate));
    }
}