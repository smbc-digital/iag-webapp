namespace StockportWebappTests_Unit.Unit.Controllers;

public class SitemapControllerTests
{
    private readonly Mock<IRepository> _mockRepository = new();
    private readonly Mock<ILogger<SitemapController>> _mockLogger = new();

    private readonly SitemapController _controller;

    private readonly Newsroom _newsroom = new(new List<News>(),
        new List<Alert>(),
        false,
        "",
        new List<string>(),
        new List<DateTime>());

    private readonly EventCalendarViewModel _eventCalendar = new(new List<Event>
    {
        new()
        {
            Slug = "slug"
        }
    }, new List<string>());

    private readonly List<ArticleSiteMap> _articlesSiteMap = new();
    private readonly List<Group> _groups = new();
    private readonly List<Showcase> _showcases = new();
    private readonly List<SectionSiteMap> _sections = new() { new SectionSiteMap("slug", DateTime.Now, DateTime.Now)};
    private readonly List<TopicSitemap> _topics = new();
    private readonly List<Profile> _profiles = new();
    private readonly List<Payment> _payments = new();
    private readonly List<StartPage> _startPages = new();

    public SitemapControllerTests()
    {
        var request = new Mock<HttpRequest>();
        var context = new Mock<HttpContext>();
        var headerDictionary = new HeaderDictionary { { "referer", "" } };
        request
            .Setup(r => r.Headers)
            .Returns(headerDictionary);

        context
            .Setup(c => c.Request)
            .Returns(request.Object);

        _mockRepository
            .Setup(repository => repository.Get<Newsroom>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _newsroom, ""));

        _mockRepository
            .Setup(repository => repository.Get<EventCalendarViewModel>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _eventCalendar, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<ArticleSiteMap>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _articlesSiteMap, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<Group>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _groups, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<Showcase>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _showcases, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<SectionSiteMap>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _sections, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<TopicSitemap>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _topics, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<Profile>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _profiles, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<Payment>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _payments, ""));

        _mockRepository
            .Setup(repository => repository.Get<List<StartPage>>("", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _startPages, ""));

        _controller = new(_mockRepository.Object, _mockLogger.Object);
        _controller.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
    }

    [Theory]
    [InlineData("news")]
    [InlineData("events")]
    [InlineData("article")]
    [InlineData("homepage")]
    [InlineData("groups")]
    [InlineData("showcase")]
    [InlineData("section")]
    [InlineData("topic")]
    [InlineData("profile")]
    [InlineData("payment")]
    [InlineData("start")]
    [InlineData("default")]
    public async Task Sitemap_ShouldProcess(string type)
    {
        // Act
        var result = await _controller.Sitemap(type) as ContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Content);
    }
}
