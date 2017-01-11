using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebappTests.Unit.Fake
{
    public class FakeProcessedContentRepository : IProcessedContentRepository
    {
        private HttpResponse _response;

        public Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            return Task.FromResult(_response);
        }

        public void Set(HttpResponse response)
        {
            _response = response;
        }
    }
}
