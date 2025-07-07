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

    [Fact]
    public void GetActiveDateFilter_ReturnsEmptyString_WhenDateFromIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = null,
            DateTo = DateTime.Today,
            DateRange = "customdate"
        };


        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Empty(result);
    }

       [Fact]
    public void GetActiveDateFilter_ReturnsEmptyString_WhenDateToIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = DateTime.Today,
            DateTo = null,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveDateFilter_ReturnsSingleDate_WhenDateRangeIsCustomdate_AndDatesAreEqual()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 7),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("07/07/2025", result);
    }

      [Fact]
    public void GetActiveDateFilter_ReturnsDateRange_WhenDateRangeIsCustomdate_AndDatesAreDifferent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 10),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("07/07/2025 to 10/07/2025", result);
    }

    [Fact]
    public void GetActiveDateFilter_ReturnsMonthYear_WhenDateRangeIsNotCustomdate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 20),
            DateRange = "month"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("July 2025", result);
    }

     [Fact]
    public void GetActiveYearFilter_ReturnsEmptyString_WhenDateFromIsNull()
    {
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = null,
            DateTo = DateTime.Today,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsEmptyString_WhenDateToIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = DateTime.Today,
            DateTo = null,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsSingleDate_WhenDateRangeIsCustomdate_AndDatesAreEqual()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 7),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("07/07/2025", result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsDateRange_WhenDateRangeIsCustomdate_AndDatesAreDifferent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 10),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("07/07/2025 to 10/07/2025", result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsYear_WhenDateRangeIsNotCustomdate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 20),
            DateRange = "year"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("2025", result);
    }

    private static Newsroom BuildNewsRoom(List<string> categories = null, string emailAlertsTopicId = "") =>
        new(new List<News>(),
            null,
            null,
            null,
            null,
            new List<Alert>(),
            true,
            emailAlertsTopicId,
            categories ?? emptyList,
            new List<DateTime>(),
             new List<int>(),
            null);
}