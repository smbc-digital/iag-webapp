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
        readonly IStockportApiRepository _stockportApiGateway;
        readonly IUrlGeneratorSimple _urlGeneratorSimple;

        public StockportApiEventsService(IStockportApiRepository stockportApiGateway, IUrlGeneratorSimple urlGeneratorSimple)
        {
            _stockportApiGateway = stockportApiGateway;
            _urlGeneratorSimple = urlGeneratorSimple;
        }

        public async Task<List<Event>> GetEventsByCategory(string category)
        {
            // TODO: Change the name of without-categories to something that makes more sense...
            return await _stockportApiGateway.GetResponse<List<Event>>("without-categories", new List<Query> { new Query("category", category) });
        }
    }
}
