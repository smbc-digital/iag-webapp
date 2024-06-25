namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class StartPageController : Controller
{
    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IFeatureManager _featureManager;

    public StartPageController(IProcessedContentRepository processedContentRepository, IFeatureManager featureManager = null)
    {
        _processedContentRepository = processedContentRepository;
        _featureManager = featureManager;
    }

    [HttpGet]
    [Route("/start/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var response = await _processedContentRepository.Get<StartPage>(slug);

        if (!response.IsSuccessful()) 
            return response;

        var startPage = response.Content as ProcessedStartPage;
        
        return _featureManager.IsEnabledAsync("StartPages").Result
            ? View("StartPage2024", startPage)
            : View(startPage);
    }
}