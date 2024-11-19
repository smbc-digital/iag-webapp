namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class TopicController : Controller
{
    private readonly IApplicationConfiguration _config;
    private readonly BusinessId _businessId;
    private readonly ITopicRepository _topicRepository;
    private readonly IStockportApiEventsService _stockportApiEventsService;

    public TopicController(ITopicRepository repository, IApplicationConfiguration config, BusinessId businessId, IStockportApiEventsService stockportApiService)
    {
        _config = config;
        _businessId = businessId;
        _topicRepository = repository;
        _stockportApiEventsService = stockportApiService;
    }

    [Route("/topic/{topicSlug}")]
    public async Task<IActionResult> Index(string topicSlug)
    {
        HttpResponse topicHttpResponse = await _topicRepository.Get<ProcessedTopic>(topicSlug);

        if (!topicHttpResponse.IsSuccessful())
            return topicHttpResponse;

        ProcessedTopic processedTopic = topicHttpResponse.Content as ProcessedTopic;

        AppSetting urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

        TopicViewModel topicViewModel = new(processedTopic, urlSetting.ToString());

        List<Event> eventsFromApi = !string.IsNullOrEmpty(processedTopic.EventCategory)
            ? await _stockportApiEventsService.GetEventsByCategory(processedTopic.EventCategory) 
            : new();
            
        topicViewModel.EventsFromApi = eventsFromApi?.Take(3).ToList();

        return View(topicViewModel);
    }
}