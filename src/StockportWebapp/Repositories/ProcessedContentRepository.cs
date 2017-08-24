using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
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
        private readonly IApplicationConfiguration _config;
        private Dictionary<string, string> authenticationKey;

        public ProcessedContentRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, ContentTypeFactory contentTypeFactory, IApplicationConfiguration config)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
            _contentTypeFactory = contentTypeFactory;
            _config = config;
            authenticationKey = new Dictionary<string, string> { { "AuthenticationKey", _config.GetContentApiAuthenticationKey() } };
        }

        public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            var url = _urlGenerator.UrlFor<T>(slug, queries);
            var httpResponse = await _httpClient.Get(url, authenticationKey);

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