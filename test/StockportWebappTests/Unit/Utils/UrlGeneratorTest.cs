using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class UrlGeneratorTest
    {
        private readonly UrlGenerator _urlGenerator;

        public UrlGeneratorTest()
        {
            var contentConfig = new Uri("http://content.com:80/api/");

            var config = new Mock<IApplicationConfiguration>();
            config.Setup(o => o.GetContentApiUri()).Returns(contentConfig);

            var businessId = new BusinessId("test-id");

            _urlGenerator = new UrlGenerator(config.Object, businessId);
        }

        [Fact]
        public void ItReturnsUrlForTopicRequest()
        {
            var topicSlug = "topic-slug";
            var url = _urlGenerator.UrlFor<Topic>(topicSlug);

            url.Should().Be($"http://content.com/api/test-id/topic/{topicSlug}");
        }

        [Fact]
        public void ItReturnsUrlForArticleRequest()
        {
            const string articleSlug = "topic-slug";
            var url = _urlGenerator.UrlFor<Article>(articleSlug);

            url.Should().Be($"http://content.com/api/test-id/article/{articleSlug}");
        }

        [Fact]
        public void ItReturnsUrlForProfileRequest()
        {
            const string profileSlug = "topic-slug";
            var url = _urlGenerator.UrlFor<Profile>(profileSlug);

            url.Should().Be($"http://content.com/api/test-id/profile/{profileSlug}");
        }

        [Fact]
        public void ItReturnsUrlForStartPageRequest()
        {
            const string startPageSlug = "topic-slug";
            var url = _urlGenerator.UrlFor<StartPage>(startPageSlug);

            url.Should().Be($"http://content.com/api/test-id/start-page/{startPageSlug}");
        }

        [Fact]
        public void ItReturnsUrlForNewsRequest()
        {
            const string slug = "news-slug";
            var url = _urlGenerator.UrlFor<News>(slug);

            url.Should().Be($"http://content.com/api/test-id/news/{slug}");
        }

        [Fact]
        public void ItReturnsUrlForNewsListingRequest()
        {
            var url = _urlGenerator.UrlFor<Newsroom>();

            url.Should().Be("http://content.com/api/test-id/news");
        }

        [Fact]
        public void ItReturnsUrlForNewsListingRequestWithTagQuery()
        {
            var url = _urlGenerator.UrlFor<Newsroom>(queries: new List<Query>() { new Query("tag", "Events") });

            url.Should().Be("http://content.com/api/test-id/news?tag=Events");
        }

        [Fact]
        public void ItReturnsUrlForNewsLatestRequest()
        {
            var url = _urlGenerator.UrlFor<List<News>>("2");

            url.Should().Be("http://content.com/api/test-id/news/latest/2");
        }

        [Fact]
        public void ItReturnsUrlForFooter()
        {
            var url = _urlGenerator.UrlFor<Footer>();

            url.Should().Be("http://content.com/api/test-id/footer");
        }

        [Fact]
        public void ItReturnsUrlForHealthcheckRequest()
        {
            var url = _urlGenerator.HealthcheckUrl();

            url.Should().Be("http://content.com/_healthcheck");
        }

        [Fact]
        public void ItReturnsUrlForRedirectsRequest()
        {
            var url = _urlGenerator.RedirectUrl();

            url.Should().Be("http://content.com/api/redirects");
        }
    }
}