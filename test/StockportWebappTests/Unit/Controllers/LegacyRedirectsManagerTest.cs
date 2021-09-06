using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class LegacyRedirectsManagerTest
    {
        private readonly LegacyRedirectsMapper _mapper;
        private LegacyUrlRedirects _legacyUrlRedirects;
        private ShortUrlRedirects _shortUrlRedirects = new ShortUrlRedirects(new BusinessIdRedirectDictionary());
        private Mock<IRepository> _mockRepository = new();
        private const string BusinessId = "businessId";

        public LegacyRedirectsManagerTest()
        {
            _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary { { BusinessId, new RedirectDictionary() }});
            _legacyUrlRedirects.LastUpdated = DateTime.Now;
            _mapper = new LegacyRedirectsMapper(new BusinessId(BusinessId), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);
        }

        [Fact]
        public async Task ShouldNotRedirectUrlIfBusinessIdIsNotInLegacyRedirects()
        {
            var legacyRedirectsManager = new LegacyRedirectsMapper(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);

            var result = await legacyRedirectsManager.RedirectUrl(string.Empty);
            result.Should().Be(string.Empty);
        }

        [Fact]
        public async Task ShouldNotCallRepository_IfCacheNotExpired()
        {
            await _mapper.RedirectUrl(string.Empty);
            _mockRepository.Verify(_ => _.GetRedirects(), Times.Never);
        }

        [Fact]
        public async Task ShouldCallRepository_IfCacheExpired()
        {
            _mockRepository
                .Setup(_ => _.GetRedirects())
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse(200, new Redirects(new BusinessIdRedirectDictionary(), new BusinessIdRedirectDictionary()), string.Empty));
            _legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 45, 0));
            var legacyRedirectsManager = new LegacyRedirectsMapper(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);

            await legacyRedirectsManager.RedirectUrl(string.Empty);
            _mockRepository.Verify(_ => _.GetRedirects(), Times.Once);
        }

        [Fact]
        public async Task ShouldIgnoreWildCardIfMoreSpecificUrlExists()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/a-url", "/a-redirected-to-url");
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/a-redirected-to-url");
        }

        [Fact]
        public async Task ShouldUseMostSpecificWildCardWhenTwoWildCardsExist()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
            AddRedirectRule("/another-url-from/*", "/yet-another-redirected-url");

            var result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public async Task ShouldRedirectToMatchedUrlIfWildcardValueMatchesPartOfRoute()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
            var firstResult = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");
            firstResult.Should().Be("/another-redirected-to-url");

            AddRedirectRule("/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here");
            var secondResult = await _mapper.RedirectUrl("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");
            secondResult.Should().Be("/another-wildcard-goes-here");
        }

        [Fact]
        public async Task ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
        {
            AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

            var result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard");
            result.Should().Be("/another-redirected-to-url");
        }

        [Fact]
        public async Task ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");

            var result = await _mapper.RedirectUrl("/a-url-from/from-this/a-url");
            result.Should().Be("");
        }

        [Fact]
        public async Task ShouldRedirectToMatchedPageIfValueFound()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");
            var firstResult = await _mapper.RedirectUrl("/a-url-from/from-this");
            firstResult.Should().Be("/a-url-to");

            AddRedirectRule("/another-url-from/another-from-this", "/another-url-to");
            var secondResult = await _mapper.RedirectUrl("/another-url-from/another-from-this");
            secondResult.Should().Be("/another-url-to");
        }

        [Fact]
        public async Task ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
        {
            var result = await _mapper.RedirectUrl("/no-url-Matching");
            result.Should().Be(string.Empty);
        }

        [Fact]
        public async Task ShouldRedirectToSpecificallyMatchedUrlEvenIfRequestHasASlashOnTheEnd()
        {
            AddRedirectRule("/a-url-from/from-this", "/a-url-to");
            var result = await _mapper.RedirectUrl("/a-url-from/from-this/");
            result.Should().Be("/a-url-to");
        }

        [Fact]
        public async Task ShouldRedirectToUrlMatchedByWildcardEvenIfRequestHasASlashOnTheEnd()
        {
            AddRedirectRule("/path/*", "/redirected-url");
            var result = await _mapper.RedirectUrl("/path/some-label/");
            result.Should().Be("/redirected-url");
        }

        private void AddRedirectRule(string rule, string toUrl)
        {
            _legacyUrlRedirects.Redirects[BusinessId].Add(rule, toUrl);
        }
    }
}
