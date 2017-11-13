using StockportWebapp.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using StockportWebapp.ContentFactory;
using StockportWebapp.Repositories;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Services
{
    public interface IStockportApiEventsService
    {
        Task<List<EventCategory>> GetEventCategories();
        Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true);
        Task<ProcessedEvents> GetProcessedEvent(string slug);
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
            return await _stockportApiRepository.GetResponse<List<Event>>("by-category", new List<Query> { new Query("category", category), new Query("onlyNextOccurrence", onlyNextOccurrence.ToString() ) });
        }

        public async Task<List<EventCategory>> GetEventCategories()
        {
            return await _stockportApiRepository.GetResponse<List<EventCategory>>();
        }

        public async Task<ProcessedEvents> GetProcessedEvent(string slug)
        {
            var eventItem = await _stockportApiRepository.GetResponse<Event>(slug);
            var processedEvent = _eventFactory.Build(eventItem);
            return processedEvent;
        }
    }
}
