using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using StockportWebapp;
using StockportWebapp.Http;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using StockportWebappTests_Integration.Http;
using StockportWebapp.AmazonSES;
using StockportWebappTests_Integration.Fake;
using StockportWebappTests_Integration.Helpers;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebappTests_Integration
{
    public class TestAppFactory : IClassFixture<WebApplicationFactory<Startup>>
    {
        public readonly WebApplicationFactory<Startup> _factory;
        public HttpClient _client;

        public TestAppFactory(WebApplicationFactory<Startup> factory)
        {
            Environment.SetEnvironmentVariable("SES_ACCESS_KEY", "access-key");
            Environment.SetEnvironmentVariable("SES_SECRET_KEY", "secret-key");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder
                    .UseEnvironment("integrationtest")
                    .UseContentRoot(Path.GetFullPath(Path.Combine(
                        PlatformServices.Default.Application.ApplicationBasePath,
                        "..", "..", "..", "..", "..", "src", "StockportWebapp")))
                    .ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IHttpClient>(p => GetHttpClient(p.GetService<ILoggerFactory>()));
                        services.AddSingleton<Func<HttpClient>>(p => () =>
                            new HttpClient(new FakeResponseHandlerFactory().ResponseHandler));
                        services.AddSingleton<IHttpEmailClient, FakeHttpEmailClient>();
                        services.AddMvc(options =>
                        {
                            for (var i = 0; i < options.Filters.Count; i++)
                            {
                                options.Filters.RemoveAt(i);
                            }
                        });
                    });
            });

            _client = _factory.CreateDefaultClient();
        }

        public void SetBusinessIdRequestHeader(string business)
        {
            _client.DefaultRequestHeaders.Remove("BUSINESS-ID");
            _client.DefaultRequestHeaders.Add("BUSINESS-ID", business);
        }

        public LoggingHttpClient GetHttpClient(ILoggerFactory loggerFactory)
        {
            var fakeHttpClient = new FakeHttpClientFactory().Client;

            return new LoggingHttpClient(fakeHttpClient, loggerFactory.CreateLogger<LoggingHttpClient>());
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
                {new Uri("http://localhost:5001/_healthcheck"), JsonFileHelper.GetStringResponseFromFile("Healthcheck.json")}
            };
            foreach (var url in urlsDict.Keys)
            {
                var httpResponseMessage = new HttpResponseMessage {Content = new StringContent(urlsDict[url])};
                ResponseHandler.AddFakeResponse(url, httpResponseMessage);
            }
        }
    }

    internal class FakeHttpClientFactory 
    {
        public FakeHttpClient Client { get; }

        public FakeHttpClientFactory()
        {
            Client = new FakeHttpClient();

            #region stockportgov
            Client.For("http://localhost:5001/stockportgov/homepage")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("HomepageStockportGov.json")));
            Client.For("http://localhost:5001/stockportgov/topics/test-topic")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("TopicWithAlerts.json")));
            Client.For("http://localhost:5001/stockportgov/articles/physical-activity")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Article.json")));
            Client.For("http://localhost:5001/stockportgov/start-page/start-page")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("StartPage.json")));
            Client.For("http://localhost:5001/stockportgov/news")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Newsroom.json")));
            Client.For("http://localhost:5001/stockportgov/news/latest/1")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("NewsListing.json")));
            Client.For("http://localhost:5001/stockportgov/news/latest/7")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("NewsListing.json")));
            Client.For("http://localhost:5001/stockportgov/news/test")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("News.json")));
            Client.For("http://localhost:5001/stockportgov/events/latest/1")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventListing.json")));
            Client.For("http://localhost:5001/stockportgov/events/latest/1?featured=true")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventListingFeatured.json")));
            Client.For("http://localhost:5001/stockportgov/profiles/test-profile")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Profile.json")));
            Client.For("http://localhost:5001/stockportgov/footer")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Footer.json")));
            Client.For("http://localhost:5001/healthystockport/footer")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Footer.json")));
            Client.For("http://localhost:5001/stockportgov/eventhomepage")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventHomepage.json")));
            Client.For("http://localhost:5001/stockportgov/events")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventsCalendar.json")));
            Client.For("http://localhost:5001/stockportgov/events/event-of-the-century")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Event.json")));
            Client.For("http://localhost:5001/stockportgov/atoz/a")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("AtoZ.json")));
            Client.For("http://localhost:5001/stockportgov/showcases/a-showcase")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Showcase.json")));
            Client.For("http://localhost:5001/stockportgov/groups/test-zumba-slug")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Group.json")));
            Client.For("http://localhost:5001/stockportgov/group-results/")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupResults.json")));
            Client.For("http://localhost:5001/stockportgov/group-results/?location=Stockport")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupResults.json")));
            Client.For("http://localhost:5001/stockportgov/group-results/?latitude=53.40581278523235&longitude=-2.158041000366211")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupResults.json")));
            Client.For("http://localhost:5001/stockportgov/group-categories")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupStart.json")));
            Client.For("http://localhost:5001/stockportgov/grouphomepage")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupHomepage.json")));
            Client.For("http://localhost:5001/stockportgov/smart/smart-test")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Smart.json")));
            Client.For("http://localhost:5001/stockportgov/groups/")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("GroupListing.json")));
            #endregion

            #region healthystockport
            Client.For("http://localhost:5001/healthystockport/start-page/start-page")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("StartPage.json")));
            Client.For("http://localhost:5001/healthystockport/topics/test-topic")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("TopicWithAlerts.json")));
            Client.For("http://localhost:5001/healthystockport/articles/physical-activity")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Article.json")));
            Client.For("http://localhost:5001/healthystockport/homepage")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("HomepageHealthyStockport.json")));
            Client.For("http://localhost:5001/healthystockport/news/latest/1")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("NewsListing.json")));
            Client.For("http://localhost:5001/healthystockport/events/latest/1")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventListing.json")));
            Client.For("http://localhost:5001/healthystockport/events/latest/2?featured=true")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventListingFeatured.json")));
            Client.For("http://localhost:5001/healthystockport/news")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Newsroom.json")));
            Client.For("http://localhost:5001/healthystockport/profiles/test-profile")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Profile.json")));
            Client.For("http://localhost:5001/healthystockport/articles/contact-us")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("ContactUsArticle.json")));
            Client.For("http://localhost:5001/healthystockport/articles/about")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("StandaloneArticleWithProfile.json")));
            Client.For("http://localhost:5001/healthystockport/contact-us-id/test-email")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("ContactUsId.json")));
            #endregion

            #region thirdsite
            Client.For("http://localhost:5001/thirdsite/homepage")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("HomepageThirdSite.json")));
            Client.For("http://localhost:5001/thirdsite/news/latest/2")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("NewsListing.json")));
            Client.For("http://localhost:5001/thirdsite/events/latest/2?featured=true")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("EventListingFeatured.json")));
            Client.For("http://localhost:5001/thirdsite/articles/physical-activity")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Article.json")));
            #endregion

            #region misc
            Client.For("http://localhost:5001/unittest/articles/non-existent-url")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/unittest/articles/this-is-a-redirect-from")
                .Return(HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/redirects")
                .Return(HttpResponse.Successful(200, JsonFileHelper.GetStringResponseFromFile("Redirects.json")));
            Client.ForPostAsync("https://www.google.com/recaptcha/api/siteverify")
                .ReturnPostAsync(new HttpResponseMessage() { Content = new StringContent("{\"success\": true,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}") });
            #endregion
        }
    }
}