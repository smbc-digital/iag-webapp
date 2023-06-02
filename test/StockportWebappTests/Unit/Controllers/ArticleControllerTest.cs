namespace StockportWebappTests_Unit.Unit.Controllers;

public class ArticleControllerTest
{
    private readonly ArticleController _controller;
    private readonly Mock<IProcessedContentRepository> _fakeContentRepository = new Mock<IProcessedContentRepository>();
    private readonly Mock<IContactUsMessageTagParser> _contactUsMessageParser;
    private readonly Mock<IArticleRepository> _articleRepository;

    private const string DefaultMessage = "A default message";

    public ArticleControllerTest()
    {
        _contactUsMessageParser = new Mock<IContactUsMessageTagParser>();

        _articleRepository = new Mock<IArticleRepository>();

        _controller = new ArticleController(_fakeContentRepository.Object, new Mock<ILogger<ArticleController>>().Object, _contactUsMessageParser.Object, _articleRepository.Object);
    }

    [Fact]
    public async Task GivenNavigateToArticleReturnsArticleView()
    {
        const string articleSlug = "physical-activity";
        var article = new ProcessedArticle("Physical Activity", "physical-activity",
            "Being active is great for your body", "teaser", "meta description", new List<ProcessedSection>() { DummySection() },
            "fa-icon", "af981b9771822643da7a03a9ae95886f/runners.jpg", "af981b9771822643da7a03a9ae95886f/runners.jpg",
            new List<Crumb>() { new Crumb("title", "slug", "type") }, new List<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var articlePage = await _controller.Article(articleSlug, DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
        var viewModel = articlePage.ViewData.Model as ArticleViewModel;

        viewModel.Article.Title.Should().Contain("Physical Activity");
        viewModel.Article.NavigationLink.Should().Be("/physical-activity");
        viewModel.Article.Body.Should().Contain("Being active is great for your body");
        viewModel.Article.BackgroundImage.Should().Contain("af981b9771822643da7a03a9ae95886f/runners.jpg");
        viewModel.Article.Image.Should().Contain("af981b9771822643da7a03a9ae95886f/runners.jpg");
        viewModel.Article.Icon.Should().Contain("fa-icon");
        viewModel.Article.Sections.Count().Should().Be(1);
    }

    [Fact]
    public async Task MultipleSectionsArticleWithNoSectionSlugReturnsFirstSection()
    {
        const string articleSlug = "physical-activity";
        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", TextHelper.AnyString, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { sectionOne, sectionTwo }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { },
            new List<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.Article(articleSlug, DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
        var displayedArticle = view.ViewData.Model as ArticleViewModel;

        displayedArticle.DisplayedSection.Title.Should().Contain("Overview");
        displayedArticle.DisplayedSection.Slug.Should().Be("physical-activity-overview");
        displayedArticle.ShouldShowArticleSummary.Should().BeTrue();
    }

    [Fact]
    public async Task MultipleSectionsArticleWithNoSectionSlugViewDataCanonicalUrlShouldBeNull()
    {
        const string articleSlug = "physical-activity";
        var sectionOne = new ProcessedSection("Overview", "physical-activity-overview", string.Empty, "body", new List<Profile>(), new List<Document>(), new List<Alert>());
        var sectionTwo = new ProcessedSection("Types of Physical Activity", TextHelper.AnyString, TextHelper.AnyString, "body", new List<Profile>(), new List<Document>(), new List<Alert>());

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<ProcessedSection>() { sectionOne, sectionTwo },
            string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.Article(articleSlug, DefaultMessage, string.Empty, string.Empty) as ViewResult;

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
            new List<Crumb>() { }, new List<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage, "", "") as ViewResult; ;

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
            new List<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        var response = new HttpResponse(200, article, string.Empty);

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

        var view = await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage, "", "") as ViewResult; ;

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
            null,
            new DateTime(),
            new bool()
        );

        _articleRepository
            .Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(200, article, string.Empty));

        // Act
        var result = await _controller
            .ArticleWithSection(
                string.Empty,
                sectionSlug,
                string.Empty,
                string.Empty,
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
            null,
            new DateTime(),
            new bool()
        );

        _articleRepository
            .Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(200, article, string.Empty));

        // Act
        var result = await _controller
            .ArticleWithSection(
                string.Empty,
                sectionSlug,
                string.Empty,
                string.Empty,
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

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(404, "error", string.Empty));

        var result =
            await _controller.ArticleWithSection(articleSlug, sectionSlug, DefaultMessage, "", "") as StatusCodeResult; ;

        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetsAlertsForArticle()
    {
        var alerts = new List<Alert>
        {
            new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false)
        };
        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, alerts, new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
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
        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, DummyProcessedArticle(), string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
        var result = indexPage.ViewData.Model as ArticleViewModel;

        result.Article.Should().BeOfType(typeof(ProcessedArticle));
    }


    [Fact]
    public async Task ShouldParseForContactUsMessageForArticle()
    {
        var processedArticle = DummyProcessedArticle();
        var slug = "healthy-living";

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, processedArticle, string.Empty));

        await _controller.Article(slug, DefaultMessage, string.Empty, string.Empty);

        _contactUsMessageParser.Verify(o => o.Parse(processedArticle, DefaultMessage, ""), Times.Once);
    }

    [Fact]
    public async Task ShouldParseForContactUsMessageForArticleWithSection()
    {
        var processedArticle = DummyProcessedArticle();
        var slug = "healthy-living";
        var sectionSlug = "test-section";

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, processedArticle, string.Empty));

        await _controller.ArticleWithSection(slug, sectionSlug, DefaultMessage, "", "");

        _contactUsMessageParser.Verify(o => o.Parse(processedArticle, DefaultMessage, sectionSlug), Times.Once);
    }

    [Fact]
    public async Task GetsAlertsInlineForArticle()
    {
        var alertsInline = new List<Alert>
        {
            new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false)
        };
        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), alertsInline, null, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
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
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false)
        };

        var processedSection = new ProcessedSection("title", "slug", string.Empty, "body", new List<Profile>(), new List<Document>(), alertsInline);

        var article = new ProcessedArticle(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            new List<ProcessedSection>() { processedSection }, string.Empty, string.Empty, string.Empty, new List<Crumb>() { }, new List<Alert>(), new NullTopic(), alertsInline, null, new DateTime(), new bool());

        _articleRepository.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponse(200, article, string.Empty));

        var indexPage = await _controller.Article("healthy-living", DefaultMessage, string.Empty, string.Empty) as ViewResult; ;
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
            new LinkedList<Alert>(), new NullTopic(), new List<Alert>(), null, new DateTime(), new bool());
    }

    private ProcessedSection DummySection()
    {
        return new ProcessedSection(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Profile>(), new List<Document>(), new List<Alert>());
    }
}