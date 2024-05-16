namespace StockportWebappTests_Unit.Unit.Controllers;

public class ArticleControllerTest
{
    private readonly ArticleController _controller;
    private readonly Mock<IProcessedContentRepository> _contentRepository = new();
    private readonly Mock<IContactUsMessageTagParser> _contactUsMessageParser = new();
    private readonly Mock<IArticleRepository> _articleRepository = new();
    private const string DefaultMessage = "A default message";

    public ArticleControllerTest()
    {
        _controller = new ArticleController(_contentRepository.Object, _contactUsMessageParser.Object, _articleRepository.Object, new BusinessId("stockportgov"));
    }

    [Fact]
    public async Task Article_ShouldReturnArticleView()
    {
        // Arrange
        ProcessedArticle article = new("Physical Activity", "physical-activity", "Being active is great for your body", "teaser", "meta description", 
            new List<ProcessedSection>() { DummySection() }, "fa-icon", "af981b9771822643da7a03a9ae95886f/runners.jpg", "af981b9771822643da7a03a9ae95886f/runners.jpg",
            new List<Crumb>() { new("title", "slug", "type") }, new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        _articleRepository.Setup(_ => _.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

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
    public async Task MultipleSectionsArticleWithNoSectionSlugReturnsFirstSection()
    {
        // Arrange
        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", TextHelper.AnyString, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { sectionOne, sectionTwo }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { },
            new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        _articleRepository.Setup(_ => _.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        // Act
        var view = await _controller.Article("physical-activity", DefaultMessage) as ViewResult; ;
        var displayedArticle = view.ViewData.Model as ArticleViewModel;
        
        // Assert
        Assert.Equal("Overview", displayedArticle.DisplayedSection.Title);
        Assert.Equal("physical-activity-overview", displayedArticle.DisplayedSection.Slug);
        Assert.True(displayedArticle.ShouldShowArticleSummary);
    }

    // more cleanup to do:
    [Fact]
    public async Task MultipleSectionsArticleWithNoSectionSlugViewDataCanonicalUrlShouldBeNull()
    {
        const string articleSlug = "physical-activity";
        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", TextHelper.AnyString, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<ProcessedSection>() { sectionOne, sectionTwo },
            string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.Article(articleSlug, DefaultMessage) as ViewResult;

        view.ViewData["CanonicalUrl"].Should().BeNull();
    }

    [Fact]
    public async Task MultipleSectionsArticleWithSectionSlugViewDataCanonicalUrlShouldNotBeNull()
    {
        const string articleSlug = "physical-activity";
        const string sectionSlug = "physical-activity-overview";

        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", TextHelper.AnyString, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { sectionOne, sectionTwo }, string.Empty, string.Empty, string.Empty,
            new List<Crumb>() { }, new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage) as ViewResult; ;

        view.ViewData["CanonicalUrl"].Should().NotBeNull();

        string canonicalUrl = (string)view.ViewData["CanonicalUrl"];

        canonicalUrl.Should().Contain("physical-activity");
        canonicalUrl.Should().NotContain("physical-activity-overview");
    }

    [Fact]
    public async Task MultipleSectionsArticleWithSectionSlugReturnsCorrespondingSection()
    {
        const string articleSlug = "physical-activity";
        const string sectionSlug = "types-of-physical-activity";

        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", sectionSlug, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { sectionOne, sectionTwo }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { },
            new List<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage) as ViewResult; ;

        var displayedArticle = view.ViewData.Model as ArticleViewModel;

        displayedArticle.DisplayedSection.Title.Should().Contain("Types of Physical Activity");
        displayedArticle.DisplayedSection.Slug.Should().Be(sectionSlug);
        displayedArticle.ShouldShowArticleSummary.Should().BeFalse();
    }

    [Fact]
    public async Task ArticleWithSectionMetaDescriptionReturnsViewDataWithMetaDescription()
    {
        // Arrange
        var expectedMetaDescription = "test meta description";
        var sectionSlug = "test-slug";
        var section = new ProcessedSection(
            string.Empty,
            sectionSlug,
            expectedMetaDescription,
            string.Empty,
            null,
            null,
            null
        );
        var article = new ProcessedArticle(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            new List<ProcessedSection> { section },
            string.Empty,
            string.Empty,
            null,
            null,
            null,
            null,
            null,
            new DateTime(),
            new bool()
        );

        _articleRepository
            .Setup(_ => _.Get(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(200, article, string.Empty));

        // Act
        var result = await _controller
            .ArticleWithSection(
                string.Empty,
                sectionSlug,
                string.Empty) as ViewResult;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        resultModel.Should().NotBeNull();
        resultModel?.DisplayedSection.MetaDescription.Should().Be(expectedMetaDescription);
    }

    [Fact]
    public async Task MulitpleArticlesWithSectionMetaDescriptionReturnsViewDataWithMetaDescription()
    {
        // Arrange
        var expectedMetaDescription = "test meta description";
        var sectionSlug = "test-slug";
        var section1 = new ProcessedSection(
            string.Empty,
            sectionSlug,
            expectedMetaDescription,
            string.Empty,
            null,
            null,
            null
        );
        var section2 = new ProcessedSection(
            string.Empty,
            string.Empty,
            "other string",
            string.Empty,
            null,
            null,
            null
        );
        var article = new ProcessedArticle(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            new List<ProcessedSection> { section1, section2 },
            string.Empty,
            string.Empty,
            null,
            null,
            null,
            null,
            null,
            new DateTime(),
            new bool()
        );

        _articleRepository
            .Setup(_ => _.Get(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(200, article, string.Empty));

        // Act
        var result = await _controller
            .ArticleWithSection(
                string.Empty,
                sectionSlug,
                string.Empty) as ViewResult;
        var resultModel = result.ViewData.Model as ArticleViewModel;

        // Assert
        resultModel.Should().NotBeNull();
        resultModel?.DisplayedSection.MetaDescription.Should().Be(expectedMetaDescription);
    }

    [Fact]
    public async Task ReturnNotFoundWhenSectionDoesNotExist()
    {
        const string articleSlug = "physical-activity";
        const string sectionSlug = "I-do-not-exist";

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(404, "error", string.Empty));

        var result =
            await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage) as StatusCodeResult; ;

        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetsAlertsForArticle()
    {
        var alerts = new List<Alert>
        {
            new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false, string.Empty)
        };
        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, alerts, new NullTopic(), new List<Alert>(), new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var result = indexPage.ViewData.Model as ArticleViewModel;

        result.Article.Alerts.Should().HaveCount(1);
        result.Article.Alerts.First().Title.Should().Be("title");
        result.Article.Alerts.First().SubHeading.Should().Be("subheading");
        result.Article.Alerts.First().Body.Should().Be("<p>body</p>\n");
        result.Article.Alerts.First().Severity.Should().Be(Severity.Warning);
    }

    [Fact]
    public async Task ItInvokesArticleFactoryToBuildArticleForView()
    {
        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, DummyProcessedArticle(), string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var result = indexPage.ViewData.Model as ArticleViewModel;

        result.Article.Should().BeOfType(typeof(ProcessedArticle));
    }


    [Fact]
    public async Task ShouldParseForContactUsMessageForArticle()
    {
        var processedArticle = DummyProcessedArticle();
        var slug = "healthy-living";

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, processedArticle, string.Empty));

        await _controller.Article(slug, DefaultMessage);

        _contactUsMessageParser.Verify(o => o.Parse(processedArticle, DefaultMessage, ""), Times.Once);
    }

