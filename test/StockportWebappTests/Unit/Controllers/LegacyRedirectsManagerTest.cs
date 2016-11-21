using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class LegacyRedirectsManagerTest
    {
        private readonly LegacyRedirectsManager _legacyRedirectsManager;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;     

        public LegacyRedirectsManagerTest()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary()
            {
                { "unittest", new RedirectDictionary()
                {
                    { "/a-url-from/from-this", "/a-url-to" },
                    { "/another-url-from/another-from-this", "/another-url-to" },
                    { "/another-url-from/a-url-with-wildcard/*", "/wildcard-goes-here" },
                    { "/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here" }
                } },
                { "another-business-id", new RedirectDictionary() }
            });
            _legacyRedirectsManager = new LegacyRedirectsManager(_httpContextAccessor.Object, new BusinessId("unittest"), _legacyUrlRedirects);
        }

        [Fact]
        public void ShouldNotRedirectUrlIfBusienssIdIsNotInLegacyRedirects()
        {
            var legacyRedirectsManager = new LegacyRedirectsManager(_httpContextAccessor.Object, new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects);

            var result = legacyRedirectsManager.RedirectUrl();
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfWildacrdValueMatchesPartOfRoute()
        {
             _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");

            var firstResult = _legacyRedirectsManager.RedirectUrl();
            firstResult.Should().Be("/wildcard-goes-here");

            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");

            var secondResult = _legacyRedirectsManager.RedirectUrl();
            secondResult.Should().Be("/another-wildcard-goes-here");
        }

        [Fact]
        public void ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
        {
            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                .Returns("/another-url-from/a-url-with-wildcard");

            var result = _legacyRedirectsManager.RedirectUrl();
            result.Should().Be("/wildcard-goes-here");
        }

        [Fact]
        public void ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
        {
            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                .Returns("/a-url-from/from-this/a-url");

            var result = _legacyRedirectsManager.RedirectUrl();
            result.Should().Be("");
        }

        [Fact]
        public void ShouldRedirectToMatchedPageIfValueFound()
        {
            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns("/a-url-from/from-this");

            var firstResult = _legacyRedirectsManager.RedirectUrl();
            firstResult.Should().Be("/a-url-to");

            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns("/another-url-from/another-from-this");

            var secondResult = _legacyRedirectsManager.RedirectUrl();
            secondResult.Should().Be("/another-url-to");
        }

        [Fact]
        public void ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
        {
            _httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns("/no-Url-Matching");

            var result = _legacyRedirectsManager.RedirectUrl();
            result.Should().Be(string.Empty);
        }
    }
}
