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

    public HomeController(BusinessId businessId, IApplicationConfiguration applicationConfiguration, INewsService newsService, IEventsService eventsService, IHomepageService homepageService, IStockportApiEventsService stockportApiService)
    {
        _config = applicationConfiguration;
        _businessId = businessId;
        _newsService = newsService;
        _eventsService = eventsService;
        _homepageService = homepageService;
        _stockportApiEventsService = stockportApiService;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        var homepage = await _homepageService.GetHomepage();

        if (homepage == null) return new NotFoundResult();

        var getEventsTask = _eventsService.GetLatestFeaturedEventItem();
        var getNewsTask = _newsService.GetLatestNewsItem();

        List<Task> tasks = new()
        {
            getEventsTask,
            getNewsTask
        };

        Task<List<Event>> eventsByCategoryTask = null;

        if (!string.IsNullOrEmpty(homepage.EventCategory))
        {
            eventsByCategoryTask = _stockportApiEventsService.GetEventsByCategory(homepage.EventCategory);
            tasks.Add(eventsByCategoryTask);
        }

        Task.WaitAll(tasks.ToArray());

        var homepageViewModel = new HomepageViewModel
        {
            HomepageContent = homepage,
            FeaturedEvent = getEventsTask.Result,
            FeaturedNews = getNewsTask.Result,
            EventsFromApi = eventsByCategoryTask != null ? eventsByCategoryTask.Result?.Take(3).ToList() : new List<Event>()
        };

        return View(homepageViewModel);
    }

    // I don't think this is used anymore, but need to double check with JH!!
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