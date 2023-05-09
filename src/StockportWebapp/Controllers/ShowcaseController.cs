namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ShowcaseController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly IApplicationConfiguration _config;

    public ShowcaseController(
        IProcessedContentRepository repository,
        IApplicationConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    [Route("/showcase/{slug}")]
    public async Task<IActionResult> Showcase(string slug)
    {
        var response = await _repository.Get<Showcase>(slug);

        if (!response.IsSuccessful())
            return response;

        var showcase = response.Content as ProcessedShowcase;

        return View("Showcase", showcase);
    }
}