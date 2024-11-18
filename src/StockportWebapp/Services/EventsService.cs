namespace StockportWebapp.Services;

public interface IEventsService
{
    Task<List<Event>> GetEventsByLimit(int limit);
    Task<Event> GetLatestEventsItem();
    Task<Event> GetLatestFeaturedEventItem();
}

public class EventsService : IEventsService
{
    private readonly IRepository _eventsRepository;

    public EventsService(IRepository eventsRepository) =>
        _eventsRepository = eventsRepository;

    public async Task<List<Event>> GetEventsByLimit(int limit)
    {
        HttpResponse response = await _eventsRepository.GetLatest<EventCalendarViewModel>(limit);
        return response.Content as List<Event>;
    }

    public async Task<Event> GetLatestEventsItem()
    {
        HttpResponse response = await _eventsRepository.GetLatest<EventCalendarViewModel>(1);
        EventCalendarViewModel eventCalendar = response.Content as EventCalendarViewModel;
        return eventCalendar?.Events?.First();
    }

    public async Task<Event> GetLatestFeaturedEventItem()
    {
        HttpResponse response = await _eventsRepository.GetLatestOrderByFeatured<EventCalendarViewModel>(1);
        EventCalendarViewModel eventCalendar = response.Content as EventCalendarViewModel;
        return eventCalendar?.Events?.FirstOrDefault();
    }
}