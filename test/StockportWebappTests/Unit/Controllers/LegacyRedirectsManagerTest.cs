using FluentAssertions;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class LegacyRedirectsManagerTest
    {
        private readonly LegacyRedirectsManager _legacyRedirectsManager;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;     

        public LegacyRedirectsManagerTest()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary()
            {
                { "unittest", new RedirectDictionary()
                {
                    { "/a-url-from/from-this", "/a-url-to" },
                    { "/another-url-from/another-from-this", "/another-url-to" },                   
                    { "/another-url-from/a-url-with-wildcard/a-url", "/a-redirected-to-url" },
                    { "/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url" },
                    { "/another-url-from/*", "/yet-another-redirected-url" },
                    { "/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here" }
                } },
                { "another-business-id", new RedirectDictionary() }
            });
            _legacyRedirectsManager = new LegacyRedirectsManager(new BusinessId("unittest"), _legacyUrlRedirects);
        }

        [Fact]
        public void ShouldNotRedirectUrlIfBusinessIdIsNotInLegacyRedirects()
        {
            var legacyRedirectsManager = new LegacyRedirectsManager(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects);

            var result = legacyRedirectsManager.RedirectUrl(string.Empty);
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldIgnoreWildCardIfMoreSpecificUrlExists()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/another-url-from/a-url-with-wildcard/a-different-url");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldUseMostSpecificWildCardWhenTwoWildCardsExist()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/a-redirected-to-url");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfWildcardValueMatchesPartOfRoute()
        {
            var firstResult = _legacyRedirectsManager.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");
            firstResult.Should().Be("/another-redirected-to-url");

            var secondResult = _legacyRedirectsManager.RedirectUrl("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");
            secondResult.Should().Be("/another-wildcard-goes-here");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/another-url-from/a-url-with-wildcard");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/a-url-from/from-this/a-url");
            result.Should().Be("");
        }

        [Fact]
        public void ShouldRedirectToMatchedPageIfValueFound()
        {
            var firstResult = _legacyRedirectsManager.RedirectUrl("/a-url-from/from-this");
            firstResult.Should().Be("/a-url-to");

            var secondResult = _legacyRedirectsManager.RedirectUrl("/another-url-from/another-from-this");
            secondResult.Should().Be("/another-url-to");
        }

        [Fact]
        public void ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/no-Url-Matching");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldRedirectToSpecificallyMatchedUrlEvenIfRequestHasASlashOnTheEnd()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/a-url-from/from-this/");

            result.Should().Be("/a-url-to");
        }

        [Fact]
        public void ShouldRedirectToUrlMatchedByWildcardEvenIfRequestHasASlashOnTheEnd()
        {
            var result = _legacyRedirectsManager.RedirectUrl("/another-url-from/a-url-with-wildcard/some-label/");

            result.Should().Be("/another-redirected-to-url");
        }
    }
}
