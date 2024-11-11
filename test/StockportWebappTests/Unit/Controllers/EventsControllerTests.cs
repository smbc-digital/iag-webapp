namespace StockportWebappTests_Unit.Unit.Controllers;

public class EventsControllerTest
{
    private readonly EventsController _controller;
    private readonly Mock<IRepository> _repository = new Mock<IRepository>();
    private readonly Mock<IProcessedContentRepository> _processedContentRepository = new();
    private readonly Event _eventsItem;
    private readonly List<string> _categories;
    private readonly HttpResponse responseListing;
    private readonly HttpResponse _responseDetail;
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration;
    private readonly Mock<IRssFeedFactory> _mockRssFeedFactory = new();
    private readonly Mock<ILogger<EventsController>> _logger;
    private readonly Mock<IApplicationConfiguration> _config = new();
    private const string BusinessId = "businessId";
    private readonly Mock<IFilteredUrl> _filteredUrl = new();
    private readonly DateCalculator _datetimeCalculator;
    private Mock<IFeatureManager> _featureManager = new();
    private readonly Group _group = new GroupBuilder().Build();

    private readonly List<Alert> _alerts = new()
        {
            new Alert("title",
                    "subHeading",
                    "body",
                    "severity",
                    new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                    string.Empty,
                    false,
                    string.Empty)
        };

    public const int MaxNumberOfItemsPerPage = 15;

