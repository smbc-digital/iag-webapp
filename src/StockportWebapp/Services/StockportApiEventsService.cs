namespace StockportWebapp.Services;

public interface IStockportApiEventsService
{
    Task<List<EventCategory>> GetEventCategories();
    Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true);
    Task<ProcessedEvents> GetProcessedEvent(string slug, DateTime? date);
}

public class StockportApiEventsService : IStockportApiEventsService
{
    readonly IStockportApiRepository _stockportApiRepository;
    readonly IUrlGeneratorSimple _urlGeneratorSimple;
    private readonly IEventFactory _eventFactory;

    public StockportApiEventsService(IStockportApiRepository stockportApiRepository, IUrlGeneratorSimple urlGeneratorSimple, IEventFactory eventFactory)
    {
        _stockportApiRepository = stockportApiRepository;
        _urlGeneratorSimple = urlGeneratorSimple;
        _eventFactory = eventFactory;
    }

    public async Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true)
    {
        return await _stockportApiRepository.GetResponse<List<Event>>("by-category", new List<Query> { new Query("category", category), new Query("onlyNextOccurrence", onlyNextOccurrence.ToString()) });
    }

    public async Task<List<EventCategory>> GetEventCategories()
    {
        return await _stockportApiRepository.GetResponse<List<EventCategory>>();
    }

    public async Task<ProcessedEvents> GetProcessedEvent(string slug, DateTime? date)
    {
        var queries = new List<Query>();
        if (date.HasValue) queries.Add(new Query("date", date.Value.ToString("yyyy-MM-dd")));

        var eventItem = await _stockportApiRepository.GetResponse<Event>(slug, queries);

        if (eventItem == null) return null;

        var processedEvent = _eventFactory.Build(eventItem);
        return processedEvent;
    }
}
