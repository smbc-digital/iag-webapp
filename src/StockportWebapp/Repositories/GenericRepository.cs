using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.Http;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetResponseAsync(string url);
        void AddHeader(string key, string value);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly Dictionary<string, string> _authenticationHeaders;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(IHttpClient httpClient, IApplicationConfiguration config, ILogger<GenericRepository<T>> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<T> GetResponseAsync(string url)
        {
            try
            {
                var response = await _httpClient.Get(url, _authenticationHeaders);
                return JsonConvert.DeserializeObject<T>(response.Content as string);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"Error getting response for url {url}");
                return null;
            }
        }

        public void AddHeader(string key, string value)
        {
            if (!_authenticationHeaders.ContainsKey(key)) _authenticationHeaders.Add(key, value);
        }
    }
}
