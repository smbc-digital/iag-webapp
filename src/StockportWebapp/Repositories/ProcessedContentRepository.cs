using System.Collections.Generic;
using System.Net.Http;
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
        private readonly Dictionary<string, string> authenticationHeaders;

        public ProcessedContentRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, ContentTypeFactory contentTypeFactory, IApplicationConfiguration config)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
            _contentTypeFactory = contentTypeFactory;
            _config = config;
            authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            //var testClientHandeler = new HttpClientHandler { Proxy = null };
            //var testClient = new System.Net.Http.HttpClient(testClientHandeler);
            var url = _urlGenerator.UrlFor<T>(slug, queries);
            var httpResponse = await _httpClient.Get(url, authenticationHeaders);

            if (!httpResponse.IsSuccessful()) {
                return httpResponse;
            }

            var model = HttpResponse.Build<T>(httpResponse);
            var processedModel = _contentTypeFactory.Build((T)model.Content);

            return HttpResponse.Successful(200, processedModel);

        }
    }
}