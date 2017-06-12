using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.ContentFactory;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public class ProcessedContentRepository : IProcessedContentRepository
    {
        private readonly ContentTypeFactory _contentTypeFactory;
        private readonly IHttpClient _httpClient;
        private readonly IStubToUrlConverter _urlGenerator;

        public ProcessedContentRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, ContentTypeFactory contentTypeFactory)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
            _contentTypeFactory = contentTypeFactory;
        }

        public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            var url = _urlGenerator.UrlFor<T>(slug, queries);
            var httpResponse = await _httpClient.Get(url);

            if (!httpResponse.IsSuccessful())
            {
                return httpResponse;
            }

            var model = HttpResponse.Build<T>(httpResponse);
            var processedModel = _contentTypeFactory.Build((T)model.Content);

            return HttpResponse.Successful(200, processedModel);
        }

        public async Task<HttpResponse> Delete<T>(string slug)
        {
            // TODO - Replace this with the actual delete functionality

            var url = _urlGenerator.UrlFor<T>(slug);
            var httpResponse = await _httpClient.Get(url);

            if (!httpResponse.IsSuccessful())
            {
                return httpResponse;
            }

            var model = HttpResponse.Build<T>(httpResponse);
            var processedModel = _contentTypeFactory.Build((T)model.Content);

            return HttpResponse.Successful(200, processedModel);
        }
    }
}