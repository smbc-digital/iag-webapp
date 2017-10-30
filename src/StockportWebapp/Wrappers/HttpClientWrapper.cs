using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockportWebapp.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
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
    }
}
