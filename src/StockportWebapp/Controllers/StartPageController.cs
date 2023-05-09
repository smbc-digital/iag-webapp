namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class StartPageController : Controller
{

    private readonly IProcessedContentRepository _processedContentRepository;

    public StartPageController(IProcessedContentRepository processedContnentRepository)
    {
        _processedContentRepository = processedContnentRepository;
    }

    [HttpGet]
    [Route("/start/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var response = await _processedContentRepository.Get<StartPage>(slug);

        if (!response.IsSuccessful()) return response;

        var startPage = response.Content as ProcessedStartPage;

        return View(startPage);
    }
}
