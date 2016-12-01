using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System.Net;
using System.Net.Http;

namespace StockportWebappTests.Integration
{
    public class RoutesTest : IClassFixture<RoutesTestServerFixture>
    {
        private readonly HttpClient _client;
        private readonly RoutesTestServerFixture _testServerFixture;

        public RoutesTest(RoutesTestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _client = _testServerFixture.Client;
        }

        [Fact]
        public void ItReturnsCorrectHeadersForRedirects()
        {
            SetBusinessIdRequestHeader("unittest");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/this-is-another-article"));

            result.Headers.CacheControl.MaxAge.Value.Should().Be(TimeSpan.FromSeconds(21600));
        }

        [Fact]
        public void ItGives404ForANonExistentPageWithoutALegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/non-existent-url"));

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public void ItPerformsARedirectWhenRequestMatchesAnExactLegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");
 
            var result = AsyncTestHelper.Resolve(_client.GetAsync("/this-is-a-redirect-from"));
 
            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.ToString().Should().Be("this-is-a-redirect-to");
        }

        #region mixedbusinessids
        [Fact]
        public void ItReturnsContentFromTheBusinessRequestedInTheBusinessIdHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var stockportResult = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));
            stockportResult.Should().Contain("Welcome to Stockport Council");

            SetBusinessIdRequestHeader("healthystockport");

            var healthyResult = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            healthyResult.Should().Contain("Welcome to Healthy Stockport");
            healthyResult.Should().Contain("Eat healthy", "Should render a business-specific piece of content");

        }
        #endregion

        #region healthystockport
        [Fact]
        public void ItReturnsAContactUsPageWithTheValidationMessageInInt()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var contactUsMessage = "You filled the form out incorrectly";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync($"/contact-us?message={contactUsMessage}"));

            result.Should().Contain(contactUsMessage);
        }

        [Theory]
        [InlineData("/", 30)]
        [InlineData("/topic/test-topic", 30)]
        [InlineData("/physical-activity", 15)]
        [InlineData("/profile/test-profile", 30)]
        [InlineData("/start/start-page", 15)]
        public async void ItReturnsTheCorrectHeaders(string path, int time)
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _client.GetAsync(path);

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(time));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public async void ItReturnsTheCorrectHeadersForArticles()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _client.GetAsync("/physical-activity");

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(15));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public void ItReturnsSubItemsInTheHomePage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find or renew a book");
            result.Should().Contain("School holiday");
        }

        [Fact]
        public void ItReturnsAStartPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/start/start-page"));

            result.Should().Contain("Start Page");
            result.Should().Contain("An upper body");
            result.Should().Contain("Lower body");
        }

        [Fact]
        public void ItReturnsATopicWithAlerts()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/topic/test-topic"));

            result.Should().Contain("This is an alert");
            result.Should().Contain("It also has a body text");
        }

        [Fact]
        public void ItReturnsAnArticlePageWithFirstSectionOnlyGivenArticleSlug()
        {
            SetBusinessIdRequestHeader("healthystockport");

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
            SetBusinessIdRequestHeader("healthystockport");

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
            SetBusinessIdRequestHeader("healthystockport");

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
            SetBusinessIdRequestHeader("healthystockport");

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
            SetBusinessIdRequestHeader("healthystockport");

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
            SetBusinessIdRequestHeader("healthystockport");

            var requestedTitle = "Test Profile";
            var requestedBody = "Test body";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/profile/test-profile"));

            result.Should().Contain(requestedTitle);
            result.Should().Contain(requestedBody);
        }

        [Fact]
        public void ItReturnsAContactUsPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var formHtmlTag = "<form";

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/contact-us"));

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public void ItReturnsThankYouMessageOnSuccessEmail()
        {
            SetBusinessIdRequestHeader("healthystockport");

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
        public void ItReturnsAHealthcheck()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/_healthcheck"));

            result.Should().Contain("appVersion");
            result.Should().Contain("sha");
            result.Should().Contain("featureToggles");
            result.Should().Contain("dependencies");
            result.Should().Contain("contentApi");
        }

        [Fact]
        public void ItReturnsAnRssFeed()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/news/rss"));

            result.Should().Contain("Stockport Council News Feed");
            result.Should().Contain("Another news article");
            result.Should().Contain("rss version=\"2.0\"");
        }

        [Fact]
        public void ItReturnsARobotsFileForHealthyStockport()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/robots.txt"));

            result.Should().Contain("# no robots");
        }

        #endregion

        #region stockportgov
        [Fact]
        public void ItReturnsRedirectResponseOnPostcodeSearch()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/postcode?postcode=this"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsARobotsFileForStockportGov()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/robots.txt"));

            result.Should().Contain("# no robots");
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/topic/test-topic")]
        [InlineData("/news")]
        [InlineData("/physical-activity")]
        [InlineData("/profile/test-profile")]
        [InlineData("/start/start-page")]
        public void ItReturnsAFooterOnThePage(string url)
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync(url));

            result.Should().Contain("2016 A Council Name");
        }

        [Fact]
        public void ItReturnsAHomepage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find a planning application");
            result.Should().Contain("Tell us about a change");
        }

        [Fact]
        public void RobotosTxtShouldHaveZeroCacheHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_client.GetAsync("/robots.txt"));

            result.Headers.CacheControl.MaxAge.Value.Seconds.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Minutes.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Hours.Should().Be(0);
        }
        #endregion

        private void SetBusinessIdRequestHeader(string businessId)
        {
            _testServerFixture.SetBusinessIdRequestHeader(businessId);
        }
    }
}
