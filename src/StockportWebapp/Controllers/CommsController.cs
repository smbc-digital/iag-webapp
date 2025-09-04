namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class CommsController(IRepository repository, ILogger<CommsController> logger) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<CommsController> _logger = logger;

    [Route("/news-room")]
    public async Task<IActionResult> Index()
    {
        HttpResponse response = await _repository.Get<CommsHomepage>(queries: new List<Query>());
        CommsHomepage commsHomepage = response.Content as CommsHomepage;
        HttpResponse latestNewsResponse = await _repository.GetLatest<List<News>>(1);
        List<News> latestNews = latestNewsResponse.Content as List<News>;

        CommsHomepageViewModel viewModel = new()
        {
            Homepage = commsHomepage,
            LatestNews = latestNews?.FirstOrDefault()
        };

        try
        {
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"News and media unable to load {ex.Message}");

            return RedirectToRoute("Error", "500");
        }
    }
}