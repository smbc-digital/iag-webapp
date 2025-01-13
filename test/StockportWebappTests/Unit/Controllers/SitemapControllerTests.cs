namespace StockportWebappTests_Unit.Unit.Controllers;

public class SitemapControllerTests
{
    private readonly SitemapController _controller;
    private readonly Mock<IRepository> _mockRepository = new();
    private readonly Mock<ILogger<SitemapController>> _mockLogger = new();

    private readonly Newsroom _newsroom = new(new List<News>(),
                                            new List<Alert>(),
                                            false,
                                            string.Empty,
                                            new List<string>(),
                                            new List<DateTime>());

    private readonly EventCalendar _eventCalendar = new(new List<Event>
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
        Mock<HttpRequest> request = new();
        Mock<HttpContext> context = new();
        HeaderDictionary headerDictionary = new() { { "referer", string.Empty } };
        
        request
            .Setup(req => req.Headers)
            .Returns(headerDictionary);

        context
            .Setup(c => c.Request)
            .Returns(request.Object);

        _mockRepository
            .Setup(repository => repository.Get<Newsroom>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _newsroom, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<EventCalendar>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _eventCalendar, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<ArticleSiteMap>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _articlesSiteMap, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<Group>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _groups, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<Showcase>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _showcases, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<SectionSiteMap>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _sections, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<TopicSitemap>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _topics, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<Profile>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _profiles, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<Payment>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _payments, string.Empty));

        _mockRepository
            .Setup(repository => repository.Get<List<StartPage>>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, _startPages, string.Empty));

        _controller = new(_mockRepository.Object, _mockLogger.Object);
        _controller.ControllerContext = new(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
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
        ContentResult result = await _controller.Sitemap(type) as ContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Content);
    }
}