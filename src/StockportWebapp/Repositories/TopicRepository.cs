using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.ContentFactory;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
        public interface ITopicRepository
        {
            Task<HttpResponse> Get<T>(string slug = "");
        }

        public class TopicRepository :ITopicRepository
        {
            private readonly TopicFactory _topicFactory;
            private readonly UrlGenerator _urlGenerator;
            private readonly IUrlGeneratorSimple _urlGeneratorSimple;
            private readonly IHttpClient _httpClient;
            private readonly IApplicationConfiguration _config;
            private Dictionary<string, string> _authenticationHeaders;

            public TopicRepository(TopicFactory topicFactory, UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple)
            {
                _topicFactory = topicFactory;
                _urlGenerator = urlGenerator;
                _httpClient = httpClient;
                _config = config;
                _urlGeneratorSimple = urlGeneratorSimple;
                _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
            }

            public async Task<HttpResponse> Get<T>(string slug = "")
            {
                var url = _urlGenerator.UrlFor<Topic>(slug);
                var httpResponse = await _httpClient.Get(url, _authenticationHeaders);

                if (!httpResponse.IsSuccessful())
                {
                    return httpResponse;
                }

                var model = HttpResponse.Build<Topic>(httpResponse);
                var topic = (Topic)model.Content;

                var processedModel = _topicFactory.Build(topic);
                return HttpResponse.Successful(200, processedModel);
            }
        }
}
