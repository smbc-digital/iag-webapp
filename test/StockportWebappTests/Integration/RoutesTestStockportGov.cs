using System.IO;
using FluentAssertions;
using StockportWebapp.Http;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System;
using Microsoft.AspNetCore.TestHost;

namespace StockportWebappTests.Integration
{
    public class RoutesTestStockportGov : IDisposable
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;

        public RoutesTestStockportGov()
        {
            FakeHttpClientFactory.MakeFakeHttpClientWithConfiguration(fakeHttpClient =>
            {
                fakeHttpClient.For("http://content:5001/api/stockportgov/homepage").Return(HttpResponse.Successful(200, ReadFile("HomepageStockportGov")));
            });

            _server = TestAppFactory.MakeFakeApp("stockportgov", "int");          
            _client = _server.CreateClient();
        }

        [Fact(Skip = "From task #321, skipping until we can set EnvVariable per test in TestAppFactory")]
        public void ItReturnsAHomepage()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Benefits &amp; Support");
            result.Should().Contain("Libraries");
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