    public EventsControllerTest()
    {
        _eventsItem = new Event
        {
            Title = "title",
            Slug = "slug",
            Teaser = "teaser",
            ImageUrl = "image.png",
            ThumbnailImageUrl = "image.png",
            Description = "description",
            Fee = "fee",
            Location = "location",
            SubmittedBy = "submittedBy",
            EventDate = new DateTime(2016, 12, 30, 00, 00, 00),
            StartTime = "startTime",
            EndTime = "endTime",
            Breadcrumbs = new List<Crumb>(),
            Group = _group,
            Alerts = _alerts
        };

        _categories = new() { "Category 1", "Category 2" };

        Mock<ITimeProvider> mockTime = new();
        _datetimeCalculator = new DateCalculator(mockTime.Object);

        EventResponse eventsCalendar = new(new List<Event> { _eventsItem }, _categories);
        ProcessedEvents eventItem = new("title",
                                        "slug",
                                        "teaser",
                                        "image.png",
                                        "image.png",
                                        "description",
                                        "fee",
                                        false,
                                        "location",
                                        "submittedBy",
                                        new DateTime(2016, 12, 30, 00, 00, 00),
                                        "startTime",
                                        "endTime",
                                        new List<Crumb>(),
                                        _categories,
                                        new MapDetails(),
                                        "booking information",
                                        _group,
                                        _alerts,
                                        string.Empty,
                                        new(),
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        new());

        EventHomepage eventHomepage = new(new List<Alert>())
        {
            Categories = new(),
            Rows = new()
        };

        // setup responses (with mock data)
        responseListing = new HttpResponse(200, eventsCalendar, string.Empty);
        _responseDetail = new HttpResponse(200, eventItem, string.Empty);
        HttpResponse responseHomepage = new(200, eventHomepage, string.Empty);
        HttpResponse response404 = new(404, null, "not found");

        // setup mocks
        _repository
            .Setup(repo => repo.Get<EventHomepage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(responseHomepage);

        _repository
            .Setup(repo => repo.Get<EventResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(responseListing);

        _processedContentRepository
            .Setup(processedContentRepo => processedContentRepo.Get<Event>("event-of-the-century", It.Is<List<Query>>(l => l.Count.Equals(0))))
            .ReturnsAsync(_responseDetail);

        _processedContentRepository
            .Setup(processedContentRepo => processedContentRepo.Get<Event>("404-event", It.Is<List<Query>>(l => l.Count.Equals(0))))
            .ReturnsAsync(response404);

        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _logger = new Mock<ILogger<EventsController>>();

        _applicationConfiguration
            .Setup(appConfig => appConfig.GetEmailEmailFrom(It.IsAny<string>()))
            .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

        _config
            .Setup(config => config.GetRssEmail(BusinessId))
            .Returns(AppSetting.GetAppSetting("rss-email"));
        
        _config
            .Setup(config => config.GetEmailAlertsNewSubscriberUrl(BusinessId))
            .Returns(AppSetting.GetAppSetting("email-alerts-url"));

        _featureManager
            .Setup(featureManager => featureManager.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _controller = new EventsController(
            _repository.Object,
            _processedContentRepository.Object,
            _mockRssFeedFactory.Object,
            _logger.Object,
            _config.Object,
            new BusinessId(BusinessId),
            _filteredUrl.Object,
            null,
            _datetimeCalculator,
            null,
            _featureManager.Object
            );
    }

    [Fact]
    public async Task Index_ShouldReturnEventsCalendar()
    {
        // Act
        ViewResult actionResponse = await _controller.Index(new EventCalendar() { FromSearch = true }, 1, 12) as ViewResult;
        EventCalendar events = actionResponse.ViewData.Model as EventCalendar;

        // Assert
        Assert.Single(events.Events);
        Assert.Equal(_eventsItem, events.Events.First());
    }

    [Fact]
    public async Task Index_ShouldReturnEventsCalendarWhenQueryStringIsPassed()
    {
        // Act
        ViewResult actionResponse = await _controller.Index(new EventCalendar
        {
            FromSearch = true,
            Category = "test",
            DateFrom = new DateTime(2017, 01, 20),
            DateTo = new DateTime(2017, 01, 25),
            DateRange = "customdate"
        },
        1, 12) as ViewResult;

        EventCalendar events = actionResponse.ViewData.Model as EventCalendar;

        // Assert
        Assert.Single(events.Events);
        Assert.Equal(_eventsItem, events.Events.First());
        Assert.Equal("test", events.Category);
        Assert.Equal(new DateTime(2017, 01, 20), events.DateFrom);
        Assert.Equal(new DateTime(2017, 01, 25), events.DateTo);
        Assert.Equal("customdate", events.DateRange);
    }

    [Fact]
    public async Task Detail_ShouldReturnEvent()
    {
        // Act
        ViewResult actionResponse = await _controller.Detail("event-of-the-century") as ViewResult; ;
        ProcessedEvents model = actionResponse.ViewData.Model as ProcessedEvents;
        
        // Assert
        Assert.Equal("title", model.Title);
        Assert.Equal("slug", model.Slug);
        Assert.Equal("teaser", model.Teaser);
        Assert.Equal("fee", model.Fee);
        Assert.Equal("location", model.Location);
        Assert.Equal("submittedBy", model.SubmittedBy);
        Assert.Equal(new DateTime(2016, 12, 30, 00, 00, 00), model.EventDate);
        Assert.Equal("startTime", model.StartTime);
        Assert.Equal("endTime", model.EndTime);
        Assert.Equal("booking information", model.BookingInformation);
        Assert.Equal(_group.Name, model.Group.Name);
        Assert.Equal(_alerts.First().Title, model.Alerts.First().Title);
        Assert.Equal(_alerts.First().Body, model.Alerts.First().Body);
        Assert.Equal(_alerts.First().Severity, model.Alerts.First().Severity);
        Assert.Equal(_alerts.First().SubHeading, model.Alerts.First().SubHeading);
        Assert.Equal(_alerts.First().SunriseDate, model.Alerts.First().SunriseDate);
        Assert.Equal(_alerts.First().SunsetDate, model.Alerts.First().SunsetDate);
    }

    [Fact]
    public async Task Detail_ShouldReturnEventWhenDateQueryPassedIn()
    {
        // Arrange
        _processedContentRepository
            .Setup(processedContentRepo => processedContentRepo.Get<Event>("event-of-the-century", It.Is<List<Query>>(query => query.Contains(new Query("date", new DateTime().ToString("yyyy-MM-dd"))))))
            .ReturnsAsync(_responseDetail);

        // Act
        await _controller.Detail("event-of-the-century", new DateTime());
        
        // Assert
        _processedContentRepository.Verify(processedContentRepo => processedContentRepo.Get<Event>("event-of-the-century", It.Is<List<Query>>(query => query.Contains(new Query("date", new DateTime().ToString("yyyy-MM-dd"))))));
    }

    [Fact]
    public async Task Detail_ItReturns404NotFoundForEvent()
    {
        // Act
        HttpResponse actionResponse = await _controller.Detail("404-event") as HttpResponse; ;

        // Assert
        Assert.Equal(404, actionResponse.StatusCode);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(2, 1, 2, 1)]
    [InlineData(MaxNumberOfItemsPerPage, 1, MaxNumberOfItemsPerPage, 1)]
    [InlineData(MaxNumberOfItemsPerPage * 3, 1, MaxNumberOfItemsPerPage, 3)]
    [InlineData(MaxNumberOfItemsPerPage + 1, 2, 1, 2)]
    public async Task Index_PaginationShouldResultInCorrectNumItemsOnPageAndCorrectNumPages(
        int totalNumItems,
        int requestedPageNumber,
        int expectedNumItemsOnPage,
        int expectedNumPages)
    {
        // Arrange
        EventsController controller = SetUpController(totalNumItems);
        EventCalendar model = new() { FromSearch = true };

        // Act
        ViewResult actionResponse = await controller.Index(model, requestedPageNumber, MaxNumberOfItemsPerPage) as ViewResult; ;

        // Assert
        EventCalendar viewModel = actionResponse.ViewData.Model as EventCalendar;
        Assert.Equal(expectedNumItemsOnPage, viewModel.Events.Count);
        Assert.Equal(expectedNumPages, model.Pagination.TotalPages);
    }

    [Theory]
    [InlineData(0, 50, 1)]
    [InlineData(5, MaxNumberOfItemsPerPage * 3, 3)]
    public async Task Index_IfSpecifiedPageNumIsImpossibleThenActualPageNumWillBeAdjustedAccordingly(
        int specifiedPageNumber,
        int numItems,
        int expectedPageNumber)
    {
        // Arrange
        EventsController controller = SetUpController(numItems);
        EventCalendar model = new() { FromSearch = true };

        // Act
        await controller.Index(model, specifiedPageNumber, MaxNumberOfItemsPerPage);

        // Assert
        Assert.Equal(expectedPageNumber, model.Pagination.CurrentPageNumber);
    }

    [Fact]
    public async Task Index_ShouldReturnEmptyPaginationObjectIfNoEventsExist()
    {
        // Arrange
        EventsController controller = SetUpController(0);
        EventCalendar model = new() { FromSearch = true };

        // Act
        await controller.Index(model, 0, 12);

        // Assert
        Assert.NotNull(model.Pagination);
    }

    [Fact]
    public async Task Index_ShouldReturnCurrentURLForPagination()
    {
        // Arrange
        EventsController controller = SetUpController(10);
        EventCalendar model = new() { FromSearch = true };

        // Act
        await controller.Index(model, 0, 12);

        // Assert
        Assert.NotNull(model.Pagination.CurrentUrl);
    }

    private EventsController SetUpController(int numItems)
    {
        List<Event> listOfEvents = BuildEventList(numItems);

        List<string> categories = new() { "Category 1", "Category 2" };
        EventResponse eventsCalendar = new(listOfEvents, categories);
        HttpResponse eventListResponse = new(200, eventsCalendar, string.Empty);

        _repository
            .Setup(repo => repo.Get<EventResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(eventListResponse);

        EventsController controller = new(
            _repository.Object,
            _processedContentRepository.Object,
            _mockRssFeedFactory.Object,
            _logger.Object,
            _config.Object,
            new BusinessId(BusinessId),
            _filteredUrl.Object,
            null,
            _datetimeCalculator,
            null,
            _featureManager.Object
        );

        return controller;
    }

    private List<Event> BuildEventList(int numberOfItems)
    {
        List<Event> listOfEvents = new();

        for (int i = 0; i < numberOfItems; i++)
        {
            Event eventItem = new()
            {
                Title = "title",
                Slug = "slug",
                Teaser = "teaser",
                ImageUrl = "image.png",
                ThumbnailImageUrl = "image.png",
                Description = "description",
                Fee = "fee",
                Location = "location",
                SubmittedBy = "submittedBy",
                EventDate = new DateTime(2016, 12, 30, 00, 00, 00),
                StartTime = "startTime",
                EndTime = "endTime",
                Breadcrumbs = new List<Crumb>(),
                Group = _group,
                Alerts = _alerts
            };

            listOfEvents.Add(eventItem);
        }

        return listOfEvents;
    }
}