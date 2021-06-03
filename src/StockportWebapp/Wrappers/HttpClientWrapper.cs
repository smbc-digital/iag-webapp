using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StockportWebapp.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<T> GetAsync<T>(string url);
    }

    /// <summary>
    /// A wrapper around the HttpClient class to be able to mock HttpClient
    /// </summary>
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientWrapper> _logger;

        public HttpClientWrapper(HttpClient httpClient, ILogger<HttpClientWrapper> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                return await _httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"There was an error calling url: {url} using GetAsync");
                return null;
            }
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

                _logger.LogError($"There was an error calling url: {url} using GetAsync for type: {GetType().Name} returned status code: {response.StatusCode}");
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"There was an error calling url: {url} using GetAsync for type: {GetType().Name}");
                return default(T);
            }
        }
    }
}
