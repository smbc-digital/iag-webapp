namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class TopicController(ITopicRepository repository,
                            IApplicationConfiguration config,
                            BusinessId businessId,
                            IStockportApiEventsService stockportApiService) : Controller
{
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;
    private readonly ITopicRepository _topicRepository = repository;
    private readonly IStockportApiEventsService _stockportApiEventsService = stockportApiService;

    [Route("/topic/{topicSlug}")]
    public async Task<IActionResult> Index(string topicSlug)
    {
        HttpResponse topicHttpResponse = await _topicRepository.Get<ProcessedTopic>(topicSlug);

        if (!topicHttpResponse.IsSuccessful())
            return topicHttpResponse;

        ProcessedTopic processedTopic = topicHttpResponse.Content as ProcessedTopic;

        TopicViewModel topicViewModel = new(processedTopic);

        List<Event> eventsFromApi = !string.IsNullOrEmpty(processedTopic.EventCategory)
            ? await _stockportApiEventsService.GetEventsByCategory(processedTopic.EventCategory) 
            : new();
            
        topicViewModel.EventsFromApi = eventsFromApi?.Take(3).ToList();

        return View(topicViewModel);
    }
}