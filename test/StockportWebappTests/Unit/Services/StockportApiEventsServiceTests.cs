namespace StockportWebappTests_Unit.Unit.Services;

public class StockportApiEventsServiceTests
{
    private Mock<IStockportApiRepository> _stockportApiRepository = new();
    private Mock<IUrlGeneratorSimple> _urlGeneratorSimple = new();
    private StockportApiEventsService _stockportApiEventsService;
    private Mock<IEventFactory> _eventFactory = new();

    public StockportApiEventsServiceTests() =>
        _stockportApiEventsService = new(_stockportApiRepository.Object, _urlGeneratorSimple.Object, _eventFactory.Object);

    [Fact]
    public async Task GetEventsByCategory_ShouldReturnListOfEventsWhenCategorySet()
    {
        // Arrange
        List<Event> builtEvents = new() { new EventBuilder().Build() };
        _stockportApiRepository
            .Setup(x => x.GetResponse<List<Event>>("by-category", It.IsAny<List<Query>>()))
            .ReturnsAsync(builtEvents);

        // Act
        List<Event> result = await _stockportApiEventsService.GetEventsByCategory("Fayre");

        // Assert
        Assert.Single(result);
        Assert.Equal("title", result.First().Title);
    }

    [Fact]
    public async Task GetEventsByCategory_ShouldReturnNullWhenCategorySetIsEmpty()
    {
        // Arrange
        List<Event> builtEvents = new() { new EventBuilder().Build() };
        _stockportApiRepository
            .Setup(x => x.GetResponse<List<Event>>("by-category", It.IsAny<List<Query>>()))
            .ReturnsAsync((List<Event>)null);

        // Act
        List<Event> result = await _stockportApiEventsService.GetEventsByCategory("");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventCategories_ShouldReturnListOfEventCategories()
    {
        // Arrange
        List<EventCategory> expectedCategories = new()
        {
            new EventCategory { Name = "Category1" },
            new EventCategory { Name = "Category2" }
        };

        _stockportApiRepository
            .Setup(repo => repo.GetResponse<List<EventCategory>>())
            .ReturnsAsync(expectedCategories);

        // Act
        List<EventCategory> result = await _stockportApiEventsService.GetEventCategories();

        // Assert
        Assert.Equal(expectedCategories.Count, result.Count);
        Assert.Equal(expectedCategories, result);
    }

    [Fact]
    public async Task GetProcessedEvent_ShouldReturnProcessedEvent_WhenEventExists()
    {
        // Arrange
        Event eventItem = new() { Title = "Event Title" };
        ProcessedEvents processedEvent = new("Event Title",
                                            "slug",
                                            "teaser",
                                            "imageUrl",
                                            "thumbnailImageUrl",
                                            "description",
                                            "fee",
                                            false,
                                            "location",
                                            "submitted by",
                                            new DateTime(),
                                            "10:00",
                                            "11:00",
                                            new List<Crumb>(),
                                            new List<string>(),
                                            new MapDetails(),
                                            "booking information",
                                            null,
                                            new List<Alert>(),
                                            "accessible transport link",
                                            new List<GroupBranding>(),
                                            "0123456789",
                                            "test@email.com",
                                            "website",
                                            "facebook",
                                            "instagram",
                                            "linkedin",
                                            "meta description",
                                            "duration",
                                            "languages",
                                            new List<ProcessedEvents>());

        _stockportApiRepository
            .Setup(repo => repo.GetResponse<Event>("event-slug", It.Is<List<Query>>(q => q.Exists(x => x.Name.Equals("date") && x.Value.Equals(new DateTime(2024, 11, 13).ToString("yyyy-MM-dd"))))))
            .ReturnsAsync(eventItem);

        _eventFactory
            .Setup(factory => factory.Build(eventItem))
            .Returns(processedEvent);

        // Act
        ProcessedEvents result = await _stockportApiEventsService.GetProcessedEvent("event-slug", new DateTime(2024, 11, 13));

        // Assert
        Assert.Equal(processedEvent, result);
    }

    [Fact]
    public async Task GetProcessedEvent_ShouldReturnNull_WhenEventDoesNotExist()
    {
        // Arrange
        _stockportApiRepository
            .Setup(repo => repo.GetResponse<Event>("non-existent-event", It.IsAny<List<Query>>()))
            .ReturnsAsync((Event)null);

        // Act
        ProcessedEvents result = await _stockportApiEventsService.GetProcessedEvent("non-existent-event", null);

        // Assert
        Assert.Null(result);
    }
}