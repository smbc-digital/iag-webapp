namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class LandingPageController(IRepository repository) : Controller
{
    private readonly IRepository _repository = repository;

    [Route("/landing/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        HttpResponse response = await _repository.Get<LandingPage>(slug);

        if (!response.IsSuccessful())
            return response;

        LandingPage landingPage = response.Content as LandingPage;

        return View(new LandingPageViewModel(landingPage));
    }
}