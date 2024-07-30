namespace StockportWebappTests_Unit.Unit.Controllers;

public class ArticleControllerTest
{
    private readonly ArticleController _controller;
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<IProcessedContentRepository> _processedRepository = new();
    private readonly Mock<IContactUsMessageTagParser> _contactUsMessageParser = new();
    private const string DefaultMessage = "A default message";
    private readonly ProcessedSection sectionOne = new("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
    private readonly ProcessedSection sectionTwo = new("Types of Physical Activity", It.IsAny<string>(), It.IsAny<string>(), "body", new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
    private readonly ProcessedArticle article;

    public ArticleControllerTest()
    {
        _controller = new(_repository.Object, _processedRepository.Object, _contactUsMessageParser.Object);
        article = new ProcessedArticle(
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection> { sectionOne, sectionTwo },
            string.Empty, string.Empty, string.Empty, string.Empty,  new List<Crumb> { },
            new List<Alert>(), new NullTopic(), new List<Alert>(), DateTime.Now, false, new List<GroupBranding>(), string.Empty, new List<SubItem>()
        );

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));
    }

    [Fact]
    public async Task Article_ShouldReturnArticleView()
    {
        // Arrange
        ProcessedArticle article = new("Physical Activity", "physical-activity", "Being active is great for your body", "teaser", "meta description", 
            new List<ProcessedSection>() { DummySection() }, "fa-icon", "af981b9771822643da7a03a9ae95886f/runners.jpg", "af981b9771822643da7a03a9ae95886f/runners.jpg", "alt-text",
            new List<Crumb>() { new("title", "slug", "type") }, new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), "logo-title", new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));

        // Act
        var articlePage = await _controller.Article("physical-activity", DefaultMessage) as ViewResult; ;
        var viewModel = articlePage.ViewData.Model as ArticleViewModel;
        
        // Assert
        Assert.Equal("Physical Activity", viewModel.Article.Title);
        Assert.Equal("/physical-activity", viewModel.Article.NavigationLink);
        Assert.Equal("Being active is great for your body", viewModel.Article.Body);
        Assert.Equal("af981b9771822643da7a03a9ae95886f/runners.jpg", viewModel.Article.BackgroundImage);
        Assert.Equal("af981b9771822643da7a03a9ae95886f/runners.jpg", viewModel.Article.Image);
        Assert.Equal("fa-icon", viewModel.Article.Icon);
        Assert.Single(viewModel.Article.Sections);
    }

    [Fact]
    public async Task Article_ShouldReturnFirstSection_If_MultipleSectionsArticleWithNoSectionSlug()
    {
        // Act
        var view = await _controller.Article("physical-activity", DefaultMessage) as ViewResult; ;
        var displayedArticle = view.ViewData.Model as ArticleViewModel;
        
        // Assert
        Assert.Equal("Overview", displayedArticle.DisplayedSection.Title);
        Assert.Equal("physical-activity-overview", displayedArticle.DisplayedSection.Slug);
        Assert.True(displayedArticle.ShouldShowArticleSummary);
    }

    [Fact]
    public async Task Article_ShouldSetViewDataNullCanonicalUrl_If_MultipleSectionsArticleWithNoSectionSlug()
    {
        // Act
        var view = await _controller.Article("physical-activity", DefaultMessage) as ViewResult;

        // Assert
        Assert.Null(view.ViewData["CanonicalUrl"]);
    }

    [Fact]
    public async Task Article_ShouldSetViewDataCanonicalUrl_If_MultipleSectionsArticleWithSectionSlug()
    {
        // Act
        var view = await _controller.ArticleWithSection("physical-activity", "physical-activity-overview", DefaultMessage) as ViewResult; ;

        // Assert
        string canonicalUrl = (string)view.ViewData["CanonicalUrl"];
        Assert.NotNull(canonicalUrl);
        Assert.Equal("/physical-activity", canonicalUrl);
        Assert.NotEqual("physical-activity-overview", canonicalUrl);
    }

    [Fact]
    public async Task Article_ShouldReturnCorrespondingSection_If_MultipleSectionsArticleWithSectionSlug()
    {
        // Arrange
        ProcessedSection sectionOne = new("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
        ProcessedSection sectionTwo = new("Types of Physical Activity", "types-of-physical-activity", It.IsAny<string>(), "body", new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { sectionOne, sectionTwo }, string.Empty, string.Empty, string.Empty, string.Empty, new List<Crumb>() { },
            new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));

        // Act
        var view = await _controller.ArticleWithSection("physical-activity", "types-of-physical-activity", DefaultMessage) as ViewResult;
        var displayedArticle = view.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.Equal("Types of Physical Activity", displayedArticle.DisplayedSection.Title);
        Assert.Equal("types-of-physical-activity", displayedArticle.DisplayedSection.Slug);
        Assert.False(displayedArticle.ShouldShowArticleSummary);
    }

    [Fact]
    public async Task Article_GetsAlertsInline()
    {
        // Arrange
        List<Alert> alertsInline = new()
        {
            new("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty)
        };
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { }, string.Empty, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), alertsInline, new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));
        
        // Act
        var result = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.Single(resultModel.Article.AlertsInline);
        Assert.Equal("title", resultModel.Article.AlertsInline.First().Title);
        Assert.Equal("subheading", resultModel.Article.AlertsInline.First().SubHeading);
        Assert.Equal("<p>body</p>\n", resultModel.Article.AlertsInline.First().Body);
        Assert.Equal(Severity.Warning, resultModel.Article.AlertsInline.First().Severity);
    }

    [Fact]
    public async Task Article_ShouldSetAlerts()
    {
        // Arrange
        List<Alert> alerts = new()
        {
            new("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),string.Empty, false, string.Empty)
        };
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>(), string.Empty, string.Empty, string.Empty, string.Empty, new List<Crumb>(), alerts, new NullTopic(), new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));
        
        // Act
        var result = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.Single(resultModel.Article.Alerts);
        Assert.Equal("title", resultModel.Article.Alerts.First().Title);
        Assert.Equal("subheading", resultModel.Article.Alerts.First().SubHeading);
        Assert.Equal("<p>body</p>\n", resultModel.Article.Alerts.First().Body);
        Assert.Equal(Severity.Warning, resultModel.Article.Alerts.First().Severity);
    }

    [Fact]
    public async Task Article_InvokesArticleFactory()
    {
        // Arrange
        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, DummyProcessedArticle()));

        // Act
        var result = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.IsType<ProcessedArticle>(resultModel.Article);
    }

    [Fact]
    public async Task Article_ShouldParseContactUsMessage()
    {
        // Arrange
        ProcessedArticle processedArticle = DummyProcessedArticle();
        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, processedArticle));

        // Act
        await _controller.Article("healthy-living", DefaultMessage);
        
        // Assert
        _contactUsMessageParser.Verify(_ => _.Parse(processedArticle, DefaultMessage, string.Empty), Times.Once);
    }

    [Fact]
    public async Task Article_GetsAlertsInlineForASectionInAnArticle()
    {
        // Arrange
        List<Alert> alertsInline = new()
        {
            new("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false, string.Empty)
        };

        ProcessedSection processedSection = new("title", "slug", string.Empty, "body", new List<Profile>(), new List<Document>(), alertsInline, new List<GroupBranding>(), "logoAreaTitle", new DateTime());

        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { processedSection }, string.Empty, string.Empty, string.Empty, string.Empty, new List<Crumb>(), new List<Alert>(), new NullTopic(), alertsInline, new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));

        // Act
        var result = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.Single(resultModel.Article.Sections.FirstOrDefault().AlertsInline);
        Assert.Equal("title", resultModel.Article.Sections.FirstOrDefault().AlertsInline.First().Title);
        Assert.Equal("subheading", resultModel.Article.Sections.FirstOrDefault().AlertsInline.First().SubHeading);
        Assert.Equal("<p>body</p>\n", resultModel.Article.Sections.FirstOrDefault().AlertsInline.First().Body);
        Assert.Equal(Severity.Warning, resultModel.Article.Sections.FirstOrDefault().AlertsInline.First().Severity);
    }

    [Fact]
    public async Task Article_ShouldReturnNotFound()
    {
        // Arrange
        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, "error", string.Empty));
        
        // Act
        var result = await _controller.Article("physical-activity-test", "I-do-not-exist") as StatusCodeResult; ;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ArticleWithSection_ShouldReturnViewDataWithMetaDescription()
    {
        // Arrange
        ProcessedSection section = new(string.Empty, "test-slug", "test meta description", string.Empty, null, null, null, null, string.Empty, new DateTime());
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<ProcessedSection> { section },
            string.Empty, string.Empty, string.Empty, null, null, null, null, null, new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));
        
        // Act
        var result = await _controller.ArticleWithSection(string.Empty, "test-slug", string.Empty) as ViewResult;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.NotNull(resultModel);
        Assert.Equal("test meta description", resultModel.DisplayedSection.MetaDescription);
    }

    [Fact]
    public async Task ArticleWithSection_ShouldReturnViewDataWithMetaDescription_If_MulitpleArticlesWithSections()
    {
        // Arrange
        ProcessedSection section1 = new(string.Empty, "test-slug", "test meta description", string.Empty, null, null, null, null, string.Empty, new DateTime());
        ProcessedSection section2 = new(string.Empty, string.Empty, "other string", string.Empty, null, null, null, null, string.Empty, new DateTime());
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<ProcessedSection> { section1, section2 },
            string.Empty, string.Empty, string.Empty, null, null, null, null, null, new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>());

        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, article));
        
        // Act
        var result = await _controller.ArticleWithSection(string.Empty, "test-slug", string.Empty) as ViewResult;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        Assert.NotNull(resultModel);
        Assert.Equal("test meta description", resultModel.DisplayedSection.MetaDescription);
    }

    [Fact]
    public async Task ArticleWithSection_ShouldReturnNotFoundWhenSectionDoesNotExist()
    {
        // Arrange
        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, "error", string.Empty));

        // Act
        var result = await _controller.ArticleWithSection("physical-activity", "I-do-not-exist", DefaultMessage) as StatusCodeResult; ;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ArticleWithSection_ShouldParseContactUsMessage()
    {
        // Arrange
        ProcessedArticle processedArticle = DummyProcessedArticle();
        _processedRepository
            .Setup(_ => _.Get<Article>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, processedArticle, string.Empty));
        
        // Act
        await _controller.ArticleWithSection("healthy-living", "test-section", DefaultMessage);

        // Assert
        _contactUsMessageParser.Verify(_ => _.Parse(processedArticle, DefaultMessage, "test-section"), Times.Once);
    }

    private static ProcessedArticle DummyProcessedArticle() => 
        new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            new List<ProcessedSection>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),  new List<Crumb>(),
            new LinkedList<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), It.IsAny<string>(), new List<SubItem>());

    private static ProcessedSection DummySection() => 
        new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
}