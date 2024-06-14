namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class StartPageController : Controller
{

    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IFeatureManager _featureManager;

    public StartPageController(IProcessedContentRepository processedContnentRepository, IFeatureManager featureManager = null)
    {
        _processedContentRepository = processedContnentRepository;
        _featureManager = featureManager;
    }

    [HttpGet]
    [Route("/start/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        
        var response = await _processedContentRepository.Get<StartPage>(slug);

        if (!response.IsSuccessful()) return response;

        var startPage = response.Content as ProcessedStartPage;
        
        if(_featureManager.IsEnabledAsync("StartPages").Result) 
            return View("StartPage2024", startPage);

        return View(startPage);
    }
}
