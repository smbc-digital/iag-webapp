using FluentAssertions;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class LegacyRedirectsManagerTest
    {
        private readonly LegacyRedirectsMapper _mapper;
        private LegacyUrlRedirects _legacyUrlRedirects;
        private const string BusinessId = "businessId";

        public LegacyRedirectsManagerTest()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary { { BusinessId, new RedirectDictionary() }});
            _mapper = new LegacyRedirectsMapper(new BusinessId(BusinessId), _legacyUrlRedirects);
        }

        [Fact]
        public void ShouldNotRedirectUrlIfBusinessIdIsNotInLegacyRedirects()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary { { BusinessId, new RedirectDictionary() } });
            var legacyRedirectsManager = new LegacyRedirectsMapper(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects);

            var result = legacyRedirectsManager.RedirectUrl(string.Empty);
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldIgnoreWildCardIfMoreSpecificUrlExists()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/a-url", "/a-redirected-to-url");
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/a-redirected-to-url");
        }

        [Fact]
        public void ShouldUseMostSpecificWildCardWhenTwoWildCardsExist()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
            AddRedirectRule("/another-url-from/*", "/yet-another-redirected-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfWildcardValueMatchesPartOfRoute()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
            var firstResult = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");
            firstResult.Should().Be("/another-redirected-to-url");

            AddRedirectRule("/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here");
            var secondResult = _mapper.RedirectUrl("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");
            secondResult.Should().Be("/another-wildcard-goes-here");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");

            var result = _mapper.RedirectUrl("/a-url-from/from-this/a-url");
            result.Should().Be("");
        }

        [Fact]
        public void ShouldRedirectToMatchedPageIfValueFound()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");
            var firstResult = _mapper.RedirectUrl("/a-url-from/from-this");
            firstResult.Should().Be("/a-url-to");

            AddRedirectRule("/another-url-from/another-from-this", "/another-url-to");
            var secondResult = _mapper.RedirectUrl("/another-url-from/another-from-this");
            secondResult.Should().Be("/another-url-to");
        }

        [Fact]
        public void ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
        {
            var result = _mapper.RedirectUrl("/no-url-Matching");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldRedirectToSpecificallyMatchedUrlEvenIfRequestHasASlashOnTheEnd()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");
            var result = _mapper.RedirectUrl("/a-url-from/from-this/");
            result.Should().Be("/a-url-to");
        }

        [Fact]
        public void ShouldRedirectToUrlMatchedByWildcardEvenIfRequestHasASlashOnTheEnd()
        {
            AddRedirectRule("/path/*", "/redirected-url");
            var result = _mapper.RedirectUrl("/path/some-label/");
            result.Should().Be("/redirected-url");
        }

        private void AddRedirectRule(string rule, string toUrl)
        {
            _legacyUrlRedirects.Redirects[BusinessId].Add(rule, toUrl);
        }
    }
}
