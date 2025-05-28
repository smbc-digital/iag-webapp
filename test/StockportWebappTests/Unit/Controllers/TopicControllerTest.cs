namespace StockportWebappTests_Unit.Unit.Controllers;

public class TopicControllerTest
{
    private readonly TopicController _controller;
    private readonly Mock<ITopicRepository> _repository = new();
    private const string BusinessId = "stockportgov";
    private readonly EventCalendarBanner _eventCalendarBanner;
    private readonly EventBanner _eventBanner;
    private readonly CallToActionBanner _callToAction;
    private readonly Mock<IStockportApiEventsService> _stockportApiService = new();

    public TopicControllerTest()
    {
        Mock<IApplicationConfiguration> config = new();

        _controller = new(_repository.Object, config.Object, new BusinessId(BusinessId), _stockportApiService.Object);

        _eventCalendarBanner = new EventCalendarBanner()
        {
            Title = "title",
            Teaser = "teaser",
            Icon = "icon",
            Link = "link",
            Colour = EColourScheme.Teal
        };

        _eventBanner = new("title", "teaser", "icon", "link");

        _callToAction = new CallToActionBanner()
        {
            Title = "title",
            AltText = "altText",
            ButtonText = "buttonText",
            Colour = EColourScheme.Teal,
            Image = "image",
            Link = "link",
            Teaser = "teaser"
        };
    }

    public SubItem CreateASubItem(int i) =>
        new($"sub-topic{i}", $"Title{i}", "Teaser", "teaser image", "Icon", "topic", "image", new List<SubItem>(), EColourScheme.Teal);

    [Fact]
    public async Task Index_ReturnsTopicWithExpectedProperties()
    {
        // Arrange
        List<SubItem> subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name",
                                "slug",
                                "<p>Summary</p>\n",
                                "Teaser",
                                "metaDescription",
                                "Icon",
                                "Image",
                                "Image",
                                null,
                                subItems,
                                null,
                                new List<Crumb>(),
                                new List<Alert>(),
                                _eventBanner,
                                _eventCalendarBanner,
                                true,
                                new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                string.Empty,
                                _callToAction,
                                null,
                                string.Empty);

        _repository
            .Setup(repo => repo.Get<ProcessedTopic>("healthy-living"))
            .ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        ViewResult indexPage = await _controller.Index( "healthy-living") as ViewResult;
        TopicViewModel viewModel = indexPage.ViewData.Model as TopicViewModel;
        ProcessedTopic result = viewModel.Topic;

        // Assert
        Assert.Equal("Name", result.Name);
        Assert.Equal("slug", result.Slug);
        Assert.Equal("<p>Summary</p>\n", result.Summary);
        Assert.Equal("Teaser", result.Teaser);
        Assert.Equal("Icon", result.Icon);
        Assert.Equal("Image", result.BackgroundImage);
        Assert.Equal("Image", result.Image);
        Assert.True(result.DisplayContactUs);
        Assert.Equal(_eventBanner.Title, result.EventBanner.Title);
        Assert.Equal(_eventBanner.Teaser, result.EventBanner.Teaser);
        Assert.Equal("metaDescription", result.MetaDescription);
        Assert.Equal(_eventBanner.Icon, result.EventBanner.Icon);
        Assert.Equal(_eventBanner.Link, result.EventBanner.Link);
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
        List<SubItem> subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name",
                                "slug",
                                "<p>Summary</p>",
                                "Teaser",
                                "metaDescription",
                                "Icon",
                                "Image",
                                "Image",
                                null,
                                subItems,
                                null,
                                new List<Crumb>(),
                                new List<Alert>(),
                                _eventBanner,
                                _eventCalendarBanner,
                                true,
                                new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                string.Empty,
                                _callToAction,
                                null,
                                string.Empty);

        _repository
            .Setup(repo => repo.Get<ProcessedTopic>("healthy-living"))
            .ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        ViewResult indexPage = await _controller.Index("healthy-living") as ViewResult;
        TopicViewModel viewModel = indexPage.ViewData.Model as TopicViewModel;
        ProcessedTopic result = viewModel.Topic;
        SubItem subItem = result.SubItems.FirstOrDefault();

        // Assert
        Assert.Equal("Title0", subItem.Title);
        Assert.Equal("/topic/sub-topic0", subItem.NavigationLink);
        Assert.Equal("Teaser", subItem.Teaser);
        Assert.Equal("Icon", subItem.Icon);
    }

    [Fact]
    public async Task Index_ReturnsNotFoundOnRequest_For_NonExistentTopic()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<ProcessedTopic>("doesnt-exist"))
            .ReturnsAsync(new HttpResponse(404, null, "No topic found for 'doesnt-exist'"));

        // Act
        StatusCodeResult result = await _controller.Index("doesnt-exist") as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_GetsAlertsForTopic()
    {
        // Arrange
        List<Alert> alerts = new()
        {
            new("title", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty)
        };

        ProcessedTopic topic = new("Name",
                                "slug",
                                "<p>Summary</p>",
                                "Teaser",
                                "metaDescription",
                                "Icon",
                                "Image",
                                "Image",
                                null,
                                null,
                                null,
                                new List<Crumb>(),
                                alerts,
                                _eventBanner,
                                _eventCalendarBanner,
                                true,
                                new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                string.Empty,
                                _callToAction,
                                null,
                                string.Empty);

        _repository
            .Setup(repo => repo.Get<ProcessedTopic>("healthy-living"))
            .ReturnsAsync(new HttpResponse(200, topic, string.Empty));

        // Act
        ViewResult indexPage = await _controller.Index("healthy-living") as ViewResult;
        TopicViewModel viewModel = indexPage.ViewData.Model as TopicViewModel;
        ProcessedTopic result = viewModel.Topic;

        // Assert
        Assert.Single(result.Alerts);
        Assert.Equal("title", result.Alerts.First().Title);
        Assert.Equal("<p>body</p>\n", result.Alerts.First().Body);
        Assert.Equal(Severity.Warning, result.Alerts.First().Severity);
    }

    [Fact]
    public async Task Index_Should_CallApiService_IfEventCategoryNotEmpty()
    {
        // Arrange
        List<SubItem> subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

        ProcessedTopic topic = new("Name",
                                "slug",
                                "<p>Summary</p>",
                                "Teaser",
                                "metaDescription",
                                "Icon",
                                "Image",
                                "Image",
                                null,
                                subItems,
                                null,
                                new List<Crumb>(),
                                new List<Alert>(),
                                _eventBanner,
                                _eventCalendarBanner,
                                true,
                                new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                "eventCategory",
                                _callToAction,
                                null,
                                string.Empty);

        _repository
            .Setup(repo => repo.Get<ProcessedTopic>("healthy-living"))
            .ReturnsAsync(new HttpResponse(200, topic, string.Empty));
        
        _stockportApiService
            .Setup(service => service.GetEventsByCategory("eventCategory", true))
            .ReturnsAsync(new List<Event> { new EventBuilder().Build() });

        // Act
        await _controller.Index("healthy-living");

        // Assert
        _stockportApiService.Verify(service => service.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }
}