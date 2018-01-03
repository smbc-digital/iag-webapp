using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace StockportWebapp.Http
{
    public interface IHttpClient
    {
        Task<HttpResponse> Get(string url, Dictionary<string, string> headers);
        Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content);
        Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers);
        Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers);
        Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers);
        Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content,
            Dictionary<string, string> headers);
    }

    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient(System.Net.Http.HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponse> Get(string url, Dictionary<string, string> headers)
        {
            headers.ToList().ForEach(header =>
            {
                _client.DefaultRequestHeaders.Remove(header.Key);
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            });

            try
            {
                var task = await _client.GetAsync(url);

                var content = await task.Content.ReadAsStringAsync();

                return new HttpResponse((int)task.StatusCode,
                    content,
                    task.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content)
        {
            return _client.PostAsync(requestURI, content);
        }

        public async Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            headers.ToList().ForEach(header =>
            {
                _client.DefaultRequestHeaders.Remove(header.Key);
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            });
            var task = await _client.PostAsync(requestURI, content);

            return new HttpResponse((int)task.StatusCode,
                                    content,
                                    task.ReasonPhrase);
        }

        public async Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            headers.ToList().ForEach(header =>
            {
                _client.DefaultRequestHeaders.Remove(header.Key);
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            });
            return await _client.PostAsync(requestURI, content);
        }

        public async Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            headers.ToList().ForEach(header =>
            {
                _client.DefaultRequestHeaders.Remove(header.Key);
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            });
            var task = await _client.PutAsync(requestURI, content);

            return new HttpResponse((int)task.StatusCode,
                                    content,
                                    task.ReasonPhrase);
        }

        public async Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers)
        {
            headers.ToList().ForEach(header =>
            {
                _client.DefaultRequestHeaders.Remove(header.Key);
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            });
            var task = await _client.DeleteAsync(requestURI);

            return new HttpResponse((int)task.StatusCode,
                                    null,
                                    task.ReasonPhrase);
        }
    }
}