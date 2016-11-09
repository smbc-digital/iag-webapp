using StockportWebapp.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockportWebappTests.Unit.Http
{
    public class FakeHttpClient : IHttpClient
    {
        private string _url;
        private readonly Dictionary<string, HttpResponse> _responses = new Dictionary<string, HttpResponse>();
        private Exception _exception;

        public string invokedUrl;

        public FakeHttpClient For(string url)
        {
            _url = url;
            return this;
        }

        public void SetResponse(string url, string content)
        {
            _responses.Add(url, new HttpResponse(200, content, string.Empty));
        }

        public void Return(HttpResponse response)
        {
            _responses.Add(_url, response);
        }

        public void Throw(Exception exception)
        {
            _exception = exception;
        }

        public Task<HttpResponse> Get(string url)
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
    }
}
