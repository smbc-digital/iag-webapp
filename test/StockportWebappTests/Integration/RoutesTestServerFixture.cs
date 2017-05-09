using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebappTests.Integration
{
    public class RoutesTestServerFixture : IDisposable
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;

        public RoutesTestServerFixture()
        {
            var intEnvironment = GetEnvironmentNameFromASPNETCORE_ENVIRONMENT();
            Console.WriteLine($"Using appsettings.{intEnvironment}.json");

            _server = TestAppFactory.MakeFakeApp("healthystockport", intEnvironment);
            _client = _server.CreateClient();
        }

        private string GetEnvironmentNameFromASPNETCORE_ENVIRONMENT()
        {
            var result = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (result == "Production")
            {
                var env = new HostingEnvironment
                {
                    // If you pass in "Production" it will go to the AWS environment settings and check in ASPNETCORE_ENVIRONMENT
                    EnvironmentName = "Production",
                    ContentRootPath = Directory.GetCurrentDirectory()
                };
                var startup = new StockportWebapp.Startup(env);
                result = startup.EnvironmentName;
            }

            return result;
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
    }
}