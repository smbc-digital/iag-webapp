namespace StockportWebappTests_Unit.Unit.Controllers;

public class TopicControllerTest
{
    private readonly TopicController _controller;
    private readonly Mock<ITopicRepository> _repository;
    private readonly Mock<IFeatureManager> _featureToggle;
    private const string BusinessId = "stockportgov";
    private readonly EventBanner _eventBanner;
    private readonly CallToActionBanner _callToAction;
    private readonly Mock<IStockportApiEventsService> _stockportApiService = new();

    public TopicControllerTest()
    {
        var config = new Mock<IApplicationConfiguration>();

        config.Setup(_ => _.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

        _repository = new Mock<ITopicRepository>();
        _featureToggle = new Mock<IFeatureManager>();
        _controller = new TopicController(_repository.Object, config.Object, new BusinessId(BusinessId), _stockportApiService.Object, _featureToggle.Object);
        _eventBanner = new EventBanner("title", "teaser", "icon", "link");
        _callToAction = new CallToActionBanner()
        {
            Title = "title",
            AltText = "altText",
            ButtonText = "buttonText",
            Colour = "colour",
            Image = "image",
            Link = "link",
            Teaser = "teaser"
        };
    }

    public SubItem CreateASubItem(int i)
    {
        return new SubItem("sub-topic" + i, "Title" + i, "Teaser", "Icon", "topic", "image", new List<SubItem>());
    }

    [Fact]
    public async Task Index_ReturnsTopicWithExpectedProperties()
    {
        // Arrange
        var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name", "slug", "<p>Summary</p>\n", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
            new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText",
            new List<ExpandingLinkBox> { new("title", subItems) }, string.Empty, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, _callToAction, null, string.Empty);

        const string slug = "healthy-living";
        _repository.Setup(_ => _.Get<ProcessedTopic>(slug)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        var indexPage = await _controller.Index(slug) as ViewResult;
        var viewModel = indexPage.ViewData.Model as TopicViewModel;
        var result = viewModel.Topic;

        // Assert
        Assert.Equal("Name", result.Name);
        Assert.Equal("slug", result.Slug);
        Assert.Equal("<p>Summary</p>\n", result.Summary);
        Assert.Equal("Teaser", result.Teaser);
        Assert.Equal("Icon", result.Icon);
        Assert.Equal("Image", result.BackgroundImage);
        Assert.Equal("Image", result.Image);
        Assert.True(result.DisplayContactUs);
        Assert.True(result.EmailAlerts);
        Assert.Equal("test-id", result.EmailAlertsTopicId);
        Assert.Equal(_eventBanner.Title, result.EventBanner.Title);
        Assert.Equal(_eventBanner.Teaser, result.EventBanner.Teaser);
        Assert.Equal("metaDescription", result.MetaDescription);
        Assert.Equal(_eventBanner.Icon, result.EventBanner.Icon);
        Assert.Equal(_eventBanner.Link, result.EventBanner.Link);
        Assert.Equal("expandingLinkText", result.ExpandingLinkTitle);
        Assert.Equal("title", result.ExpandingLinkBoxes.First().Title);
        Assert.Equal("topic", result.ExpandingLinkBoxes.First().Links.First().Type);
        Assert.Equal(_callToAction.Title, result.CallToAction.Title);
        Assert.Equal(_callToAction.AltText, result.CallToAction.AltText);
        Assert.Equal(_callToAction.ButtonText, result.CallToAction.ButtonText);
        Assert.Equal(_callToAction.Colour, result.CallToAction.Colour);
        Assert.Equal(_callToAction.Image, result.CallToAction.Image);
        Assert.Equal(_callToAction.Link, result.CallToAction.Link);
        Assert.Equal(_callToAction.Teaser, result.CallToAction.Teaser);
    }

    [Fact]
    public async Task Index_ReturnsListOfSubItemsByTopic()
    {
        // Arrange
        var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name", "slug", "<p>Summary</p>", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
          new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
           new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, _callToAction, null, string.Empty);

        _repository.Setup(_ => _.Get<ProcessedTopic>("healthy-living")).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        var indexPage = await _controller.Index("healthy-living") as ViewResult;
        var viewModel = indexPage.ViewData.Model as TopicViewModel;
        var result = viewModel.Topic;
        var subItem = result.SubItems.FirstOrDefault();

        // Assert
        Assert.Equal("Title0", subItem.Title);
        Assert.Equal("/topic/sub-topic0", subItem.NavigationLink);
        Assert.Equal("Teaser", subItem.Teaser);
        Assert.Equal("Icon", subItem.Icon);
        Assert.True(result.EmailAlerts);
        Assert.Equal("test-id", result.EmailAlertsTopicId);
    }

