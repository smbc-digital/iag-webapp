namespace StockportWebappTests_Unit.Unit.Controllers;

public class StartPageControllerTest
{
    private readonly StartPageController _controller;
    private readonly Mock<IProcessedContentRepository> _repository;

    public StartPageControllerTest()
    {
        // declarations
        _repository = new Mock<IProcessedContentRepository>();

        // data
        var alerts = new List<Alert> { new Alert("title", "subHeading", "body", "severity", new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty) };
        // data
        var inlineAlerts = new List<Alert> { new Alert("title", "subHeading", "body", "severity", new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty) };

        var startPage = new ProcessedStartPage(
            "start-page",
            "Start Page",
            "this is a teaser",
            "This is a summary",
            "<p>An upper body</p>\n",
            "Start now",
            "http://start.com",
            "<p>Lower body</p>\n",
            new List<Crumb>
            {
                new Crumb("title", "slug", "type")
            },
            "image.jpg",
            "icon",
            alerts
        );

        // setup mocks
        _repository.Setup(o => o.Get<StartPage>("start-page", null)).ReturnsAsync(new HttpResponse(200, startPage, string.Empty));
        _repository.Setup(o => o.Get<StartPage>("doesnt-exist", null)).ReturnsAsync(new HttpResponse(404, null, "No start-page found for 'doesnt-exist'"));

        // objects
        _controller = new StartPageController(_repository.Object);
    }

    [Fact]
    public async Task GetAStartPage()
    {
        var indexPage = await _controller.Index("start-page") as ViewResult;
        var result = indexPage.ViewData.Model as ProcessedStartPage;

        result.Title.Should().Be("Start Page");
        result.Slug.Should().Be("start-page");
        result.Teaser.Should().Be("this is a teaser");
        result.Summary.Should().Be("This is a summary");
        result.UpperBody.Should().Be(MarkdownWrapper.ToHtml("An upper body"));
        result.FormLinkLabel.Should().Be("Start now");
        result.FormLink.Should().Be("http://start.com");
        result.LowerBody.Should().Be(MarkdownWrapper.ToHtml("Lower body"));
        result.Breadcrumbs.Should().HaveCount(1);
        result.Alerts.First().Title.Should().Be("title");
        result.Alerts.First().Body.Should().Contain("body");
        result.Alerts.First().SubHeading.Should().Be("subHeading");
        result.Alerts.First().Severity.Should().Be("severity");
    }

    [Fact]
    public async Task GivesNotFoundOnRequestForNonExistentStartPage()
    {
        var result = await _controller.Index("doesnt-exist") as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
