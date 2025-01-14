namespace StockportWebappTests_Unit.Unit.Utils;

public class UrlGeneratorTest
{
    private readonly UrlGenerator _urlGenerator;

    public UrlGeneratorTest()
    {
        Uri contentConfig = new("http://localhost.com:80/");

        Mock<IApplicationConfiguration> config = new();
        config
            .Setup(conf => conf.GetContentApiUri())
            .Returns(contentConfig);

        config
            .Setup(conf => conf.GetContentApiUrlRoot())
            .Returns(contentConfig);

        BusinessId businessId = new("test-id");

        _urlGenerator = new(config.Object, businessId);
    }

    [Fact]
    public void ItReturnsUrlForTopicRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<Topic>("topic-slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/topics/topic-slug", url);
    }

    [Fact]
    public void ItReturnsUrlForArticleRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<Article>("topic-slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/articles/topic-slug", url);
    }

    [Fact]
    public void ItReturnsUrlForProfileRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<Profile>("topic-slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/profiles/topic-slug", url);
    }

    [Fact]
    public void ItReturnsUrlForStartPageRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<StartPage>("start-page-slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/start-page/start-page-slug", url);
    }

    [Fact]
    public void ItReturnsUrlForNewsRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<News>("news-slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/news/news-slug", url);
    }

    [Fact]
    public void ItReturnsUrlForNewsListingRequest()
    {
        // Act
        string url = _urlGenerator.UrlFor<Newsroom>();

        // Assert
        Assert.Equal("http://localhost.com/test-id/news", url);
    }

    [Fact]
    public void ItReturnsUrlForNewsListingRequestWithTagQuery()
    {
        // Act
        string url = _urlGenerator.UrlFor<Newsroom>(queries: new List<Query>() { new("tag", "Events") });

        // Assert
        Assert.Equal("http://localhost.com/test-id/news?tag=Events", url);
    }

    [Fact]
    public void ItReturnsUrlForNewsLatestRequest()
    {
        // Act
        string url = _urlGenerator.UrlForLimit<List<News>>(2);

        // Assert
        Assert.Equal("http://localhost.com/test-id/news/latest/2", url);
    }

    [Fact]
    public void ItReturnsUrlForFooter()
    {
        // Act
        string url = _urlGenerator.UrlFor<Footer>();

        // Assert
        Assert.Equal("http://localhost.com/test-id/footer", url);
    }

    [Fact]
    public void ItReturnsUrlForEventCalendar()
    {
        // Act
        string url = _urlGenerator.UrlFor<EventCalendar>();

        // Assert
        Assert.Equal("http://localhost.com/test-id/events", url);
    }

    [Fact]
    public void ItReturnsUrlForEvent()
    {
        // Act
        string url = _urlGenerator.UrlFor<Event>("slug");
        
        // Assert
        Assert.Equal("http://localhost.com/test-id/events/slug", url);
    }

    [Fact]
    public void ItReturnsUrlForLatestEventRequest()
    {
        // Act
        string url = _urlGenerator.UrlForLimit<EventCalendar>(2);

        // Assert
        Assert.Equal("http://localhost.com/test-id/events/latest/2", url);
    }

    [Fact]
    public void ItReturnsUrlShowcaseRequestWithSlug()
    {
        // Act
        string url = _urlGenerator.UrlFor<Showcase>("slug");

        // Assert
        Assert.Equal("http://localhost.com/test-id/showcases/slug", url);
    }

    [Fact]
    public void ItReturnsUrlForHealthcheckRequest()
    {
        // Act
        string url = _urlGenerator.HealthcheckUrl();

        // Assert
        Assert.Equal("http://localhost.com/_healthcheck", url);
    }
}