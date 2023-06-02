namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class HomeController : Controller
{
    private readonly BusinessId _businessId;
    private readonly IApplicationConfiguration _config;
    private readonly INewsService _newsService;
    private readonly IEventsService _eventsService;
    private readonly IHomepageService _homepageService;
    private readonly IStockportApiEventsService _stockportApiEventsService;
    private readonly IFeatureManager _featureManager;

    public HomeController(BusinessId businessId, IApplicationConfiguration applicationConfiguration, INewsService newsService, IEventsService eventsService, IHomepageService homepageService, IStockportApiEventsService stockportApiService, IFeatureManager featureManager)
    {
        _config = applicationConfiguration;
        _businessId = businessId;
        _newsService = newsService;
        _eventsService = eventsService;
        _homepageService = homepageService;
        _stockportApiEventsService = stockportApiService;
        _featureManager = featureManager;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        var homepage = await _homepageService.GetHomepage();

        if (homepage == null) return new NotFoundResult();

        var eventsFromApi = !string.IsNullOrEmpty(homepage.EventCategory) ? await _stockportApiEventsService.GetEventsByCategory(homepage.EventCategory) : new List<Event>();

        var homepageViewModel = new HomepageViewModel
        {
            HomepageContent = homepage,
            FeaturedEvent = _businessId.ToString() != "healthystockport" ? await _eventsService.GetLatestFeaturedEventItem() : null,
            FeaturedNews = _businessId.ToString() != "healthystockport" ? await _newsService.GetLatestNewsItem() : null,
            EventsFromApi = eventsFromApi?.Take(3).ToList()
        };

        if(await _featureManager.IsEnabledAsync("SiteRedesign"))
            return View("Index2023", homepageViewModel);

        return View(homepageViewModel);
    }

    [Route("/subscribe")]
    public async Task<IActionResult> EmailSubscribe(string emailAddress, string emailAlertsTopicId)
    {
        AppSetting urlSetting = null;
        string redirectUrl = null;

        if (!string.IsNullOrEmpty(emailAddress))
        {
            urlSetting = _config.GetEmailAlertsUrl(_businessId.ToString());
            redirectUrl = string.Concat(urlSetting, $"?email={emailAddress}");
        }
        else if (!string.IsNullOrEmpty(emailAlertsTopicId))
        {
            urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());
            redirectUrl += string.Concat(urlSetting, $"?topic_id={emailAlertsTopicId}");
        }

        if (urlSetting is null || !urlSetting.IsValid())
            return NotFound();

        return await Task.FromResult(Redirect(redirectUrl));
    }
}