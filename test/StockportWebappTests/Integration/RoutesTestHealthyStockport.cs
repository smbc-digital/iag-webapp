using FluentAssertions;
using Xunit;
using HttpClient = System.Net.Http.HttpClient;
using System;
using System.Net;
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
            TestContentApiFixture.SetupContentApiResponses();

            _server = TestAppFactory.MakeFakeApp("healthystockport", IntEnvironment);
            _client = _server.CreateClient();
            SetBusinessIdRequestHeader("healthystockport");
        }

        [Fact]
        public void ItReturnsContentFromTheBusinessRequestedInTheBusinessIdHeader()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var stockportResult = AsyncTestHelper.Resolve(Client().GetStringAsync("/"));
            stockportResult.Should().Contain("Welcome to Stockport Council");

            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "healthystockport");
            SetBusinessIdRequestHeader("healthystockport");

            var healthyResult = AsyncTestHelper.Resolve(Client().GetStringAsync("/"));

            healthyResult.Should().Contain("Welcome to Healthy Stockport");
            healthyResult.Should().Contain("Eat healthy", "Should render a business-specific piece of content");

        }

        private HttpClient Client()
        {
            return _client;
        }

        [Fact]
        public void ItReturnsPopularSearchTermsOnTheHomepage()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/"));

            result.Should().Contain("/search?query=popular search term");
        }

        [Fact]
        public void ItReturnsAContactUsPageWithTheValidationMessageInInt()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "healthystockport");

            var contactUsMessage = "You filled the form out incorrectly";

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync($"/contact-us?message={contactUsMessage}"));

            result.Should().Contain(contactUsMessage);
        }

        [Fact]
        public void ItReturnsRedirectResponseOnSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetAsync("/search?something=something"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsRedirectResponseOnPostcodeSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetAsync("/postcode?postcode=this"));

            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItRedirectsToExistingStockportGovSearch()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetAsync("/search?query=hello"));

            result.Headers.Location.Should().Be("http://stockport.searchimprove.com/search.aspx?pc=&pckid=816028173&aid=448530&pt=6018936&addid=&sw=hello");
            result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public void ItReturnsARobotsFileForStockportGov()
        {
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync("/robots.txt"));

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
            SwitchEnvironmentIncludingBusinessIdEnvVar(IntEnvironment, "stockportgov");
            SetBusinessIdRequestHeader("stockportgov");

            var result = AsyncTestHelper.Resolve(Client().GetStringAsync(url));

            result.Should().Contain("2016 A Council Name");
        }

        private void SwitchEnvironmentIncludingBusinessIdEnvVar(string environment, string businessId)
        {
            _server = TestAppFactory.MakeFakeApp(businessId, environment);
            _client = _server.CreateClient();
            SetBusinessIdRequestHeader(businessId);
        }

        private void SetBusinessIdRequestHeader(string businessId)
        {
            _client.DefaultRequestHeaders.Remove("BUSINESS-ID");
            _client.DefaultRequestHeaders.Add("BUSINESS-ID", businessId);
        }

        public void Dispose()
        {
            Client().Dispose();
            _server.Dispose();
        }
    }
}
