using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebappTests_Integration.Fake
{
    public class FakeProcessedContentRepository : IProcessedContentRepository
    {
        private HttpResponse _response;

        public Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            return Task.FromResult(_response);
        }

        public Task<HttpResponse> Delete<T>(string slug = "")
        {
            // TODO - Get working when SDK work complete
            return Task.FromResult(_response);
        }

        public void Set(HttpResponse response)
        {
            _response = response;
        }
        
        public Task<HttpResponse> Archive<T>(string slug = "")
        {
            return Task.FromResult(_response);
        }
    }
}
