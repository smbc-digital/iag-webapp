namespace StockportWebappTests_Unit.Unit.Services;

public class EventsServiceTests
{
    private readonly Mock<IRepository> _mockEventsRepository = new();
    private readonly EventsService _service;

    public EventsServiceTests() =>
        _service = new EventsService(_mockEventsRepository.Object);

    [Fact]
    public async Task GetEventsByLimit_ShouldReturnEventsList_WhenLimitIsProvided()
    {
        // Arrange
        List<Event> expectedEvents = new()
        {
            new Event { Title = "Event 1" },
            new Event { Title = "Event 2" },
            new Event { Title = "Event 3" }
        };

        _mockEventsRepository
            .Setup(repo => repo.GetLatest<EventCalendar>(5))
            .ReturnsAsync(new HttpResponse(200, expectedEvents, null));

        // Act
        List<Event> result = await _service.GetEventsByLimit(5);

        // Assert
        Assert.Equal(expectedEvents, result);
    }

    [Fact]
    public async Task GetLatestEventsItem_ShouldReturnNull_WhenNoEventsExist()
    {
        // Arrange
        EventResponse eventCalendar = new(new List<Event>(), new List<string>(), new List<Event>());

        _mockEventsRepository.Setup(repo => repo.GetLatest<EventCalendar>(1))
            .ReturnsAsync(new HttpResponse(200, eventCalendar, null));

        // Act
        Event result = await _service.GetLatestEventsItem();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetLatestFeaturedEventItem_ShouldReturnNull_WhenNoFeaturedEventsExist()
    {
        // Arrange
        EventCalendar eventCalendar = new(new List<Event>(), new List<string>());

        _mockEventsRepository
            .Setup(repo => repo.GetLatestOrderByFeatured<EventCalendar>(1))
            .ReturnsAsync(new HttpResponse(200, eventCalendar, null));

        // Act
        Event result = await _service.GetLatestFeaturedEventItem();

        // Assert
        Assert.Null(result);
    }
}