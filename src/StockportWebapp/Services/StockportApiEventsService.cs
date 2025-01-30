namespace StockportWebapp.Services;

public interface IStockportApiEventsService
{
    Task<List<EventCategory>> GetEventCategories();
    Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true);
    Task<ProcessedEvents> GetProcessedEvent(string slug, DateTime? date);
    ProcessedEvents BuildProcessedEvent(Event baseEvent);
}

public class StockportApiEventsService(IStockportApiRepository stockportApiRepository,
                                    IEventFactory eventFactory) : IStockportApiEventsService
{
    readonly IStockportApiRepository _stockportApiRepository = stockportApiRepository;
    private readonly IEventFactory _eventFactory = eventFactory;

    public async Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true) =>
        await _stockportApiRepository
            .GetResponse<List<Event>>("by-category", new List<Query> { new("category", category), new("onlyNextOccurrence", onlyNextOccurrence.ToString()) });

    public async Task<List<EventCategory>> GetEventCategories() =>
        await _stockportApiRepository.GetResponse<List<EventCategory>>();

    public async Task<ProcessedEvents> GetProcessedEvent(string slug, DateTime? date)
    {
        List<Query> queries = new();

        if (date.HasValue)
            queries.Add(new Query("date", date.Value.ToString("yyyy-MM-dd")));

        Event eventItem = await _stockportApiRepository.GetResponse<Event>(slug, queries);

        if (eventItem is null)
            return null;

        return _eventFactory.Build(eventItem);
    }

    public ProcessedEvents BuildProcessedEvent(Event baseEvent) =>
        _eventFactory.Build(baseEvent);
}