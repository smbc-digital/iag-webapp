namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class LandingPageController : Controller
{
    private readonly IRepository _repository;
    private readonly IFeatureManager _featureManager;

    public LandingPageController(IRepository repository, IFeatureManager featureManager = null)
    {
        _repository = repository;
        _featureManager = featureManager;
    } 

    [Route("/landing/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        if(_featureManager is not null && !_featureManager.IsEnabledAsync("LandingPages").Result)
            return NotFound();
        
        HttpResponse response = await _repository.Get<LandingPage>(slug);

        if (!response.IsSuccessful())
            return response;

        LandingPage landingPage = response.Content as LandingPage;

        return View(new LandingPageViewModel(landingPage));
    }
}