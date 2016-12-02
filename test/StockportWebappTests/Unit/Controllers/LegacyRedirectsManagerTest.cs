using FluentAssertions;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class LegacyRedirectsManagerTest
    {
        private LegacyRedirectsManager _mapper;
        private LegacyUrlRedirects _legacyUrlRedirects;     

        public LegacyRedirectsManagerTest()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary() { { "unittest", new RedirectDictionary() }});
            _mapper = new LegacyRedirectsManager(new BusinessId("unittest"), _legacyUrlRedirects);
        }

        [Fact]
        public void ShouldNotRedirectUrlIfBusinessIdIsNotInLegacyRedirects()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary() { { "unittest", new RedirectDictionary() } });
            var legacyRedirectsManager = new LegacyRedirectsManager(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects);

            var result = legacyRedirectsManager.RedirectUrl(string.Empty);
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldIgnoreWildCardIfMoreSpecificUrlExists()
        {
            SetupRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-different-url");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldUseMostSpecificWildCardWhenTwoWildCardsExist()
        {
            SetupRedirectRule("/another-url-from/a-url-with-wildcard/a-url", "/a-redirected-to-url",
                "/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url",
                "/another-url-from/*", "/yet-another-redirected-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/a-redirected-to-url");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfWildcardValueMatchesPartOfRoute()
        {
            SetupRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url", 
                              "/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here");
   
            var firstResult = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");
            firstResult.Should().Be("/another-redirected-to-url");

            var secondResult = _mapper.RedirectUrl("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");
            secondResult.Should().Be("/another-wildcard-goes-here");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
        {
            SetupRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public void ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
        {
            SetupRedirectRule("/a-url-from/from-this", "/a-url-to");

            var result = _mapper.RedirectUrl("/a-url-from/from-this/a-url");
            result.Should().Be("");
        }

        [Fact]
        public void ShouldRedirectToMatchedPageIfValueFound()
        {
            SetupRedirectRule("/a-url-from/from-this", "/a-url-to", 
                              "/another-url-from/another-from-this", "/another-url-to");

            var firstResult = _mapper.RedirectUrl("/a-url-from/from-this");
            firstResult.Should().Be("/a-url-to");

            var secondResult = _mapper.RedirectUrl("/another-url-from/another-from-this");
            secondResult.Should().Be("/another-url-to");
        }

        [Fact]
        public void ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
        {
            var result = _mapper.RedirectUrl("/no-Url-Matching");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldRedirectToSpecificallyMatchedUrlEvenIfRequestHasASlashOnTheEnd()
        {
            SetupRedirectRule("/a-url-from/from-this", "/a-url-to");
            var result = _mapper.RedirectUrl("/a-url-from/from-this/");

            result.Should().Be("/a-url-to");
        }

        [Fact]
        public void ShouldRedirectToUrlMatchedByWildcardEvenIfRequestHasASlashOnTheEnd()
        {
            SetupRedirectRule("/path/*", "/redirected-url");

            var result = _mapper.RedirectUrl("/path/some-label/");

            result.Should().Be("/redirected-url");
        }

        private void SetupRedirectRule(string rule, string toUrl, string rule2 = "defaultrule2", string toUrl2 = "defaultToUrl", string rule3 = "defaultrule3", string toUrl3 = "defaultToUrl")
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary()
            {
                {
                    "unittest", new RedirectDictionary()
                    {
                        {rule, toUrl},
                        {rule2, toUrl2},
                        {rule3, toUrl3}
                    }
                },
                {"another-business-id", new RedirectDictionary()}
            });
            _mapper = new LegacyRedirectsManager(new BusinessId("unittest"), _legacyUrlRedirects);
        }
    }
}
