using System.IO;
using FluentAssertions;
using StockportWebapp.Http;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace StockportWebappTests.Integration
{
    public class RoutesTestHealthyStockport : IDisposable
    {
        private const string IntEnvironment = "int";
        private HttpClient _client;
        private TestServer _server;

        public RoutesTestHealthyStockport()
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
            });

            FakeResponseHandlerFactory.MakeFakeWithUrlConfiguration(() =>
            {
                var dictionary = new Dictionary<Uri, string>
                {
                    {new Uri("http://content:5001/_healthcheck"), ReadFile("Healthcheck")}
                };
                return dictionary;
            });

            _server = TestAppFactory.MakeFakeApp("healthystockport", IntEnvironment);
            _client = _server.CreateClient();
            SetBusinessIdRequestHeader("healthystockport");
        }

        [Fact]
        public void ItReturnsContentFromTheBusinessRequestedInTheBusinessIdHeader()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var stockportResult = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));
            stockportResult.Should().Contain("Welcome to Stockport Council");

            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "healthystockport");
            SetBusinessIdRequestHeader("healthystockport");

            var healthyResult = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            healthyResult.Should().Contain("Welcome to Healthy Stockport");
            healthyResult.Should().Contain("Eat healthy", "Should render a business-specific piece of content");

        }

        [Theory]
        [InlineData("/", 30)]
        [InlineData("/topic/test-topic", 30)]
        [InlineData("/physical-activity", 15)]
        [InlineData("/profile/test-profile", 30)]
        [InlineData("/start/start-page", 15)]
        public async void ItReturnsTheCorrectHeaders(string path, int time)
        {
            var result = await _client.GetAsync(path);

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(time));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public async void ItReturnsTheCorrectHeadersForArticles()
        {
            var result = await _client.GetAsync("/physical-activity");

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(15));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public void ItReturnsSubItemsInTheHomePage()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find or renew a book");
            result.Should().Contain("School holiday");
        }

        [Fact]
        public void ItReturnsPopularSearchTermsOnTheHomepage()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            result.Should().Contain("/search?query=popular search term");
        }

        [Fact]
        public void ItReturnsAStartPage()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/start/start-page"));

            result.Should().Contain("Start Page");
            result.Should().Contain("An upper body");
            result.Should().Contain("Lower body");
        }

        [Fact]
        public void ItReturnsATopicWithAlerts()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/topic/test-topic"));

            result.Should().Contain("This is an alert");
            result.Should().Contain("It also has a body text");
        }

        [Fact]
        public void ItReturnsAnArticlePageWithFirstSectionOnlyGivenArticleSlug()
        {
            var articleSummary = "Being active is great for your body";
            var firstSectionBody = "Staying active and exercising";
            var sectionTwoBody = "Blah blah blah here";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/physical-activity"));

            result.Should().Contain(articleSummary);
            result.Should().Contain(firstSectionBody);
            result.Should().NotContain(sectionTwoBody);
        }

        [Fact]
        public void ItReturnsAnArticlePageAndRendersProfileInSection()
        {
            var profileTitle = "Test Profile";
            var profileTeaser = "Profile teaser";
            var profileSubtitle = "This is a test profile";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/physical-activity/test-profile-section"));

            result.Should().Contain(profileTitle);
            result.Should().Contain(profileTeaser);
            result.Should().Contain(profileSubtitle);
        }

        [Fact]
        public void ItReturnsAnArticlePageAndRendersDocumentInSection()
        {
            var documentTitle = "Metroshuttle route map";
            var documentSize = "658 KB";
            var documentUrl = "document.pdf";
            var fileName = "document.pdf";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/physical-activity/physical-activity-overview"));

            result.Should().Contain(documentTitle);
            result.Should().Contain(documentSize);
            result.Should().Contain(documentUrl);
            result.Should().Contain(fileName);
        }

        [Fact]
        public void ItReturnsAStandaloneArticlePageAndRendersProfile()
        {
            var profileTitle = "Test Profile";
            var profileTeaser = "This is a profile teaser";
            var profileSubtitle = "This is a test profile";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/about"));

            result.Should().Contain(profileTitle);
            result.Should().Contain(profileTeaser);
            result.Should().Contain(profileSubtitle);
            result.Should().NotContain("<code>");
        }

        [Fact]
        public void ItReturnsAnArticlePageWithRequestedSectionGivenArticleAndSectionSlugs()
        {
            var requestedSectionBody = "body content";
            var articleSummary = "Being active is great for your body";
            var sectionOneBody = "not in the content";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/physical-activity/types-of-physical-activity"));

            result.Should().Contain(requestedSectionBody);
            result.Should().NotContain(articleSummary);
            result.Should().NotContain(sectionOneBody);
        }

        [Fact]
        public void ItReturnsAProfilePage()
        {
            var requestedTitle = "Test Profile";
            var requestedBody = "Test body";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/profile/test-profile"));

            result.Should().Contain(requestedTitle);
            result.Should().Contain(requestedBody);
        }

        [Theory]
        [InlineData("int")]
        [InlineData("qa")]
        [InlineData("stage")]
        public void ItReturnsAContactUsPage(string environment)
        {
            var formHtmlTag = "<form";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/contact-us"));

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public void ItReturnsAContactUsPageWithTheValidationMessageInInt()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "healthystockport");

            var contactUsMessage = "You filled the form out incorrectly";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync($"/contact-us?message={contactUsMessage}"));

            result.Should().Contain(contactUsMessage);
        }

        [Fact]
        public void ItReturnsThankYouMessageOnSuccessEmail()
        {
            var formContents = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Name", "Bill"),
                new KeyValuePair<string, string>("Email", "bill@place.uk"),
                new KeyValuePair<string, string>("Subject", "Test Subject"),
                new KeyValuePair<string, string>("Message", "Test Message"),
                new KeyValuePair<string, string>("ServiceEmail", "service@place.uk")
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/contact-us") { Content = formContents };
            request.Headers.Add("Referer", "http://something.com/a-page");

            var result = AsyncTestHelper.Resolve(_client.SendAsync(request));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.OriginalString.Should().Be("/thank-you?referer=%2Fa-page");
        }

        [Fact]
        public void ItReturnsRedirectResponseOnSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/search?something=something"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsRedirectResponseOnPostcodeSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/postcode?postcode=this"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsAHealthcheck()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/_healthcheck"));

            result.Should().Contain("appVersion");
            result.Should().Contain("sha");
            result.Should().Contain("featureToggles");
            result.Should().Contain("dependencies");
            result.Should().Contain("contentApi");
        }

        [Fact]
        public void ItRedirectsToExistingStockportGovSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/search?query=hello"));

            result.Headers.Location.Should().Be("http://stockport.searchimprove.com/search.aspx?pc=&pckid=816028173&aid=448530&pt=6018936&addid=&sw=hello");
            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        // this works when run independently, however it will fail randomally when run in "run all" with the other tests
        [Fact(Skip = "#272 - skipping until test server is sorted to run between multiple tests")]
        public void ItReturnsCorrectHeadersForRedirects()
        {
            var result = AsyncTestHelper.Resolve(_client.GetAsync("/this-is-another-article"));

            result.Headers.CacheControl.MaxAge.Value.Should().Be(TimeSpan.FromSeconds(21600));
        }

        [Fact]
        public void ItReturnsAnRssFeed()
        {
            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/news/rss"));

            result.Should().Contain("Stockport Council News Feed");
            result.Should().Contain("Another news article");
            result.Should().Contain("rss version=\"2.0\"");
        }

        [Fact]
        public void ItReturnsARobotsFileForStockportGov()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/robots.txt"));

            result.Should().Contain("# no robots");
        }

        [Fact]
        public void ItReturnsARobotsFileForHealthyStockport()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/robots.txt"));

            result.Should().Contain("# yes robots");
        }

        private void SwitchEnvironmentIncludingBusinessIdEnvVar(string environment, string businessId)
        {
            _server = TestAppFactory.MakeFakeApp(businessId, environment);
            _client = _server.CreateClient();
            SetBusinessIdRequestHeader(businessId);
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText($"Unit/MockResponses/{fileName}.json");
        }

        private void SetBusinessIdRequestHeader(string business)
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
