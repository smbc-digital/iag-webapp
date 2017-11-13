using StockportWebapp.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using StockportWebapp.Repositories;
using StockportWebapp.Models;

namespace StockportWebapp.Services
{
    public interface IStockportApiEventsService
    {
        Task<List<EventCategory>> GetEventCategories();
        Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true);
    }

    public class StockportApiEventsService : IStockportApiEventsService
    {
        readonly IStockportApiRepository _stockportApiRepository;
        readonly IUrlGeneratorSimple _urlGeneratorSimple;

        public StockportApiEventsService(IStockportApiRepository stockportApiRepository, IUrlGeneratorSimple urlGeneratorSimple)
        {
            _stockportApiRepository = stockportApiRepository;
            _urlGeneratorSimple = urlGeneratorSimple;
        }

        public async Task<List<Event>> GetEventsByCategory(string category, bool onlyNextOccurrence = true)
        {
            return await _stockportApiRepository.GetResponse<List<Event>>("by-category", new List<Query> { new Query("category", category), new Query("onlyNextOccurrence", onlyNextOccurrence.ToString() ) });
        }

        public async Task<List<EventCategory>> GetEventCategories()
        {
            return await _stockportApiRepository.GetResponse<List<EventCategory>>();
        }
    }
}
