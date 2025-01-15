namespace StockportWebappTests_Unit.Unit.Controllers;

public class StartPageControllerTest
{
    private readonly StartPageController _controller;
    private readonly Mock<IProcessedContentRepository> _repository = new();

    public StartPageControllerTest()
    {
        List<Alert> alerts = new(){ new("title",
                                        "subHeading",
                                        "body",
                                        "severity",
                                        new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                        new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                                        string.Empty,
                                        false,
                                        string.Empty) };
        
        List<Alert> inlineAlerts = new(){ new("title",
                                            "subHeading",
                                            "body",
                                            "severity",
                                            new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                            new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                                            string.Empty,
                                            false,
                                            string.Empty) };

        ProcessedStartPage startPage = new("start-page",
                                        "Start Page",
                                        "this is a teaser",
                                        "This is a summary",
                                        "<p>An upper body</p>\n",
                                        "Start now",
                                        "http://start.com",
                                        "<p>Lower body</p>\n",
                                        new List<Crumb>
                                        {
                                            new("title", "slug", "type")
                                        },
                                        "image.jpg",
                                        "icon",
                                        alerts);

        _repository
            .Setup(repo => repo.Get<StartPage>("start-page", null))
            .ReturnsAsync(new HttpResponse(200, startPage, string.Empty));
        
        _repository
            .Setup(repo => repo.Get<StartPage>("doesnt-exist", null))
            .ReturnsAsync(new HttpResponse(404, null, "No start-page found for 'doesnt-exist'"));
        
        _controller = new(_repository.Object);
    }

    [Fact]
    public async Task GetAStartPage()
    {
        // Act
        ViewResult indexPage = await _controller.Index("start-page") as ViewResult;
        ProcessedStartPage result = indexPage.ViewData.Model as ProcessedStartPage;

        // Assert
        Assert.Equal("Start Page", result.Title);
        Assert.Equal("start-page", result.Slug);
        Assert.Equal("this is a teaser", result.Teaser);
        Assert.Equal("This is a summary", result.Summary);
        Assert.Equal(MarkdownWrapper.ToHtml("An upper body"), result.UpperBody);
        Assert.Equal("Start now", result.FormLinkLabel);
        Assert.Equal("http://start.com", result.FormLink);
        Assert.Equal(MarkdownWrapper.ToHtml("Lower body"), result.LowerBody);
        Assert.Single(result.Breadcrumbs);
        Assert.Equal("title", result.Alerts.First().Title);
        Assert.Contains("body", result.Alerts.First().Body);
        Assert.Equal("subHeading", result.Alerts.First().SubHeading);
        Assert.Equal("severity", result.Alerts.First().Severity);
    }

    [Fact]
    public async Task GivesNotFoundOnRequestForNonExistentStartPage()
    {
        // Act
        StatusCodeResult result = await _controller.Index("doesnt-exist") as StatusCodeResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
    }
}