using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<HttpResponse> Get(string slug = "", List<Query> queries = null);
    }

    public class GenericRepository<T> : IGenericRepository<T>
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly UrlGenerator _urlGenerator;
        private readonly Dictionary<string, string> _authenticationHeaders;

        public GenericRepository(IHttpClient httpClient, IApplicationConfiguration config, UrlGenerator urlGenerator)
        {
            _httpClient = httpClient;
            _config = config;
            _urlGenerator = urlGenerator;
            _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<HttpResponse> Get(string slug = "", List<Query> queries = null)
        {
            var url = _urlGenerator.UrlFor<T>(slug, queries);
            return await _httpClient.Get(url, _authenticationHeaders);
        }
    }
}
