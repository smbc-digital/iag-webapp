using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Http
{
    public class LoggingHttpClient : IHttpClient
    {
        readonly IHttpClient _inner;
        private readonly ILogger _logger;

        public LoggingHttpClient(IHttpClient inner, ILogger<LoggingHttpClient> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<HttpResponse> Get(string url)
        {
            _logger.LogInformation("Querying: " + url);
            try
            {
                HttpResponse response = await _inner.Get(url);
                _logger.LogDebug("Response: " + response);
                return await Task.FromResult(response);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0,ex, "Failed to get the requested resource: ");
                    return handle;
                });
            }
            return await Task.FromResult(HttpResponse.Failure(503, "Failed to invoke the requested resource"));
        }
    }
}
