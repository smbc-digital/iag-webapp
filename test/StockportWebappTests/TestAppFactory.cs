using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using StockportWebapp;
using StockportWebapp.Http;
using StockportWebappTests.Unit.Http;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using StockportWebapp.AmazonSES;
using StockportWebappTests.Unit.Fake;

namespace StockportWebappTests
{
    public class TestAppFactory
    {
        public static TestServer MakeFakeApp(string businessId, string environment)
        {
            Environment.SetEnvironmentVariable("BUSINESS_ID", businessId);
            Environment.SetEnvironmentVariable("SES_ACCESS_KEY", "access-key");
            Environment.SetEnvironmentVariable("SES_SECRET_KEY", "secret-key");

            var hostBuilder = new WebHostBuilder()
              .UseStartup<FakeStartup>()
              .UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "src", "StockportWebapp"))
              .UseUrls("http://localhost:5001")
              .UseKestrel()
              .UseEnvironment(environment);

            return new TestServer(hostBuilder);
        }
    }

    public class FakeStartup : Startup
    {
        public FakeStartup(IHostingEnvironment env) : base(env){}

        public  LoggingHttpClient GetHttpClient(ILoggerFactory loggerFactory)
        {
            var fakeHttpClient = FakeHttpClientFactory.Client;
            
            return new LoggingHttpClient(fakeHttpClient, loggerFactory.CreateLogger<LoggingHttpClient>());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddTransient<IHttpClient>(p => GetHttpClient(p.GetService<ILoggerFactory>()));
            services.AddSingleton<IHttpEmailClient, FakeHttpEmailClient>();
            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => new System.Net.Http.HttpClient(FakeResponseHandlerFactory.ResponseHandler));
        }
    }

    class FakeResponseHandlerFactory
    {
        public static void MakeFakeWithUrlConfiguration(
            Func<Dictionary<Uri, string>> urlsFunc)
        {
            ResponseHandler = new FakeResponseHandler();
            var urlsDict = urlsFunc();
            foreach (var url in urlsDict.Keys)
            {
                var httpResponseMessage = new HttpResponseMessage() {Content = new StringContent(urlsDict[url])};
                ResponseHandler.AddFakeResponse(url, httpResponseMessage);
            }
        }

        public static FakeResponseHandler ResponseHandler { get; private set; }

    }

    class FakeHttpClientFactory
    {
        public static void MakeFakeHttpClientWithConfiguration(
            Action<FakeHttpClient> configureFakeHttpClient)
        {
            Client = new FakeHttpClient();
            configureFakeHttpClient(Client);
        }

        public static FakeHttpClient Client { get; private set; }
    }
}