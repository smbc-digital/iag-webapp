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

        public LoggingHttpClient GetHttpClient(ILoggerFactory loggerFactory)
        {
            var fakeHttpClient = new FakeHttpClientFactory().Client;
            
            return new LoggingHttpClient(fakeHttpClient, loggerFactory.CreateLogger<LoggingHttpClient>());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IHttpClient>(p => GetHttpClient(p.GetService<ILoggerFactory>()));
            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => new System.Net.Http.HttpClient(new FakeResponseHandlerFactory().ResponseHandler));
            services.AddSingleton<IHttpEmailClient, FakeHttpEmailClient>();
        }
    }

    internal class FakeResponseHandlerFactory
    {
        public FakeResponseHandler ResponseHandler { get; private set; }

        public FakeResponseHandlerFactory()
        {
            ResponseHandler = new FakeResponseHandler();
            var urlsDict = new Dictionary<Uri, string>
            {
                {new Uri("http://content:5001/_healthcheck"), ReadFile("Healthcheck")}
            };
            foreach (var url in urlsDict.Keys)
            {
                var httpResponseMessage = new HttpResponseMessage {Content = new StringContent(urlsDict[url])};
                ResponseHandler.AddFakeResponse(url, httpResponseMessage);
            }
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }
    }

    internal class FakeHttpClientFactory
    {
        public FakeHttpClient Client { get; }

        public FakeHttpClientFactory()
        {
            Client = new FakeHttpClient();

            Client.For("http://content:5001/api/unittest/article/non-existent-url")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://content:5001/api/unittest/article/this-is-a-redirect-from")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://content:5001/api/healthystockport/start-page/start-page")
                .Return(HttpResponse.Successful(200, ReadFile("StartPage")));
            Client.For("http://content:5001/api/healthystockport/topic/test-topic")
                .Return(HttpResponse.Successful(200, ReadFile("TopicWithAlerts")));
            Client.For("http://content:5001/api/healthystockport/article/physical-activity")
                .Return(HttpResponse.Successful(200, ReadFile("Article")));
            Client.For("http://content:5001/api/healthystockport/homepage")
                .Return(HttpResponse.Successful(200, ReadFile("HomepageHealthyStockport")));
            Client.For("http://content:5001/api/healthystockport/news/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
            Client.For("http://content:5001/api/healthystockport/events/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("EventListing")));
            Client.For("http://content:5001/api/healthystockport/news")
                .Return(HttpResponse.Successful(200, ReadFile("Newsroom")));
            Client.For("http://content:5001/api/healthystockport/profile/test-profile")
                .Return(HttpResponse.Successful(200, ReadFile("Profile")));
            Client.For("http://content:5001/api/healthystockport/article/contact-us")
                .Return(HttpResponse.Successful(200, ReadFile("ContactUsArticle")));
            Client.For("http://content:5001/api/healthystockport/article/about")
                .Return(HttpResponse.Successful(200, ReadFile("StandaloneArticleWithProfile")));
            Client.For("http://content:5001/api/redirects")
                .Return(HttpResponse.Successful(200, ReadFile("Redirects")));
            Client.For("http://content:5001/api/stockportgov/homepage")
                .Return(HttpResponse.Successful(200, ReadFile("HomepageStockportGov")));
            Client.For("http://content:5001/api/stockportgov/topic/test-topic")
                .Return(HttpResponse.Successful(200, ReadFile("TopicWithAlerts")));
            Client.For("http://content:5001/api/stockportgov/article/physical-activity")
                .Return(HttpResponse.Successful(200, ReadFile("Article")));
            Client.For("http://content:5001/api/stockportgov/start-page/start-page")
                .Return(HttpResponse.Successful(200, ReadFile("StartPage")));
            Client.For("http://content:5001/api/stockportgov/news")
                .Return(HttpResponse.Successful(200, ReadFile("Newsroom")));
            Client.For("http://content:5001/api/stockportgov/news/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
            Client.For("http://content:5001/api/stockportgov/events/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("EventListing")));
            Client.For("http://content:5001/api/stockportgov/profile/test-profile")
                .Return(HttpResponse.Successful(200, ReadFile("Profile")));
            Client.For("http://content:5001/api/stockportgov/footer")
                .Return(HttpResponse.Successful(200, ReadFile("Footer")));
            Client.For("http://content:5001/api/stockportgov/events")
                .Return(HttpResponse.Successful(200, ReadFile("EventsCalendar")));
            Client.For("http://content:5001/api/stockportgov/events/event-of-the-century")
                .Return(HttpResponse.Successful(200, ReadFile("Event")));
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }
    }
}