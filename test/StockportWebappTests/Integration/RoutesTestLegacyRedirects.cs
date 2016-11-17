using System.IO;
using FluentAssertions;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Moq;
using StockportWebapp.Controllers;

namespace StockportWebappTests.Integration
{
    public class RoutesTestLegacyRedirects :IDisposable
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;
       private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public RoutesTestLegacyRedirects()
        {
           _server = TestAppFactory.MakeFakeApp("stockportgov", "int");
           _client = _server.CreateClient();

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                .Returns("/services/councildemocracy/counciltax/difficultypaying");
        }

        [Fact]
        public void ItShouldRedirectPage()
        {
           LegacyRedirectsManager legacyRedirectsManager = new LegacyRedirectsManager(_httpContextAccessor.Object);
           var result = legacyRedirectsManager.RedirectUrl();

            result.Should().Be("/council-tax");
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }
    }

   
}
