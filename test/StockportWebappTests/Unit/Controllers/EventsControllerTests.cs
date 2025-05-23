namespace StockportWebappTests_Unit.Unit.Controllers;

public class EventsControllerTest
{
    private readonly EventsController _controller;
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<IProcessedContentRepository> _processedContentRepository = new();
    private readonly Event _eventsItem;
    private readonly HttpResponse responseListing;
    private readonly HttpResponse _responseDetail;
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new();
    private readonly Mock<IRssFeedFactory> _mockRssFeedFactory = new();
    private readonly Mock<ILogger<EventsController>> _logger = new();
    private readonly Mock<IApplicationConfiguration> _config = new();
    private const string BusinessId = "businessId";
    private readonly Mock<IFilteredUrl> _filteredUrl = new();
    private readonly DateCalculator _datetimeCalculator;
    private readonly Mock<IStockportApiEventsService> _stockportApiEventsService = new();
    private readonly CalendarHelper _calendarhelper = new();
    private readonly Group _group = new GroupBuilder().Build();

    private readonly List<Alert> _alerts = new()
    {
        new Alert("title",
                "body",
                "severity",
                new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                string.Empty,
                false,
                string.Empty)
    };

    private readonly List<Alert> _globalAlerts = new()
    {
        new Alert("global alert title",
                "global alert body",
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

        Mock<ITimeProvider> mockTime = new();
        _datetimeCalculator = new DateCalculator(mockTime.Object);

        EventResponse eventsCalendar = new(new List<Event> { _eventsItem }, new List<Event>());
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
                                        string.Empty,
                                        string.Empty,
                                        string.Empty,
                                        new(),
                                        new List<CallToActionBanner>());

        EventHomepage eventHomepage = new(new List<Alert>())
        {
            Categories = new(),
            Rows = new()
        };

        responseListing = new HttpResponse(200, eventsCalendar, string.Empty);
        _responseDetail = new HttpResponse(200, eventItem, string.Empty);
        HttpResponse responseHomepage = new(200, eventHomepage, string.Empty);
        HttpResponse response404 = new(404, null, "not found");

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

        _applicationConfiguration
            .Setup(appConfig => appConfig.GetEmailEmailFrom(It.IsAny<string>()))
            .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

        _config
            .Setup(config => config.GetRssEmail(BusinessId))
            .Returns(AppSetting.GetAppSetting("rss-email"));
        
        _config
            .Setup(config => config.GetEmailAlertsNewSubscriberUrl(BusinessId))
            .Returns(AppSetting.GetAppSetting("email-alerts-url"));

        _controller = new EventsController(
            _repository.Object,
            _processedContentRepository.Object,
            _mockRssFeedFactory.Object,
            _logger.Object,
            _config.Object,
            new BusinessId(BusinessId),
            _filteredUrl.Object,
            _calendarhelper,
            _datetimeCalculator,
            _stockportApiEventsService.Object);
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
            DateRange = "customdate",
            Price = ["5", "10"],
            Longitude = 12345.60,
            Latitude = 12345.61,
            Free = true,
            DateSelection = DateTime.Now.ToString("yyyy-MM-dd")
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
        Assert.Equal(new List<string>() { "5", "10" } , events.Price);
        Assert.Equal(12345.60, events.Longitude);
        Assert.Equal(12345.61, events.Latitude);
        Assert.True(events.Free);
        Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), events.DateSelection);
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
        Assert.Equal(_alerts.First().SunriseDate, model.Alerts.First().SunriseDate);
        Assert.Equal(_alerts.First().SunsetDate, model.Alerts.First().SunsetDate);
    }

    [Fact]
    public async Task Detail_ShouldReturnEventWhenDateQueryPassedIn()
    {
        // Arrange
        _processedContentRepository
            .Setup(processedContentRepo => processedContentRepo.Get<Event>("event-of-the-century",
                                                                        It.Is<List<Query>>(query => query.Contains(new Query("date", new DateTime().ToString("yyyy-MM-dd"))))))
            .ReturnsAsync(_responseDetail);

        // Act
        await _controller.Detail("event-of-the-century", new DateTime());
        
        // Assert
        _processedContentRepository
            .Verify(processedContentRepo => processedContentRepo.Get<Event>("event-of-the-century",
                                                                            It.Is<List<Query>>(query => query.Contains(new Query("date", new DateTime().ToString("yyyy-MM-dd"))))));
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
    public async Task Index_ClearsModelStateErrors_ForDateToAndDateFrom()
    {
        // Arrange
        EventCalendar eventCalendar = new();
        _controller.ModelState.AddModelError("DateTo", "DateTo is invalid");
        _controller.ModelState.AddModelError("DateFrom", "DateFrom is invalid");

        // Act
        await _controller.Index(eventCalendar, 1, 1);

        // Assert
        Assert.Empty(_controller.ModelState["DateTo"].Errors);
        Assert.Empty(_controller.ModelState["DateFrom"].Errors);
    }

    [Fact]
    public void Index_DoesNotThrow_WhenDateToOrDateFromModelStateKeyIsMissing()
    {
        // Act
        Exception exception = Record.Exception(() => _controller.Index(new EventCalendar(), 1, 1).Wait());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task Index_AssignsTagToKeepTag_WhenTagIsNotEmpty()
    {
        // Arrange
        EventCalendar eventCalendar = new() { Tag = "music" };

        // Act
        await _controller.Index(eventCalendar, 1, 1);

        // Assert
        Assert.Equal("music", eventCalendar.KeepTag);
    }

    [Fact]
    public async Task Index_DoesNotAssignKeepTag_WhenTagIsEmpty()
    {
        // Arrange
        EventCalendar eventCalendar = new() { Tag = string.Empty };

        // Act
        await _controller.Index(eventCalendar, 1, 1);

        // Assert
        Assert.Null(eventCalendar.KeepTag);
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
    
    [Fact]
    public async Task IndexWithCategory_ReturnsViewResult_WithExpectedModel()
    {
        // Arrange
        List<Event> events = new() { new Event { Title = "Concert" } };
        List<EventCategory> categories = new() { new EventCategory { Slug = "music", Name = "Music Events" } };

        _stockportApiEventsService
            .Setup(service => service.GetEventsByCategory("music", false))
            .ReturnsAsync(events);
        
        _stockportApiEventsService
            .Setup(service => service.GetEventCategories())
            .ReturnsAsync(categories);

        // Act
        IActionResult result = await _controller.IndexWithCategory("music", 1, 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        EventResultsViewModel model = Assert.IsAssignableFrom<EventResultsViewModel>(viewResult.Model);
        Assert.Equal("Music Events", model.Title);
        Assert.Equal(events, model.Events);
    }

    [Fact]
    public async Task IndexWithCategory_ReturnsIndexView_WhenEventsAreNull()
    {
        // Arrange
        EventResultsViewModel viewModel = new() { Title = "music" };
        _stockportApiEventsService
            .Setup(service => service.GetEventsByCategory("music", It.IsAny<bool>()))
            .ReturnsAsync((List<Event>)null);

        // Act
        IActionResult result = await _controller.IndexWithCategory("music", page: 1, pageSize: 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(viewModel.Title, ((EventResultsViewModel)viewResult.Model).Title);
    }

    [Fact]
    public async Task IndexWithCategory_ReturnsIndexView_WhenEventsAreEmpty()
    {
        // Arrange
        EventResultsViewModel viewModel = new() { Title = "music" };
        _stockportApiEventsService
            .Setup(service => service.GetEventsByCategory("music", It.IsAny<bool>()))
            .ReturnsAsync(new List<Event>());

        // Act
        IActionResult result = await _controller.IndexWithCategory("music", page: 1, pageSize: 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(viewModel.Title, ((EventResultsViewModel)viewResult.Model).Title);
    }

    [Fact]
    public async Task EventDetail_ReturnsViewResult_WithExpectedModel_WhenEventExists()
    {
        // Arrange
        ProcessedEvents eventItem = new("Special Event",
                                        "event-slug",
                                        "teaser",
                                        "imageUrl",
                                        "thumbnailImageUrl",
                                        "description",
                                        "fee",
                                        false,
                                        "location",
                                        "submittedBy",
                                        new DateTime(),
                                        "start time",
                                        "end time",
                                        new List<Crumb>(),
                                        new MapDetails(),
                                        "booking information",
                                        null,
                                        new List<Alert>(),
                                        "logo title",
                                        new List<GroupBranding>(),
                                        "0123456789",
                                        "email",
                                        "website",
                                        "facebook",
                                        "instagram",
                                        "linkedIn",
                                        "metadescription",
                                        "duration",
                                        "languages",
                                        new List<ProcessedEvents>(),
                                        new List<CallToActionBanner>());

        _stockportApiEventsService
            .Setup(service => service.GetProcessedEvent("event-slug", null))
            .ReturnsAsync(eventItem);

        // Act
        IActionResult result = await _controller.EventDetail("event-slug");

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Detail", viewResult.ViewName);
        Assert.Equal(eventItem, viewResult.Model);
    }

    [Fact]
    public void AddToCalendar_ReturnsRedirectResult_ForGoogleCalendar()
    {
        // Act
        IActionResult result = _controller.AddToCalendar("google",
                                                        "http://example.com/event",
                                                        "event-slug",
                                                        new DateTime(2024, 10, 12),
                                                        "Sample Event",
                                                        "Some Place",
                                                        "10:00",
                                                        "12:00",
                                                        "Event Description",
                                                        "Event Summary");

        // Assert
        RedirectResult redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("https://www.google.com/calendar/render?action=TEMPLATE&text=Sample Event&dates=20241012T100000/20241012T120000&details=For+details,+link+here: http://example.com/event &location=Some Place&sf=true&output=xml", redirectResult.Url);   
    }

    [Fact]
    public void AddToCalendar_ReturnsRedirectResult_ForYahooCalendar()
    {
        // Act
        IActionResult result = _controller.AddToCalendar("yahoo",
                                                        "http://example.com/event",
                                                        "event-slug",
                                                        new DateTime(2024, 10, 12),
                                                        "Sample Event",
                                                        "Some Place",
                                                        "10:00",
                                                        "12:00",
                                                        "Event Description",
                                                        "Event Summary");

        // Assert
        RedirectResult redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("https://calendar.yahoo.com/?v=60&view=d&type=20&title=Sample Event&st=20241012T100000&et=20241012T120000&desc=For+details,+link+here: http://example.com/event&in_loc=Some Place", redirectResult.Url);   
    }

    [Fact]
    public void AddToCalendar_ReturnsIcsFile_WhenTypeIsWindows()
    {

        // Act
        FileContentResult result = _controller.AddToCalendar("windows",
                                                            "url",
                                                            "slug",
                                                            DateTime.Now,
                                                            "name",
                                                            "location",
                                                            "start",
                                                            "end",
                                                            "desc",
                                                            "summary") as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("text/calendar", result.ContentType);
        Assert.Equal("slug.ics", result.FileDownloadName);
    }
    
    [Fact]
    public void AddToCalendar_ReturnsIcsFile_WhenTypeIsApple()
    {
        // Act
        FileContentResult result = _controller.AddToCalendar("apple",
                                                            "url",
                                                            "slug",
                                                            DateTime.Now,
                                                            "name",
                                                            "location",
                                                            "start",
                                                            "end",
                                                            "desc",
                                                            "summary") as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("text/calendar", result.ContentType);
        Assert.Equal("slug.ics", result.FileDownloadName);
    }

    [Fact]
    public void AddToCalendar_ReturnsNotFoundIfTypeIsEmpty()
    {
        // Act
        IActionResult result = _controller.AddToCalendar(string.Empty,
                                                            "url",
                                                            "slug",
                                                            DateTime.Now,
                                                            "name",
                                                            "location",
                                                            "start",
                                                            "end",
                                                            "desc",
                                                            "summary");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private EventsController SetUpController(int numItems)
    {
        List<Event> listOfEvents = BuildEventList(numItems);
        EventResponse eventsCalendar = new(listOfEvents, new List<Event>());
        HttpResponse eventListResponse = new(200, eventsCalendar, string.Empty);

        _repository
            .Setup(repo => repo.Get<EventResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(eventListResponse);

        return new EventsController(_repository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object,
                                        null,
                                        _datetimeCalculator,
                                        null);
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