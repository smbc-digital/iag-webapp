namespace StockportWebapp.Services;

public interface IEventsService
{
    Task<List<Event>> GetEventsByLimit(int limit);
    Task<Event> GetLatestEventsItem();
    Task<Event> GetLatestFeaturedEventItem();
    Task<List<Event>> GetLatestFeaturedEvents();
}

public class EventsService(IRepository eventsRepository) : IEventsService
{
    private readonly IRepository _eventsRepository = eventsRepository;

    public async Task<List<Event>> GetEventsByLimit(int limit)
    {
        HttpResponse response = await _eventsRepository.GetLatest<EventCalendar>(limit);

        return response.Content as List<Event>;
    }

    public async Task<Event> GetLatestEventsItem()
    {
        HttpResponse response = await _eventsRepository.GetLatest<EventCalendar>(1);
        EventCalendar eventCalendar = response.Content as EventCalendar;

        return eventCalendar?.Events?.First();
    }

    public async Task<Event> GetLatestFeaturedEventItem()
    {
        HttpResponse response = await _eventsRepository.GetLatestOrderByFeatured<EventCalendar>(1);
        EventCalendar eventCalendar = response.Content as EventCalendar;
        
        return eventCalendar?.Events?.FirstOrDefault();
    }

    public async Task<List<Event>> GetLatestFeaturedEvents()
    {
        HttpResponse response = await _eventsRepository.GetLatestOrderByFeatured<EventCalendar>(6);
        EventCalendar eventCalendar = response.Content as EventCalendar;

        return eventCalendar?.FeaturedEvents;
    }
}