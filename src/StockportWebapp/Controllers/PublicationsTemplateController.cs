namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class PublicationsTemplateController(IPublicationsTemplateRepository repository) : Controller
{
    private readonly IPublicationsTemplateRepository _repository = repository;

    [Route("/publications/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        HttpResponse response = await _repository.Get(slug);

        if (!response.IsSuccessful())
            return response;

        PublicationsTemplate publicationsTemplate = response.Content as PublicationsTemplate;

        return View(new PublicationsTemplateViewModel(publicationsTemplate));
    }
}