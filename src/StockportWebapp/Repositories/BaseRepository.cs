using System.Net;
using Newtonsoft.Json;
using StockportWebapp.Config;
using StockportWebapp.Http;

namespace StockportWebapp.Repositories
{
    public interface IBaseRepository
    {
        Task<T> GetResponseAsync<T>(string url);
        void AddHeader(string key, string value);
    }

    public class BaseRepository : IBaseRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly Dictionary<string, string> _authenticationHeaders;
        private readonly ILogger<BaseRepository> _logger;

        public BaseRepository(IHttpClient httpClient, IApplicationConfiguration config, ILogger<BaseRepository> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<T> GetResponseAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.Get(url, _authenticationHeaders);
                return JsonConvert.DeserializeObject<T>(response.Content as string);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"Error getting response for url {url}");
                return default(T);
            }
        }

        public async Task<HttpStatusCode> PutResponseAsync<T>(string url, HttpContent httpContent)
        {
            try
            {
                var response = await _httpClient.PutAsync(url, httpContent, _authenticationHeaders);
                return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"Error getting response for url {url}");
                throw;
            }
        }

        public void AddHeader(string key, string value)
        {
            if (!_authenticationHeaders.ContainsKey(key)) _authenticationHeaders.Add(key, value);
        }
    }
}
