namespace StockportWebappTests_Unit.Unit.ViewModels;

public class NewsroomViewModelTest
{
    private static readonly List<string> emptyList = new();
    private const string EmailAlertsUrl = "url";
    private readonly Newsroom _newsroom;

    public NewsroomViewModelTest() =>
        _newsroom = BuildNewsRoom(emailAlertsTopicId: "tag-id");

    [Fact]
    public void ShouldSetEmailAlertsUrlWithTopicId()
    {
        // Act
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(emailAlertsTopicId: "tag-id"), EmailAlertsUrl);

        // Assert
        Assert.Equal(string.Concat(EmailAlertsUrl, "?topic_id=", _newsroom.EmailAlertsTopicId), newsroomViewModel.EmailAlertsUrl);
    }

    [Fact]
    public void ShouldSetEmailAlertsUrlWithoutTopicId()
    {
        // Act
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(emailAlertsTopicId: string.Empty), EmailAlertsUrl);

        // Assert
        Assert.Equal(EmailAlertsUrl, newsroomViewModel.EmailAlertsUrl);
    }

    [Fact]
    public void ShouldGiveCategoriesInAlphabeticalOrder()
    {
        // Act
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });

        NewsroomViewModel newsroomViewModel = new(newsroom, EmailAlertsUrl);

        // Assert
        List<string> expectedOrder = new() { "Asses", "Oxen", "Zebras" };
        Assert.Equal(expectedOrder, newsroomViewModel.Categories);
    }

    [Fact]
    public void ShouldReturnToAndFromDatesIfDateRangeIsCustomDate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 02, 01)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("01/01/2016 to 01/02/2016", result);
    }

    [Fact]
    public void ShouldReturnMonthNameIfDateRangeIsMonth()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 31)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("January 2016", result);
    }

    [Fact]
    public void ShouldDisplaySingleDateIfToDateAndFromDateAreTheSame()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 01)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("01/01/2016", result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnFalseWhenNoActiveFiltersExsist()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom, EmailAlertsUrl);
        
        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnTrueWhenActiveFiltersExist()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom, EmailAlertsUrl);
        newsroomViewModel.Category = "Zebras";
        newsroomViewModel.Tag = "Tag";
        newsroomViewModel.DateFrom = DateTime.Now.AddDays(-5);
        newsroomViewModel.DateTo = DateTime.Now;

        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnFalseWhenDateFilterIsInvalid()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom, EmailAlertsUrl);
        newsroomViewModel.DateFrom = DateTime.Now;
        newsroomViewModel.DateTo = DateTime.Now.AddDays(-5);

        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.False(result);
    }

    private static Newsroom BuildNewsRoom(List<string> categories = null, string emailAlertsTopicId = "") =>
        new(new List<News>(),
            new List<Alert>(),
            true,
            emailAlertsTopicId,
            categories ?? emptyList,
            new List<DateTime>(),
            null);
}