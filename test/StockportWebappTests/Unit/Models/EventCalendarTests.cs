using Microsoft.AspNetCore.Mvc.Rendering;
namespace StockportWebappTests_Unit.Unit.Models;

public class EventCalendarTests
{
    [Fact]
    public void AddCarouselContents_ShouldAddItemsIfNotNull()
    {
        // Arrange
        EventCalendar eventCalendar = new();
        Event eventToAdd = new()
        {
            Title = "Title",
            Slug = "Slug",
            EventDate = DateTime.Today,
            Teaser = "Teaser",
            ImageUrl = "ImageUrl"
        };

        // Act
        eventCalendar.AddCarouselContents(new List<Event> { eventToAdd });

        // Assert
        Assert.Single(eventCalendar.CarouselContents);
    }

    [Fact]
    public void AddCarouselContents_ShouldNotAddItemsIfNull()
    {
        // Arrange
        EventCalendar eventCalendar = new();

        // Act
        eventCalendar.AddCarouselContents(null);

        // Assert
        Assert.Empty(eventCalendar.CarouselContents);
    }

    [Fact]
    public void AddCarouselContents_ShouldNotAddItemsIfEventsEmpty()
    {
        // Arrange
        EventCalendar eventCalendar = new();

        // Act
        eventCalendar.AddCarouselContents(new List<Event>());
        
        // Assert
        Assert.Empty(eventCalendar.CarouselContents);
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenFromSearchIsTrue()
    {
        // Arrange
        EventCalendar eventCalendar = new() { FromSearch = true };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenFreeIsTrue()
    {
        // Arrange
        EventCalendar eventCalendar = new() { Free = true };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenCategoryIsNotNullOrWhitespace()
    {
        // Arrange
        EventCalendar eventCalendar = new() { Category = "Music" };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenTagIsNotNullOrWhitespace()
    {
        // Arrange
        EventCalendar eventCalendar = new() { Tag = "Concert" };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenDateFromIsNotNull()
    {
        // Arrange
        EventCalendar eventCalendar = new() { DateFrom = DateTime.Now };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsTrue_WhenDateToIsNotNull()
    {
        // Arrange
        EventCalendar eventCalendar = new() { DateTo = DateTime.Now.AddDays(1) };

        // Act & Assert
        Assert.True(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void IsFromSearch_ReturnsFalse_WhenAllConditionsAreFalse()
    {
        // Arrange
        EventCalendar eventCalendar = new();

        // Act & Assert
        Assert.False(eventCalendar.IsFromSearch());
    }

    [Fact]
    public void EventCategoryOptions_ReturnsDefaultCategory_WhenNoCategoriesProvided()
    {
        // Arrange
        EventHomepage homepage = new(new List<Alert>()) { Categories = new List<EventCategory>() };
        EventCalendar eventCalendar = new() { Homepage = homepage };

        // Act
        List<SelectListItem> result = eventCalendar.EventCategoryOptions();

        // Assert
        Assert.Single(result);
        Assert.Equal("All categories", result[0].Text);
        Assert.Equal(string.Empty, result[0].Value);
    }

    [Fact]
    public void EventCategoryOptions_ReturnsCategories_WhenCategoriesProvided()
    {
        // Arrange
        EventHomepage homepage = new(new List<Alert>())
        {
            Categories = new List<EventCategory>
            {
                new() { Name = "Music", Slug = "music" },
                new() { Name = "Sports", Slug = "sports" }
            }
        };

        EventCalendar eventCalendar = new() { Homepage = homepage };

        // Act
        List<SelectListItem> result = eventCalendar.EventCategoryOptions();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("All categories", result[0].Text);
        Assert.Equal(string.Empty, result[0].Value);
        Assert.Equal("Music", result[1].Text);
        Assert.Equal("music", result[1].Value);
        Assert.Equal("Sports", result[2].Text);
        Assert.Equal("sports", result[2].Value);
    }
}