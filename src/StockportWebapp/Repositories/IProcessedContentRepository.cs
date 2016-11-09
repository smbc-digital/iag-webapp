using System.Threading.Tasks;
using StockportWebapp.Http;

namespace StockportWebapp.Repositories
{
    public interface IProcessedContentRepository
    {
        Task<HttpResponse> Get<T>(string slug = "");
    }
}