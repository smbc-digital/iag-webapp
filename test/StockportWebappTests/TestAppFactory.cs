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
            services.AddMvc(options => 
            {
                for (var i = 0; i < options.Filters.Count; i++)
                {
                    options.Filters.RemoveAt(i);
                }
            });
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
                {new Uri("http://localhost:5001/_healthcheck"), ReadFile("Healthcheck")}
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

            Client.For("http://localhost:5001/api/unittest/article/non-existent-url")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/api/unittest/article/this-is-a-redirect-from")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/api/healthystockport/start-page/start-page")
                .Return(HttpResponse.Successful(200, ReadFile("StartPage")));
            Client.For("http://localhost:5001/api/healthystockport/topic/test-topic")
                .Return(HttpResponse.Successful(200, ReadFile("TopicWithAlerts")));
            Client.For("http://localhost:5001/api/healthystockport/article/physical-activity")
                .Return(HttpResponse.Successful(200, ReadFile("Article")));
            Client.For("http://localhost:5001/api/healthystockport/homepage")
                .Return(HttpResponse.Successful(200, ReadFile("HomepageHealthyStockport")));
            Client.For("http://localhost:5001/api/healthystockport/news/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
            Client.For("http://localhost:5001/api/healthystockport/events/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("EventListing")));
            Client.For("http://localhost:5001/api/healthystockport/events/latest/2?featured=true")
                .Return(HttpResponse.Successful(200, ReadFile("EventListingFeatured")));
            Client.For("http://localhost:5001/api/healthystockport/news")
                .Return(HttpResponse.Successful(200, ReadFile("Newsroom")));
            Client.For("http://localhost:5001/api/healthystockport/profile/test-profile")
                .Return(HttpResponse.Successful(200, ReadFile("Profile")));
            Client.For("http://localhost:5001/api/healthystockport/article/contact-us")
                .Return(HttpResponse.Successful(200, ReadFile("ContactUsArticle")));
            Client.For("http://localhost:5001/api/healthystockport/article/about")
                .Return(HttpResponse.Successful(200, ReadFile("StandaloneArticleWithProfile")));
            Client.For("http://localhost:5001/api/redirects")
                .Return(HttpResponse.Successful(200, ReadFile("Redirects")));
            Client.For("http://localhost:5001/api/stockportgov/homepage")
                .Return(HttpResponse.Successful(200, ReadFile("HomepageStockportGov")));
            Client.For("http://localhost:5001/api/stockportgov/topic/test-topic")
                .Return(HttpResponse.Successful(200, ReadFile("TopicWithAlerts")));
            Client.For("http://localhost:5001/api/stockportgov/article/physical-activity")
                .Return(HttpResponse.Successful(200, ReadFile("Article")));
            Client.For("http://localhost:5001/api/stockportgov/start-page/start-page")
                .Return(HttpResponse.Successful(200, ReadFile("StartPage")));
            Client.For("http://localhost:5001/api/stockportgov/news")
                .Return(HttpResponse.Successful(200, ReadFile("Newsroom")));
            Client.For("http://localhost:5001/api/stockportgov/news/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
            Client.For("http://localhost:5001/api/stockportgov/news/latest/7")
                .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
            Client.For("http://localhost:5001/api/stockportgov/news/test")
                .Return(HttpResponse.Successful(200, ReadFile("News")));
            Client.For("http://localhost:5001/api/stockportgov/events/latest/2")
                .Return(HttpResponse.Successful(200, ReadFile("EventListing")));
            Client.For("http://localhost:5001/api/stockportgov/events/latest/2?featured=true")
                .Return(HttpResponse.Successful(200, ReadFile("EventListingFeatured")));
            Client.For("http://localhost:5001/api/stockportgov/profile/test-profile")
                .Return(HttpResponse.Successful(200, ReadFile("Profile")));
            Client.For("http://localhost:5001/api/stockportgov/footer")
                .Return(HttpResponse.Successful(200, ReadFile("Footer")));
            Client.For("http://localhost:5001/api/healthystockport/footer")
                .Return(HttpResponse.Successful(200, ReadFile("Footer")));
            Client.For("http://localhost:5001/api/stockportgov/eventhomepage")
                .Return(HttpResponse.Successful(200, ReadFile("EventHomepage")));
            Client.For("http://localhost:5001/api/stockportgov/events")
                .Return(HttpResponse.Successful(200, ReadFile("EventsCalendar")));
            Client.For("http://localhost:5001/api/stockportgov/events/event-of-the-century")
                .Return(HttpResponse.Successful(200, ReadFile("Event")));
            Client.For("http://localhost:5001/api/stockportgov/atoz/a")
                .Return(HttpResponse.Successful(200, ReadFile("AtoZ")));
            Client.For("http://localhost:5001/api/stockportgov/showcase/a-showcase")
                .Return(HttpResponse.Successful(200, ReadFile("Showcase")));
            Client.For("http://localhost:5001/api/stockportgov/group/test-zumba-slug")
                .Return(HttpResponse.Successful(200, ReadFile("Group")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/")
                .Return(HttpResponse.Successful(200, ReadFile("GroupResults")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/?location=Stockport")
                .Return(HttpResponse.Successful(200, ReadFile("GroupResults")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/?latitude=53.40581278523235&longitude=-2.158041000366211")
                .Return(HttpResponse.Successful(200, ReadFile("GroupResults")));
            Client.For("http://localhost:5001/api/stockportgov/groupCategory/")
                .Return(HttpResponse.Successful(200, ReadFile("GroupStart")));
            Client.For("http://localhost:5001/api/healthystockport/ContactUsId/test-email")
                .Return(HttpResponse.Successful(200, ReadFile("ContactUsId")));
            Client.ForPostAsync("https://www.google.com/recaptcha/api/siteverify")     
                .ReturnPostAsync(new HttpResponseMessage() { Content = new StringContent("{\"success\": true,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}") });
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }
    }
}