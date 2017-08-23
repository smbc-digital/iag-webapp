using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Moq;
using HttpClient = System.Net.Http.HttpClient;
using System.Net;
using System.Net.Http;

namespace StockportWebappTests.Integration
{
    public class RoutesTest : IClassFixture<RoutesTestServerFixture>
    {
        // All requests made using this fake client will instantiate the actual controllers, but
        // when the controllers make requests to ContentApi, the responses will be fake json files from the Unit/MockResponses folder.
        // The routing which maps urls to fake responses is done in FakeHttpClientFactory.FakeHttpClientFactory() (currently in TestAppFactory.cs).
        private readonly HttpClient _fakeClient;
        private readonly RoutesTestServerFixture _testServerFixture;
       
        public RoutesTest(RoutesTestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _fakeClient = _testServerFixture.Client;          
        }

        [Fact]
        public void ItReturnsCorrectHeadersForRedirects()
        {
            SetBusinessIdRequestHeader("unittest");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetAsync("/this-is-another-article"));

            result.Headers.CacheControl.MaxAge.Value.Should().Be(TimeSpan.FromSeconds(21600));
        }

        [Fact]
        public void ItGives404ForANonExistentPageWithoutALegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetAsync("/non-existent-url"));
             
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public void ItPerformsARedirectWhenRequestMatchesAnExactLegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");
 
            var result = AsyncTestHelper.Resolve(_fakeClient.GetAsync("/this-is-a-redirect-from"));
 
            result.StatusCode.Should().Be(HttpStatusCode.MovedPermanently);
            result.Headers.Location.ToString().Should().Be("this-is-a-redirect-to");
        }

        #region mixedbusinessids
        [Fact]
        public void ItReturnsContentFromTheBusinessRequestedInTheBusinessIdHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var stockportResult = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/"));
            stockportResult.Should().Contain("Welcome to Stockport Council");

            SetBusinessIdRequestHeader("healthystockport");

            var healthyResult = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync($"/contact-us?message={contactUsMessage}"));

            result.Should().Contain(contactUsMessage);
        }

        [Fact]
        public async void ItReturnsTheCorrectHeadersForArticles()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetAsync("/physical-activity");

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(15));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public void ItReturnsSubItemsInTheHomePage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find or renew a book");
            result.Should().Contain("School holiday");
        }

        [Fact]
        public void ItReturnsAStartPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/start/start-page"));

            result.Should().Contain("Start Page");
            result.Should().Contain("An upper body");
            result.Should().Contain("Lower body");
        }

        [Fact]
        public void ItReturnsATopicWithAlerts()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/topic/test-topic"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/physical-activity"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/physical-activity/test-profile-section"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/physical-activity/physical-activity-overview"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/about"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/physical-activity/types-of-physical-activity"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/profile/test-profile"));

            result.Should().Contain(requestedTitle);
            result.Should().Contain(requestedBody);
        }

        [Fact]
        public void ItReturnsAContactUsPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var formHtmlTag = "<form";

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/contact-us"));

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
                new KeyValuePair<string, string>("ServiceEmailId", "test-email"),
                new KeyValuePair<string, string>("g-recaptcha-response", "test")
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/contact-us") { Content = formContents };
            request.Headers.Add("Referer", "http://something.com/a-page");

            var result = AsyncTestHelper.Resolve(_fakeClient.SendAsync(request ));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.OriginalString.Should().Be("/thank-you?referer=%2Fa-page");
        }

        [Fact]
        public void ItReturnsAHealthcheck()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/_healthcheck"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/news/rss"));

            result.Should().Contain("Stockport Council News Feed");
            result.Should().Contain("Another news article");
            result.Should().Contain("rss version=\"2.0\"");
        }

        [Fact]
        public void ItReturnsARobotsFileForHealthyStockport()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/robots.txt"));

            result.Should().Contain("# no robots");
        }

        #endregion

        #region stockportgov
        [Theory]
        [InlineData("/", 30)]
        [InlineData("/topic/test-topic", 30)]
        [InlineData("/physical-activity", 15)]
        [InlineData("/profile/test-profile", 60)]
        [InlineData("/start/start-page", 15)]
        [InlineData("/news/test", 15)]
        [InlineData("/news", 15)]
        [InlineData("/events", 30)]
        [InlineData("/events/event-of-the-century", 30)]
        [InlineData("/atoz/a", 60)]
        [InlineData("/showcase/a-showcase", 30)]
        [InlineData("/smart/smart-test", 15)]
        public async void ItReturnsTheCorrectHeaders(string path, int time)
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetAsync(path);

            result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(time));
            result.Headers.CacheControl.Public.Should().Be(true);
        }

        [Fact]
        public void ItReturnsRedirectResponseOnPostcodeSearch()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetAsync("/postcode?postcode=this"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsARobotsFileForStockportGov()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/robots.txt"));

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

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync(url));

            result.Should().Contain("2016 A Council Name");
        }

        [Fact]
        public void ItReturnsAHomepage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/"));

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find a planning application");
            result.Should().Contain("Tell us about a change");
        }

        [Fact]
        public void RobotosTxtShouldHaveZeroCacheHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetAsync("/robots.txt"));

            result.Headers.CacheControl.MaxAge.Value.Seconds.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Minutes.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Hours.Should().Be(0);
        }

        [Fact]
        public void ItShouldReturnsEventsCalendar()
        {
            SetBusinessIdRequestHeader("stockportgov");
            
            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/events"));

            result.Should().Contain("This is the event");
            result.Should().Contain("This is the second event");
            result.Should().Contain("This is the third event");
        }

        [Fact]
        public void ItShouldReturnsEventsCalendarBySlug()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/events/event-of-the-century"));

            result.Should().Contain("This is the event");
        }

        public void ReverseCmsTemplateShouldBeServedForDemocracyWebsiteWithAbsoluteLinks()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/ExternalTemplates/Democracy"));

            result.Should().Contain("{pagetitle}");
            result.Should().Contain("{breadcrumb}");
            result.Should().Contain("{content}");
            result.Should().Contain("{sidenav}");
            result.Should().Contain("href=\"https://www.stockport.gov.uk/\"");
            result.Should().Contain("href=\"https://www.stockport.gov.uk/topic/contact-us\"");
        }

        [Fact]
        public void ItReturnsAEventSubmissionPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formHtmlTag = "<form";

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/events/add-your-event"));

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public void ItReturnsThankYouMessageOnSuccessEventSubmission()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formContents = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Title", "title"),
                new KeyValuePair<string, string>("EventDate", "11/12/2020"),
                new KeyValuePair<string, string>("EndDate", "12/12/2020"),
                new KeyValuePair<string, string>("StartTime", "09:30"),
                new KeyValuePair<string, string>("EndTime", "17:30"),
                new KeyValuePair<string, string>("Frequency","Daily"),
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/events/add-your-event") { Content = formContents };

            var result = AsyncTestHelper.Resolve(_fakeClient.SendAsync(request));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public void ItReturnsAGroupSubmissionPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formHtmlTag = "<form";

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/groups/add-a-group"));

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public void ShouldRedirectToThankYouMessageOnSuccessGroupSubmission()
        {
            SetBusinessIdRequestHeader("stockportgov");         

            var formContents = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Name", "name"),                
                new KeyValuePair<string, string>("Description", "description"),
                new KeyValuePair<string, string>("CategoriesList", "Dancing"),
                new KeyValuePair<string, string>("Address", "address"),
                new KeyValuePair<string, string>("Email", "email@gmail.com"),
                new KeyValuePair<string, string>("PhoneNumber", "1234"),
                new KeyValuePair<string, string>("Website","http://www.group.org.uk"),
                new KeyValuePair<string, string>("g-recaptcha-response", "test")
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/groups/add-a-group") { Content = formContents };

            var result = AsyncTestHelper.Resolve(_fakeClient.SendAsync(request));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ShouldReturnToSamePageWhenGroupSubmissionDoneWithoutRequiredFields()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formContents = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Name", "name"),
                new KeyValuePair<string, string>("Description", "description"),
                new KeyValuePair<string, string>("CategoriesList", "Dancing"),
                new KeyValuePair<string, string>("Address", "address"),
                new KeyValuePair<string, string>("Email", "email@gmail.com"),            
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "/groups/add-a-group") { Content = formContents };

            var result = AsyncTestHelper.Resolve(_fakeClient.SendAsync(request));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ItReturnsAShowcasePage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/showcase/a-showcase"));

            result.Should().Contain("test showcase");
        }

        [Fact]
        public void ItReturnsAGroupPage()
        {
            SetBusinessIdRequestHeader("stockportgov");
            
            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/groups/test-zumba-slug"));

            result.Should().Contain("zumba");
        }
        
        [Fact]
        public void ItReturnsAGroupResultsPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/groups/results"));

            result.Should().Contain("Brinnington");
        }

        [Fact]
        public void ItReturnsAGroupStartPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/groups"));

            result.Should().Contain("Dancing");
        }

        #endregion

        private void SetBusinessIdRequestHeader(string businessId)
        {
            _testServerFixture.SetBusinessIdRequestHeader(businessId);
        }
    }
}
