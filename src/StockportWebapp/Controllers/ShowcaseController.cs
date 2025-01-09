namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ShowcaseController(IProcessedContentRepository repository) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;

    [Route("/showcase/{slug}")]
    public async Task<IActionResult> Showcase(string slug)
    {
        HttpResponse response = await _repository.Get<Showcase>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedShowcase showcase = response.Content as ProcessedShowcase;

        return View("Showcase", showcase);
    }
}