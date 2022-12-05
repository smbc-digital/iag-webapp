using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Services
{
    public interface IEventsService
    {
        Task<List<Event>> GetEventsByLimit(int limit);
        Task<Event> GetLatestEventsItem();
        Task<Event> GetLatestFeaturedEventItem();
    }

    public class EventsService : IEventsService
    {
        private readonly IRepository _eventsRepository;

        public EventsService(IRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<List<Event>> GetEventsByLimit(int limit)
        {
            var response = await _eventsRepository.GetLatest<EventCalendar>(limit);
            return response.Content as List<Event>;
        }

        public async Task<Event> GetLatestEventsItem()
        {
            var response = await _eventsRepository.GetLatest<EventCalendar>(1);
            var newsItems = response.Content as EventCalendar;
            return newsItems?.Events?.First();
        }

        public async Task<Event> GetLatestFeaturedEventItem()
        {
            var response = await _eventsRepository.GetLatestOrderByFeatured<EventCalendar>(1);
            var newsItems = response.Content as EventCalendar;
            return newsItems?.Events?.First();
        }
    }
}
