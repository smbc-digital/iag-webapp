using Microsoft.Extensions.Logging;
using StockportWebapp.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Moq;
using Xunit;

namespace StockportWebappTests.Unit.Http
{
    public class LoggingHttpClientTest
    {
        readonly FakeHttpClient _fakeHttpClient = new FakeHttpClient();

        [Fact]
        public async void HandlesNoResponseFromRemote()
        {
            _fakeHttpClient.For("a url").Throw(
                new AggregateException(new HttpRequestException()));
            var logger = new Mock<ILogger<LoggingHttpClient>>().Object;
            var httpClient = new LoggingHttpClient( _fakeHttpClient, logger);
            HttpResponse response = await httpClient.Get("a url", new Dictionary<string, string>());

            Assert.Equal(503, response.StatusCode);
            Assert.Equal("Failed to invoke the requested resource", response.Error);
        }

        [Fact]
        public async void ReturnsSuccessfulResponseFromRemote()
        {
            _fakeHttpClient.For("a url").Return(HttpResponse.Successful(200, "some data"));
            var logger = new Mock<ILogger<LoggingHttpClient>>().Object;
            var httpClient = new LoggingHttpClient(_fakeHttpClient, logger);
            HttpResponse response = await httpClient.Get("a url", new Dictionary<string, string>());

            Assert.Equal(200, response.StatusCode);
        }
    }
}
