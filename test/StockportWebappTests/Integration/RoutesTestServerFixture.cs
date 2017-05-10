using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using StockportWebapp.Config;
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
            string result = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (WeAreInAWS(result))
            {
                result = FetchEnvironmentNameForAWS();
            }

            return result;
        }

        private string FetchEnvironmentNameForAWS()
        {
            // ConfigurationLoader.EnvironmentName knows to do some extra AWS finagling if environment name is "Production"
            const string defaultAWSEnvName = "Production";
            var env = new HostingEnvironment
            {
                EnvironmentName = defaultAWSEnvName,
                ContentRootPath = Directory.GetCurrentDirectory()
            };
            var configBuilder = new ConfigurationBuilder();
            const string configDirectory = "app-config";
            var configLoader = new ConfigurationLoader(configBuilder, configDirectory);

            return configLoader.EnvironmentName(env);
        }

        // NOTE: There is an issue in AWS ElasticBeanstalk that means our environment variables aren't set.
        // This will mean that ASPNETCORE_ENVIRONMENT will default to Production.
        private bool WeAreInAWS(string environmentName)
        {
            return environmentName == "Production";
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