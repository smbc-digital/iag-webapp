namespace StockportWebappTests_Unit.Unit.Utils;

public class DateCalculatorTest
{
    private readonly Mock<ITimeProvider> _mockTimeProvider = new();

    [Fact]
    public void ShouldGetTodayDate()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 02));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);
        
        // Assert
        Assert.Equal("2016-08-02", dateCalculator.Today());
    }

    [Fact]
    public void ShouldGetTomorrowDate()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 02));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);
        
        // Assert
        Assert.Equal("2016-08-03", dateCalculator.Tomorrow());
    }

    [Fact]
    public void ShouldGetNearestFriday()
    {
        // Arrange
        _mockTimeProvider.Setup(o => o.Today()).Returns(new DateTime(2016, 08, 02));
        
        // Act
        DateCalculator dateCalculator = new DateCalculator(_mockTimeProvider.Object);
        
        // Assert
        dateCalculator.NearestFriday().Should().Be("2016-08-05");
    }

    [Fact]
    public void ShouldGetTodayAsNearestFridayIfTodayIsFriday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 05));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-05", dateCalculator.NearestFriday());
    }

    [Fact]
    public void ShouldGetTodayAsNearestFridayIfTodayIsSaturday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 06));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-06", dateCalculator.NearestFriday());
    }

    [Fact]
    public void ShouldGetTodayAsNearestFridayIfTodayIsSunday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 07));

        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-07", dateCalculator.NearestFriday());
    }

    [Fact]
    public void ShouldGetNearestSunday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 02));

        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-07", dateCalculator.NearestSunday());
    }

    [Fact]
    public void ShouldGetTodayAsNearestSundayIfTodayIsSunday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 07));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-07", dateCalculator.NearestSunday());
    }

    [Fact]
    public void ShouldGetNearestMonday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 02));

        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-08", dateCalculator.NearestMonday());
    }

    [Fact]
    public void ShouldGetNextMondayIfTodayIsMonday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 01));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-08", dateCalculator.NearestMonday());
    }

    [Fact]
    public void ShouldGetNextSunday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 02));

        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-14", dateCalculator.NextSunday());
    }

    [Fact]
    public void ShouldGetNextSundayIfTodayIsSunday()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2016, 08, 07));
        
        // Act
        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Assert
        Assert.Equal("2016-08-14", dateCalculator.NextSunday());
    }

    [Fact]
    public void ShouldGetValueForKeyForFilterIfKeyExists()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        string value = dateCalculator.ReturnDisplayNameForFilter("today");

        // Assert
        Assert.Equal("Today", value);
    }

    [Fact]
    public void ShouldGetValueForKeyForFilterIfKeyExistsForAnotherKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        string value = dateCalculator.ReturnDisplayNameForFilter("thisweekend");

        // Assert
        Assert.Equal("This weekend", value);
    }

    [Fact]
    public void ShouldNotGiveAValueForNoExistantKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        string value = dateCalculator.ReturnDisplayNameForFilter("none");

        // Assert
        Assert.Empty(value);
    }

    [Fact]
    public void ShouldReturnEmptyStringForNullKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        string value = dateCalculator.ReturnDisplayNameForFilter(null);

        // Assert
        Assert.Empty(value);
    }

    [Fact]
    public void ShouldReturnEmptyStringForEmptyKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        string value = dateCalculator.ReturnDisplayNameForFilter(string.Empty);

        // Assert
        Assert.Empty(value);
    }

    [Fact]
    public void ShouldReturnAEventFilterForAKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        EventFilter filter = dateCalculator.ReturnFilterForKey("today");

        // Assert
        Assert.NotNull(filter);
        Assert.Equal("2017-12-25", filter.DateFrom);
        Assert.Equal("2017-12-25", filter.DateTo);
        Assert.Equal("Today", filter.DateRange);
    }

    [Fact]
    public void ShouldReturnEmptyFilterForNotFoundKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        EventFilter filter = dateCalculator.ReturnFilterForKey("notfoundkey");

        // Assert
        Assert.NotNull(filter);
        Assert.Empty(filter.DateRange);
        Assert.Empty(filter.DateTo);
        Assert.Empty(filter.DateFrom);
    }

    [Fact]
    public void ShouldReturnEmptyFilterForNullKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        EventFilter filter = dateCalculator.ReturnFilterForKey(null);

        // Assert
        Assert.NotNull(filter);
        Assert.Empty(filter.DateRange);
        Assert.Empty(filter.DateTo);
        Assert.Empty(filter.DateFrom);
    }

    [Fact]
    public void ShouldReturnEmptyFilterForStringEmptyKey()
    {
        // Arrange
        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(new DateTime(2017, 12, 25));

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);

        // Act
        EventFilter filter = dateCalculator.ReturnFilterForKey(string.Empty);

        // Assert
        Assert.NotNull(filter);
        Assert.Empty(filter.DateRange);
        Assert.Empty(filter.DateTo);
        Assert.Empty(filter.DateFrom);
    }

    [Theory]
    [InlineData(0, EventFrequency.None, 1)]
    [InlineData(3, EventFrequency.Daily, 3)]
    [InlineData(21, EventFrequency.Weekly, 3)]
    [InlineData(42, EventFrequency.Fortnightly, 3)]
    [InlineData(31, EventFrequency.Monthly, 1)]
    [InlineData(31, EventFrequency.MonthlyDate, 1)]
    [InlineData(365, EventFrequency.Yearly, 1)]
    public void ShowReturnCorrectEndDateForReoccurringEvents(int daysHence, EventFrequency freq, int occurences)
    {
        // Arrange
        DateTime date = new(2017, 12, 25);

        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(date);

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);
        Event testEvent = new() { EventFrequency = freq, Occurences = occurences, EventDate = date };

        // Act
        DateTime enddate = dateCalculator.GetEventEndDate(testEvent);

        // Assert
        Assert.Equal(testEvent.EventDate.AddDays(daysHence), enddate);
    }

    [Theory]
    [InlineData(0, EventFrequency.None, 1)]
    [InlineData(3, EventFrequency.Daily, 4)]
    [InlineData(6, EventFrequency.Weekly, 1)]
    [InlineData(7, EventFrequency.Weekly, 2)]
    [InlineData(20, EventFrequency.Weekly, 3)]
    [InlineData(21, EventFrequency.Weekly, 4)]
    [InlineData(22, EventFrequency.Weekly, 4)]
    [InlineData(13, EventFrequency.Fortnightly, 1)]
    [InlineData(15, EventFrequency.Fortnightly, 2)]
    [InlineData(42, EventFrequency.Fortnightly, 4)]
    [InlineData(31, EventFrequency.Monthly, 2)]
    [InlineData(65, EventFrequency.MonthlyDate, 3)]
    [InlineData(360, EventFrequency.Yearly, 1)]
    [InlineData(365, EventFrequency.Yearly, 2)]
    [InlineData(370, EventFrequency.Yearly, 2)]
    public void GetEventOccurences_ShowReturnCorrectCountOfEventOccurences(int daysHence, EventFrequency freq, int occurences)
    {
        // Arrange
        DateTime date = new(2017, 12, 25);

        _mockTimeProvider
            .Setup(provider => provider.Today())
            .Returns(date);

        DateCalculator dateCalculator = new(_mockTimeProvider.Object);
        Event testEvent = new() { EventFrequency = freq, Occurences = occurences, EventDate = date };

        // Act
        int result = dateCalculator.GetEventOccurences(testEvent.EventFrequency, testEvent.EventDate, testEvent.EventDate.AddDays(daysHence));

        // Assert
        Assert.Equal(occurences, result);
    }
}