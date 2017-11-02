using StockportWebapp.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using StockportWebapp.Repositories;
using StockportWebapp.Models;

namespace StockportWebapp.Services
{
    public interface IStockportApiEventsService
    {
        Task<List<Event>> GetEventsByCategory(string category);
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

        public async Task<List<Event>> GetEventsByCategory(string category)
        {
            // TODO: Change the name of without-categories to something that makes more sense...
            return await _stockportApiRepository.GetResponse<List<Event>>("without-categories", new List<Query> { new Query("category", category) });
        }
    }
}
