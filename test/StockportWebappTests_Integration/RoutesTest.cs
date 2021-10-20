using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebappTests_Integration
{
    public class RoutesTest : TestAppFactory
    {
        // All requests made using this fake client will instantiate the actual controllers, but
        // when the controllers make requests to ContentApi, the responses will be fake json files from the Unit/MockResponses folder.
        // The routing which maps urls to fake responses is done in FakeHttpClientFactory.FakeHttpClientFactory() (currently in TestAppFactory.cs).
        private readonly HttpClient _fakeClient;

        public RoutesTest(WebApplicationFactory<StockportWebapp.Startup> factory)
            : base(factory)
        {
            _fakeClient = _client;
        }

        [Fact]
        public async Task ItReturnsCorrectHeadersForRedirects()
        {
            SetBusinessIdRequestHeader("unittest");
            // Look, I know this seems odd.
            // We really had no choice, and if you remove this line the test fails when you run it on it's own.
            // Good luck if you want to try to figure out why.
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Look, I know this seems odd.
            // We really had no choice, and if you remove this line the test fails when you run it on it's own.
            // Good luck if you want to try to figure out why.
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var result = await _fakeClient.GetAsync("/this-is-another-article");

            result.Headers.CacheControl.MaxAge.Value.Should().Be(TimeSpan.FromSeconds(21600));
        }

        [Fact]
        public async Task ItGives404ForANonExistentPageWithoutALegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");

            var result = await _fakeClient.GetAsync("/non-existent-url");
             
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ItPerformsARedirectWhenRequestMatchesAnExactLegacyRedirectRule()
        {
            SetBusinessIdRequestHeader("unittest");
            
            // Look, I know this seems odd.
            // We really had no choice, and if you remove this line the test fails when you run it on it's own.
            // Good luck if you want to try to figure out why.
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var result = await _fakeClient.GetAsync("/this-is-a-redirect-from");
 
            result.StatusCode.Should().Be(HttpStatusCode.MovedPermanently);
            result.Headers.Location.ToString().Should().Be("this-is-a-redirect-to");
        }

        #region mixedbusinessids
        [Fact]
        public async Task ItReturnsContentFromTheBusinessRequestedInTheBusinessIdHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var stockportResult = await _fakeClient.GetStringAsync("/");
            stockportResult.Should().Contain("Welcome to Stockport Council");

            SetBusinessIdRequestHeader("healthystockport");

            var healthyResult = await _fakeClient.GetStringAsync("/");

            healthyResult.Should().Contain("Welcome to Healthy Stockport");
            healthyResult.Should().Contain("Eat healthy", "Should render a business-specific piece of content");
        }
        #endregion

        #region healthystockport
        [Fact]
        public async Task ItReturnsAContactUsPageWithTheValidationMessageInInt()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var contactUsMessage = "You filled the form out incorrectly";

            var result = await _fakeClient.GetStringAsync($"/contact-us?message={contactUsMessage}");

            result.Should().Contain(contactUsMessage);
        }

        //[Fact]
        //public async void ItReturnsTheCorrectHeadersForArticles()
        //{
        //    SetBusinessIdRequestHeader("healthystockport");

        //    var result = await _fakeClient.GetAsync("/physical-activity");

        //    result.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromMinutes(15));
        //    result.Headers.CacheControl.Public.Should().Be(true);
        //}

        [Fact]
        public async Task ItReturnsSubItemsInTheHomePage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetStringAsync("/");

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find or renew a book");
            result.Should().Contain("School holiday");
        }

        [Fact]
        public async Task ItReturnsAStartPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetStringAsync("/start/start-page");

            result.Should().Contain("Start Page");
            result.Should().Contain("An upper body");
            result.Should().Contain("Lower body");
        }

        [Fact]
        public async Task ItReturnsATopicWithAlerts()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetStringAsync("/topic/test-topic");

            result.Should().Contain("This is an alert");
            result.Should().Contain("It also has a body text");
        }

        [Fact]
        public async Task ItReturnsAnArticlePageWithFirstSectionOnlyGivenArticleSlug()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var articleSummary = "Being active is great for your body";
            var firstSectionBody = "Staying active and exercising";
            var sectionTwoBody = "Blah blah blah here";

            var result = await _fakeClient.GetStringAsync("/physical-activity");

            result.Should().Contain(articleSummary);
            result.Should().Contain(firstSectionBody);
            result.Should().NotContain(sectionTwoBody);
        }

        [Fact]
        public async Task ItReturnsAnArticlePageAndRendersDocumentInSection()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var documentTitle = "Metroshuttle route map";
            var documentSize = "658 KB";
            var documentUrl = "document.pdf";
            var fileName = "document.pdf";

            var result = await _fakeClient.GetStringAsync("/physical-activity/physical-activity-overview");

            result.Should().Contain(documentTitle);
            result.Should().Contain(documentSize);
            result.Should().Contain(documentUrl);
            result.Should().Contain(fileName);
        }

        [Fact]
        public async Task ItReturnsAnArticlePageWithRequestedSectionGivenArticleAndSectionSlugs()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var requestedSectionBody = "body content";
            var articleSummary = "Being active is great for your body";
            var sectionOneBody = "not in the content";

            var result = await _fakeClient.GetStringAsync("/physical-activity/types-of-physical-activity");

            result.Should().Contain(requestedSectionBody);
            result.Should().NotContain(articleSummary);
            result.Should().NotContain(sectionOneBody);
        }

        [Fact]
        public async Task ItReturnsAContactUsPage()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var formHtmlTag = "<form";

            var result = await _fakeClient.GetStringAsync("/contact-us");

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public async Task ItReturnsThankYouMessageOnSuccessEmail()
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

            var result =await _fakeClient.SendAsync(request);

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
            result.Headers.Location.OriginalString.Should().Be("/thank-you?ReturnUrl=%2Fa-page");
        }

        /*[Fact]
        public void ItReturnsAHealthcheck()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = AsyncTestHelper.Resolve(_fakeClient.GetStringAsync("/_healthcheck"));

            result.Should().Contain("appVersion");
            result.Should().Contain("sha");
            result.Should().Contain("featureToggles");
            result.Should().Contain("dependencies");
            result.Should().Contain("contentApi");
        }*/

        [Fact]
        public async Task ItReturnsAnRssFeed()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetStringAsync("/news/rss");

            result.Should().Contain("Stockport Council News Feed");
            result.Should().Contain("Another news article");
            result.Should().Contain("rss version=\"2.0\"");
        }

        [Fact]
        public async Task ItReturnsARobotsFileForHealthyStockport()
        {
            SetBusinessIdRequestHeader("healthystockport");

            var result = await _fakeClient.GetStringAsync("/robots.txt");

            result.Should().Contain("User-agent: *\r\nDisallow: /");
        }

        #endregion

        #region stockportgov
        [Fact]
        public async Task ItReturnsRedirectResponseOnPostcodeSearch()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetAsync("/postcode?postcode=this");

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task ItReturnsARobotsFileForStockportGov()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/robots.txt");

            result.Should().Contain("User-agent: *\r\nDisallow: /");
        }

        [Fact]
        public async Task ItReturnsAHomepage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/");

            result.Should().Contain("Pay Council Tax");
            result.Should().Contain("Check your bin day");
            result.Should().Contain("Find a planning application");
            result.Should().Contain("Tell us about a change");
        }

        [Fact]
        public async Task RobotosTxtShouldHaveZeroCacheHeader()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetAsync("/robots.txt");

            result.Headers.CacheControl.MaxAge.Value.Seconds.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Minutes.Should().Be(0);
            result.Headers.CacheControl.MaxAge.Value.Hours.Should().Be(0);
        }

        [Fact]
        public async Task ItShouldReturnsEventsCalendar()
        {
            SetBusinessIdRequestHeader("stockportgov");
            
            var result = await _fakeClient.GetStringAsync("/events");

            result.Should().Contain("This is the event");
            result.Should().Contain("This is the second event");
            result.Should().Contain("This is the third event");
        }

        [Fact]
        public async Task ItShouldReturnsEventsCalendarBySlug()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/events/event-of-the-century");

            result.Should().Contain("This is the event");
        }

        [Fact(Skip="some reason ")]
        public async Task ReverseCmsTemplateShouldBeServedForDemocracyWebsiteWithAbsoluteLinks()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/ExternalTemplates/Democracy");

            result.Should().Contain("{pagetitle}");
            result.Should().Contain("{breadcrumb}");
            result.Should().Contain("{content}");
            result.Should().Contain("{sidenav}");
            result.Should().Contain("href=\"https://www.stockport.gov.uk/\"");
            result.Should().Contain("href=\"https://www.stockport.gov.uk/topic/contact-us\"");
        }

        [Fact]
        public async Task ItReturnsAEventSubmissionPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formHtmlTag = "<form";

            var result = await _fakeClient.GetStringAsync("/events/add-your-event");

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public async Task ItReturnsThankYouMessageOnSuccessEventSubmission()
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

            var result = await _fakeClient.SendAsync(request);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ItReturnsAGroupSubmissionPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var formHtmlTag = "<form";

            var result = await _fakeClient.GetStringAsync("/groups/add-a-group");

            result.Should().Contain(formHtmlTag);
        }

        [Fact]
        public async Task ShouldRedirectToThankYouMessageOnSuccessGroupSubmission()
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

            var result = await _fakeClient.SendAsync(request);

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task ShouldReturnToSamePageWhenGroupSubmissionDoneWithoutRequiredFields()
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

            var result = await _fakeClient.SendAsync(request);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ItReturnsAShowcasePage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/showcase/a-showcase");

            result.Should().Contain("test showcase");
        }

        [Fact]
        public async Task ItReturnsAGroupPage()
        {
            SetBusinessIdRequestHeader("stockportgov");
            
            var result = await _fakeClient.GetStringAsync("/groups/test-zumba-slug");

            result.Should().Contain("zumba");
        }
        
        [Fact]
        public async Task ItReturnsAGroupResultsPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/groups/results");

            result.Should().Contain("Brinnington");
        }

        [Fact]
        public async Task ItReturnsAGroupStartPage()
        {
            SetBusinessIdRequestHeader("stockportgov");

            var result = await _fakeClient.GetStringAsync("/groups");

            result.Should().Contain("Dancing");
            result.Should().Contain("group-homepage-title");
        }

        #endregion
    }
}
