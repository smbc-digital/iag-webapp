using StockportWebapp.Models;

namespace StockportWebapp.Repositories
{
    public interface IProcessedContentRepository
    {
        Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null);
    }
}