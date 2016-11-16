using System;
using System.Collections.Generic;
using System.IO;
using StockportWebapp.Http;

namespace StockportWebappTests.Integration
{
    public class TestContentApiFixture
    {
        public static void SetupContentApiResponses()
        {
            FakeHttpClientFactory.MakeFakeHttpClientWithConfiguration(fakeHttpClient =>
            {
                fakeHttpClient.For("http://content:5001/api/healthystockport/start-page/start-page")
                    .Return(HttpResponse.Successful(200, ReadFile("StartPage")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/topic/test-topic")
                    .Return(HttpResponse.Successful(200, ReadFile("TopicWithAlerts")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/article/physical-activity")
                    .Return(HttpResponse.Successful(200, ReadFile("Article")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/homepage")
                    .Return(HttpResponse.Successful(200, ReadFile("HomepageHealthyStockport")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/news/latest/2")
                    .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/news")
                    .Return(HttpResponse.Successful(200, ReadFile("Newsroom")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/profile/test-profile")
                    .Return(HttpResponse.Successful(200, ReadFile("Profile")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/article/contact-us")
                    .Return(HttpResponse.Successful(200, ReadFile("ContactUsArticle")));
                fakeHttpClient.For("http://content:5001/api/healthystockport/article/about")
                    .Return(HttpResponse.Successful(200, ReadFile("StandaloneArticleWithProfile")));
                fakeHttpClient.For("http://content:5001/api/redirects")
                    .Return(HttpResponse.Successful(200, ReadFile("Redirects")));
                fakeHttpClient.For("http://content:5001/api/stockportgov/homepage")
                    .Return(HttpResponse.Successful(200, ReadFile("HomepageStockportGov")));
                fakeHttpClient.For("http://content:5001/api/stockportgov/news/latest/2")
                    .Return(HttpResponse.Successful(200, ReadFile("NewsListing")));
                fakeHttpClient.For("http://content:5001/api/stockportgov/footer")
                    .Return(HttpResponse.Successful(200, ReadFile("Footer")));
            });

            FakeResponseHandlerFactory.MakeFakeWithUrlConfiguration(() =>
            {
                var dictionary = new Dictionary<Uri, string>
                {
                    {new Uri("http://content:5001/_healthcheck"), ReadFile("Healthcheck")}
                };
                return dictionary;
            });
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }
    }
}