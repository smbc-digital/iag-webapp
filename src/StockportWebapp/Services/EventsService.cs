using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Services
{
    public interface IEventsService
    {
        Task<List<Event>> GetEventsByLimit(int limit);
        Task<Event> GetLatestEventsItem();
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
    }
}
