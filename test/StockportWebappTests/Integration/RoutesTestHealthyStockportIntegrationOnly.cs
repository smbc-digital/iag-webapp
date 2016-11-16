using FluentAssertions;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace StockportWebappTests.Integration
{
    public class RoutesTestHealthyStockportIntegrationOnly : IClassFixture<RoutesTestServerFixture>
    {
        private RoutesTestServerFixture _testServerFixture;

        public RoutesTestHealthyStockportIntegrationOnly(RoutesTestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            testServerFixture.SetBusinessIdRequestHeader("healthystockport");
        }

        public HttpClient Client()
        {
            return _testServerFixture.Client;
        }

        [Theory]
        [InlineData("/", 30)]
        [InlineData("/topic/test-topic", 30)]
        [InlineData("/physical-activity", 15)]
        [InlineData("/profile/test-profile", 30)]
        [InlineData("/start/start-page", 15)]
        public async void ItReturnsTheCorrectHeaders(string path, int time)
        {
            var result = await Client().GetAsync(path);

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(time));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public async void ItReturnsTheCorrectHeadersForArticles()
        {
            var result = await Client().GetAsync("/physical-activity");

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(15));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public void ItReturnsSubItemsInTheHomePage()
        {
            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find or renew a book");
            result.Should().Contain("School holiday");
        }

        [Fact]
        public void ItReturnsAStartPage()
        {
            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/start/start-page"));

            result.Should().Contain("Start Page");
            result.Should().Contain("An upper body");
            result.Should().Contain("Lower body");
        }

        [Fact]
        public void ItReturnsATopicWithAlerts()
        {
            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/topic/test-topic"));

            result.Should().Contain("This is an alert");
            result.Should().Contain("It also has a body text");
        }

        [Fact]
        public void ItReturnsAnArticlePageWithFirstSectionOnlyGivenArticleSlug()
        {
            var articleSummary = "Being active is great for your body";
            var firstSectionBody = "Staying active and exercising";
            var sectionTwoBody = "Blah blah blah here";

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/physical-activity"));

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

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/physical-activity/test-profile-section"));

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

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/physical-activity/physical-activity-overview"));

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

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/about"));

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

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/physical-activity/types-of-physical-activity"));

            result.Should().Contain(requestedSectionBody);
            result.Should().NotContain(articleSummary);
            result.Should().NotContain(sectionOneBody);
        }

        [Fact]
        public void ItReturnsAProfilePage()
        {
            var requestedTitle = "Test Profile";
            var requestedBody = "Test body";

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/profile/test-profile"));

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

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/contact-us"));

            result.Should().Contain(formHtmlTag);
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

            var result = AsyncTestHelper.Resolve(Client().SendAsync(request));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.OriginalString.Should().Be("/thank-you?referer=%2Fa-page");
        }

        [Fact]
        public void ItReturnsAHealthcheck()
        {
            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/_healthcheck"));

            result.Should().Contain("appVersion");
            result.Should().Contain("sha");
            result.Should().Contain("featureToggles");
            result.Should().Contain("dependencies");
            result.Should().Contain("contentApi");
        }

        // this works when run independently, however it will fail randomally when run in "run all" with the other tests
        [Fact(Skip = "#272 - skipping until test server is sorted to run between multiple tests")]
        public void ItReturnsCorrectHeadersForRedirects()
        {
            var result = AsyncTestHelper.Resolve(Client().GetAsync("/this-is-another-article"));

            result.Headers.CacheControl.MaxAge.Value.Should().Be(TimeSpan.FromSeconds(21600));
        }

        [Fact]
        public void ItReturnsAnRssFeed()
        {
            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/news/rss"));

            result.Should().Contain("Stockport Council News Feed");
            result.Should().Contain("Another news article");
            result.Should().Contain("rss version=\"2.0\"");
        }

        [Fact]
        public void ItReturnsARobotsFileForHealthyStockport()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/robots.txt"));

            result.Should().Contain("# yes robots");
        }

        [Fact]
        public void ItPerformsARedirectWhenRequestMatchesAnExactLegacyRedirectRule()
        {
            var legacyUrl = "/services/councildemocracy/counciltax/difficultypaying";
            _testServerFixture.AddLegacyRedirectRule(legacyUrl, "/council-tax");

            var result = AsyncTestHelper.Resolve(Client().GetAsync(legacyUrl));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItGives404ForANonExistentPageWithoutALegacyRedirectRule()
        {
            var result = AsyncTestHelper.Resolve(Client().GetAsync("/non-existent-url"));

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void SetBusinessIdRequestHeader(string businessId)
        {
            _testServerFixture.SetBusinessIdRequestHeader(businessId);
        }
    }
}
