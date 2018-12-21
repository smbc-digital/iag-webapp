using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockportWebappTests_Integration.Fake
{
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, dynamic> _fakeResponses = new Dictionary<Uri, dynamic>();
        public HttpRequestMessage HttpRequest;
        public string RequestContent;

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            if (!_fakeResponses.ContainsKey(uri)) _fakeResponses.Add(uri, responseMessage);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            HttpRequest = request;
            AssignRequestContent();

            if (!_fakeResponses.ContainsKey(request.RequestUri))
                return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request});

            var response = _fakeResponses[request.RequestUri];
            if (response.GetType() == typeof(HttpResponseMessage)) return (HttpResponseMessage) response;

            throw (HttpRequestException) response;
        }

        private void AssignRequestContent()
        {
            if (HttpRequest.Method != HttpMethod.Post && HttpRequest.Method != HttpMethod.Put) return;
            var requestContentTask = HttpRequest.Content.ReadAsStringAsync();
            requestContentTask.Wait();
            RequestContent = requestContentTask.Result;
        }

        public void ThrowException(Uri uri, HttpRequestException httpRequestException)
        {
            _fakeResponses.Add(uri, httpRequestException);
        }
    }
}
