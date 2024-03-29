﻿namespace StockportWebappTests_Unit.Unit.Utils;

public class UrlGeneratorTest
{
    private readonly UrlGenerator _urlGenerator;

    public UrlGeneratorTest()
    {
        var contentConfig = new Uri("http://localhost.com:80/");

        var config = new Mock<IApplicationConfiguration>();
        config.Setup(o => o.GetContentApiUri()).Returns(contentConfig);

        config.Setup(o => o.GetContentApiUrlRoot()).Returns(contentConfig);

        var businessId = new BusinessId("test-id");

        _urlGenerator = new UrlGenerator(config.Object, businessId);
    }

    [Fact]
    public void ItReturnsUrlForTopicRequest()
    {
        var topicSlug = "topic-slug";
        var url = _urlGenerator.UrlFor<Topic>(topicSlug);

        url.Should().Be($"http://localhost.com/test-id/topics/{topicSlug}");
    }

    [Fact]
    public void ItReturnsUrlForArticleRequest()
    {
        const string articleSlug = "topic-slug";
        var url = _urlGenerator.UrlFor<Article>(articleSlug);

        url.Should().Be($"http://localhost.com/test-id/articles/{articleSlug}");
    }

    [Fact]
    public void ItReturnsUrlForProfileRequest()
    {
        const string profileSlug = "topic-slug";
        var url = _urlGenerator.UrlFor<Profile>(profileSlug);

        url.Should().Be($"http://localhost.com/test-id/profiles/{profileSlug}");
    }

    [Fact]
    public void ItReturnsUrlForStartPageRequest()
    {
        const string startPageSlug = "topic-slug";
        var url = _urlGenerator.UrlFor<StartPage>(startPageSlug);

        url.Should().Be($"http://localhost.com/test-id/start-page/{startPageSlug}");
    }

    [Fact]
    public void ItReturnsUrlForNewsRequest()
    {
        const string slug = "news-slug";
        var url = _urlGenerator.UrlFor<News>(slug);

        url.Should().Be($"http://localhost.com/test-id/news/{slug}");
    }

    [Fact]
    public void ItReturnsUrlForNewsListingRequest()
    {
        var url = _urlGenerator.UrlFor<Newsroom>();

        url.Should().Be("http://localhost.com/test-id/news");
    }

    [Fact]
    public void ItReturnsUrlForNewsListingRequestWithTagQuery()
    {
        var url = _urlGenerator.UrlFor<Newsroom>(queries: new List<Query>() { new Query("tag", "Events") });

        url.Should().Be("http://localhost.com/test-id/news?tag=Events");
    }

    [Fact]
    public void ItReturnsUrlForNewsLatestRequest()
    {
        var url = _urlGenerator.UrlForLimit<List<News>>(2);

        url.Should().Be("http://localhost.com/test-id/news/latest/2");
    }

    [Fact]
    public void ItReturnsUrlForFooter()
    {
        var url = _urlGenerator.UrlFor<Footer>();

        url.Should().Be("http://localhost.com/test-id/footer");
    }

    [Fact]
    public void ItReturnsUrlForEventCalendar()
    {
        var url = _urlGenerator.UrlFor<EventCalendar>();

        url.Should().Be("http://localhost.com/test-id/events");
    }

    [Fact]
    public void ItReturnsUrlForEvent()
    {
        var url = _urlGenerator.UrlFor<Event>("slug");

        url.Should().Be("http://localhost.com/test-id/events/slug");
    }

    [Fact]
    public void ItReturnsUrlForLatestEventRequest()
    {
        var url = _urlGenerator.UrlForLimit<EventCalendar>(2);

        url.Should().Be("http://localhost.com/test-id/events/latest/2");
    }

    [Fact]
    public void ItReturnsUrlShowcaseRequestWithSlug()
    {
        var url = _urlGenerator.UrlFor<Showcase>("slug");

        url.Should().Be("http://localhost.com/test-id/showcases/slug");
    }

    [Fact]
    public void ItReturnsUrlForHealthcheckRequest()
    {
        var url = _urlGenerator.HealthcheckUrl();

        url.Should().Be("http://localhost.com/_healthcheck");
    }
}