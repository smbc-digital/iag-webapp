using System;
using Microsoft.AspNetCore.TestHost;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebappTests.Integration
{
    public class RoutesTestServerFixture : IDisposable
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;
        private const string IntEnvironment = "int";

        public RoutesTestServerFixture()
        {
            TestContentApiFixture.SetupContentApiResponses();

            _server = TestAppFactory.MakeFakeApp("healthystockport", IntEnvironment);
            _client = _server.CreateClient();
        }

        public HttpClient Client => _client;

        public void SetBusinessIdRequestHeader(string business)
        {
            _client.DefaultRequestHeaders.Remove("BUSINESS-ID");
            _client.DefaultRequestHeaders.Add("BUSINESS-ID", business);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        public void AddLegacyRedirectRule(string legacyUrl, string councilTax)
        {
//            throw new NotImplementedException();
        }
    }
}