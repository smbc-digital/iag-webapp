using StockportWebapp.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockportWebappTests_Integration.Http
{
    public class FakeHttpClient : IHttpClient
    {
        private string _url;
        private readonly Dictionary<string, HttpResponse> _responses = new Dictionary<string, HttpResponse>();
        private readonly Dictionary<string, HttpResponseMessage> _postAsyncresponses = new Dictionary<string, HttpResponseMessage>();
        private Exception _exception;

        public string invokedUrl;

        public FakeHttpClient For(string url)
        {
            _url = url;
            return this;
        }

        public FakeHttpClient ForPostAsync(string url)
        {
            _url = url;
            return this;
        }

        public void SetResponse(string url, string content)
        {
            _responses.Add(url, new HttpResponse(200, content, string.Empty));
        }
        public void SetPostAsyncResponse(string url, string content)
        {
            _postAsyncresponses.Add(url, new HttpResponseMessage() { Content = new StringContent(content) });
        }

        public void Return(HttpResponse response)
        {
           if (!_responses.ContainsKey(_url)) _responses.Add(_url, response);
        }

        public void ReturnPostAsync(HttpResponseMessage response)
        {
            if (!_postAsyncresponses.ContainsKey(_url)) _postAsyncresponses.Add(_url, response);
        }

        public void Throw(Exception exception)
        {
            _exception = exception;
        }

        public Task<HttpResponse> Get(string url, Dictionary<string, string> headers)
        {
            invokedUrl = url;
            if (_exception != null)
                throw _exception;

            try
            {
                return Task.FromResult(_responses[url]);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"No response found for: {url}");
                throw new KeyNotFoundException($"No response found for: {url}");
            }
        }

        public bool Invoked(string url)
        {
            return invokedUrl == url;
        }

        public Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content)
        {
            invokedUrl = requestURI;
            if (_exception != null)
                throw _exception;

            try
            {
                return Task.FromResult(_postAsyncresponses[requestURI]);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"No response found for: {requestURI}");
                throw new KeyNotFoundException($"No response found for: {requestURI}");
            }
        }

        public Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }
    }
}