    [Fact]
    public async Task ShouldParseForContactUsMessageForArticleWithSection()
    {
        var processedArticle = DummyProcessedArticle();
        var slug = "healthy-living";
        var sectionSlug = "test-section";

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, processedArticle, string.Empty));

        await _controller.ArticleWithSection(slug, sectionSlug, DefaultMessage);

        _contactUsMessageParser.Verify(o => o.Parse(processedArticle, DefaultMessage, sectionSlug), Times.Once);
    }

    [Fact]
    public async Task GetsAlertsInlineForArticle()
    {
        var alertsInline = new List<Alert>
        {
            new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false, string.Empty)
        };
        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), alertsInline, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var result = indexPage.ViewData.Model as ArticleViewModel;

        result.Article.AlertsInline.Should().HaveCount(1);
        result.Article.AlertsInline.First().Title.Should().Be("title");
        result.Article.AlertsInline.First().SubHeading.Should().Be("subheading");
        result.Article.AlertsInline.First().Body.Should().Be("<p>body</p>\n");
        result.Article.AlertsInline.First().Severity.Should().Be(Severity.Warning);
    }

    [Fact]
    public async Task GetsAlertsInlineForASectionInAnArticle()
    {
        var alertsInline = new List<Alert>
        {
            new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false, string.Empty)
        };

        var processedSection = new ProcessedSection("title", "slug", string.Empty, "body", new List<Profile>(), new List<Document>(), alertsInline);

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { processedSection }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), alertsInline, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage) as ViewResult; ;
        var result = indexPage.ViewData.Model as ArticleViewModel;

        result.Article.Sections.FirstOrDefault().AlertsInline.Should().HaveCount(1);
        result.Article.Sections.FirstOrDefault().AlertsInline.First().Title.Should().Be("title");
        result.Article.Sections.FirstOrDefault().AlertsInline.First().SubHeading.Should().Be("subheading");
        result.Article.Sections.FirstOrDefault().AlertsInline.First().Body.Should().Be("<p>body</p>\n");
        result.Article.Sections.FirstOrDefault().AlertsInline.First().Severity.Should().Be(Severity.Warning);
    }

    private ProcessedArticle DummyProcessedArticle()
    {
        return new ProcessedArticle(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
            new List<ProcessedSection>(), TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(),
            new LinkedList<Alert>(), new NullTopic(), new List<Alert>(), new DateTime(), new bool());
    }

    private ProcessedSection DummySection()
    {
        return new ProcessedSection(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Profile>(), new List<Document>(), new List<Alert>());
    }
}