namespace StockportWebappTests_Unit.Unit.Helpers;

public class CalendarHelperTest
{
    private readonly CalendarHelper _helper;

    public CalendarHelperTest() =>
        _helper = new();

    [Fact]
    public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnWindowsOrApple()
    {
        // Arrange
        Event eventItem = new()
        {
            Slug = "test-slug",
            Description = "Test Description",
            EventDate = new DateTime(2017, 12, 12),
            EndTime = "17:00",
            StartTime = "14:00",
            Location = "location"
        };

        // Act
        string calenderUrl = _helper.GetIcsText(eventItem, "www.test.com/test-event");

        // Assert
        Assert.Contains("LOCATION:location", calenderUrl);
    }

    [Fact]
    public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnGoogle()
    {
        // Arrange
        Event eventItem = new()
        {
            Slug = "test-slug",
            Description = "Test Description",
            EventDate = new DateTime(2017, 12, 12),
            EndTime = "14:00",
            StartTime = "18:00",
            Location = "location"
        };

        // Act
        string result = _helper.GetCalendarUrl(eventItem, "www.test.com/test-event", "google");

        // Assert
        Assert.StartsWith("https://www.google.com/calendar/render", result);
        Assert.Equal("https://www.google.com/calendar/render?action=TEMPLATE&text=&dates=20171212T180000/20171212T140000&details=For+details,+link+here: www.test.com/test-event &location=location&sf=true&output=xml", result);
    }

    [Fact]
    public void ShouldReturnCorrectCalendarUrlForEventWhenClickOnYahoo()
    {
        // Arrange
        Event eventItem = new()
        {
            Slug = "test-slug",
            Description = "Test Description",
            EventDate = new DateTime(2017, 12, 12),
            EndTime = "14:00",
            StartTime = "18:00",
            Location = "location"
        };

        // Act
        string result = _helper.GetCalendarUrl(eventItem, "www.test.com/test-event", "yahoo");

        // Assert
        Assert.StartsWith("https://calendar.yahoo.com/", result);
        Assert.Equal("https://calendar.yahoo.com/?v=60&view=d&type=20&title=&st=20171212T180000&et=20171212T140000&desc=For+details,+link+here: www.test.com/test-event&in_loc=location", result);
    }
}