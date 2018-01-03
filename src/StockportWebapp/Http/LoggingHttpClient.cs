using System;
using System.Collections.Generic;
using System.Net;
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

        public async Task<HttpResponse> Get(string url, Dictionary<string, string> headers)
        {
            _logger.LogInformation("Querying: " + url);
            try
            {
                HttpResponse response = await _inner.Get(url, headers);
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

        public async Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content)
        {
            _logger.LogInformation("Posting: " + requestURI);

            try
            {
                HttpResponseMessage response = await _inner.PostRecaptchaAsync(requestURI, content);
                _logger.LogDebug("Response: " + response);
                return await Task.FromResult(response);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0, ex, "Failed to post the requested resource: ");
                    return handle;
                });
            }

            return await Task.FromResult(new HttpResponseMessage());
        }

        public async Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            _logger.LogInformation("Putting: " + requestURI);

            try
            {
                HttpResponse response = await _inner.PutAsync(requestURI, content, headers);
                _logger.LogDebug("Response: " + response);
                return await Task.FromResult(response);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0, ex, "Failed to post the requested resource: ");
                    return handle;
                });
            }

            return await Task.FromResult(new HttpResponse((int)HttpStatusCode.OK, null, string.Empty));
        }

        public async Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            _logger.LogInformation("Posting: " + requestURI);

            try
            {
                HttpResponse response = await _inner.PostAsync(requestURI, content, headers);
                _logger.LogDebug("Response: " + response);
                return await Task.FromResult(response);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0, ex, "Failed to post the requested resource: ");
                    return handle;
                });
            }

            return await Task.FromResult(new HttpResponse((int)HttpStatusCode.OK, null, string.Empty));
        }

        public async Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers)
        {
            _logger.LogInformation("Deleting: " + requestURI);

            try
            {
                HttpResponse response = await _inner.DeleteAsync(requestURI, headers);
                _logger.LogDebug("Response: " + response);
                return await Task.FromResult(response);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0, ex, "Failed to post the requested resource: ");
                    return handle;
                });
            }

            return await Task.FromResult(new HttpResponse((int)HttpStatusCode.OK, null, string.Empty));
        }

        public async Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            _logger.LogInformation("Posting: " + requestURI);

            try
            {
                var response = await _inner.PostAsyncMessage(requestURI, content, headers);
                _logger.LogDebug("Response: " + response);
                return response;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex => {
                    bool handle = ex is HttpRequestException;
                    if (handle)
                        _logger.LogError(0, ex, "Failed to post the requested resource: ");
                    return handle;
                });
            }

            return await Task.FromResult(new HttpResponseMessage());
        }
    }
}
