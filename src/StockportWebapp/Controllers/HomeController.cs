namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class HomeController(BusinessId businessId,
                            IApplicationConfiguration applicationConfiguration,
                            INewsService newsService, IEventsService eventsService,
                            IHomepageService homepageService,
                            IStockportApiEventsService stockportApiService) : Controller
{
    private readonly BusinessId _businessId = businessId;
    private readonly IApplicationConfiguration _config = applicationConfiguration;
    private readonly INewsService _newsService = newsService;
    private readonly IEventsService _eventsService = eventsService;
    private readonly IHomepageService _homepageService = homepageService;
    private readonly IStockportApiEventsService _stockportApiEventsService = stockportApiService;

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        ProcessedHomepage homepage = await _homepageService.GetHomepage();

        if (homepage is null)
            return new NotFoundResult();

        Task<List<Event>> getFeaturedEvents = _eventsService.GetLatestFeaturedEvents();
        Task<Event> getEventsTask = _eventsService.GetLatestFeaturedEventItem();
        Task<News> getNewsTask = _newsService.GetLatestNewsItem();

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

        return View(new HomepageViewModel
            {
                HomepageContent = homepage,
                FeaturedEvent = getEventsTask.Result,
                FeaturedNews = getNewsTask.Result,
                FeaturedEvents = getFeaturedEvents.Result,
                EventsFromApi = eventsByCategoryTask is not null
                    ? eventsByCategoryTask.Result?.Take(3).ToList()
                    : new List<Event>()
            });
    }

    [Route("/subscribe")]
    public async Task<IActionResult> EmailSubscribe(string emailAddress, string emailAlertsTopicId, string mailingListId)
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
        else if (!string.IsNullOrEmpty(mailingListId))
        {
            urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());
            redirectUrl += string.Concat(urlSetting, $"?topic_id={mailingListId}");
        }

        if (urlSetting is null || !urlSetting.IsValid())
            return NotFound();

        return await Task.FromResult(Redirect(redirectUrl));
    }
}