    [Fact]
    public async Task Index_ReturnsNotFoundOnRequest_For_NonExistentTopic()
    {
        // Arrange
        const string nonExistentTopic = "doesnt-exist";
        _repository.Setup(_ => _.Get<ProcessedTopic>(nonExistentTopic)).ReturnsAsync(new HttpResponse(404, null, "No topic found for 'doesnt-exist'"));

        // Act
        var result = await _controller.Index(nonExistentTopic) as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_GetsAlertsForTopic()
    {
        // Arrange
        List<Alert> alerts = new()
        {
            new("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty)
        };

        ProcessedTopic topic = new("Name", "slug", "<p>Summary</p>", "Teaser", "metaDescription", "Icon", "Image", "Image", null, null, null,
            new List<Crumb>(), alerts, true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, _callToAction, null, string.Empty);

        _repository.Setup(_ => _.Get<ProcessedTopic>("healthy-living")).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        var indexPage = await _controller.Index("healthy-living") as ViewResult;
        var viewModel = indexPage.ViewData.Model as TopicViewModel;
        var result = viewModel.Topic;

        // Assert
        Assert.Single(result.Alerts);
        Assert.Equal("title", result.Alerts.First().Title);
        Assert.Equal("subheading", result.Alerts.First().SubHeading);
        Assert.Equal("<p>body</p>\n", result.Alerts.First().Body);
        Assert.Equal(Severity.Warning, result.Alerts.First().Severity);
        Assert.True(result.EmailAlerts);
        Assert.Equal("test-id", result.EmailAlertsTopicId);
    }

    [Fact]
    public async Task Index_Should_CallApiService_IfEventCategoryNotEmpty()
    {
        // Arrange
        var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name", "slug", "<p>Summary</p>", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
            new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), "eventCategory", _callToAction, null, string.Empty);

        _repository.Setup(_ => _.Get<ProcessedTopic>("healthy-living")).ReturnsAsync(new HttpResponse(200, topic, string.Empty));
        _stockportApiService.Setup(_ => _.GetEventsByCategory("eventCategory", true)).ReturnsAsync(new List<Event> { new EventBuilder().Build() });

        // Act
        await _controller.Index("healthy-living");

        // Assert
        _stockportApiService.Verify(_ => _.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task Index_Should_ReturnIndex2023_WithFeatureToggleEnabled()
    {
        // Arrange
        var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name", "slug", "<p>Summary</p>", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
            new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), "eventCategory", _callToAction, null, string.Empty);

        const string slug = "healthy-living";
        _repository.Setup(_ => _.Get<ProcessedTopic>(slug)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        _featureToggle.Setup(_ => _.IsEnabledAsync("TopicRedesign")).Returns(Task.FromResult(true));

        // Act
        var indexPage = await _controller.Index("healthy-living") as ViewResult;
        var page = indexPage.ViewData.Model as TopicViewModel;

        // Assert
        Assert.True(await _featureToggle.Object.IsEnabledAsync("TopicRedesign"));
        Assert.Equal("Index2023", indexPage.ViewName);
    }
}