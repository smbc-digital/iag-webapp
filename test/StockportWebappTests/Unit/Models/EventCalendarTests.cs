using Microsoft.AspNetCore.Mvc.Rendering;
namespace StockportWebappTests_Unit.Unit.Models;

public class EventCalendarTests
{
    [Fact]
    public void DoesCategoryExist_ShouldReturnTrueForExistingCategory()
    {
        // Arrange
        EventCalendar eventCalendar = new(new List<Event>(), new List<string> { "test", "test2" });

        // Act & Assert
        Assert.True(eventCalendar.DoesCategoryExist("test"));
    }

    [Fact]
    public void DoesCategoryExist_ShouldReturnFalseForNonExistingCategory()
    {
        // Arrange
        EventCalendar eventCalendar = new(new List<Event>(), new List<string> { "test1", "test2" });

        // Act & Assert
        Assert.False(eventCalendar.DoesCategoryExist("test"));
    }

    [Fact]
    public void GetCustomEventFilterName_ShouldReturnDateRangeForCustomDate()
    {
        // Arrange
        EventCalendar eventCalendar = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 10)
        };

        // Act & Assert
        Assert.Equal("01/01/2016 to 10/01/2016", eventCalendar.GetCustomEventFilterName());
    }

    [Fact]
    public void GetCustomEventFilterName_ShouldReturnEmptyStringForBlankDates()
    {
        // Arrange
        EventCalendar eventCalendar = new();

        // Act & Assert
        Assert.Equal(string.Empty, eventCalendar.GetCustomEventFilterName());
    }

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

    [Fact]
    public void RefineByBar_ReturnsBar_WithShowLocationSetToTrue()
    {
        // Arrange
        EventCalendar eventModel = new();

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ShowLocation);
    }

    [Fact]
    public void RefineByBar_AddsFeaturedFilter_WhenKeepTagOrTagIsNotNullOrEmpty()
    {
        // Arrange
        EventCalendar eventModel = new()
        {
            KeepTag = "music",
            Tag = "music"
        };

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        RefineByFilters featuredFilter = result.Filters.FirstOrDefault(filter => filter.Name.Equals("tag"));
        Assert.NotNull(featuredFilter);
        Assert.Equal("Featured events", featuredFilter.Label);
        Assert.False(featuredFilter.Mandatory);
        Assert.Single(featuredFilter.Items);
        Assert.Equal("music", featuredFilter.Items[0].Label);
        Assert.Equal("music", featuredFilter.Items[0].Value);
        Assert.True(featuredFilter.Items[0].Checked);
    }

    [Fact]
    public void RefineByBar_DoesNotAddFeaturedFilter_WhenKeepTagAndTagAreNullOrEmpty()
    {
        // Arrange
        EventCalendar eventModel = new();

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        Assert.DoesNotContain(result.Filters, filter => filter.Name.Equals("tag"));
    }

    [Fact]
    public void RefineByBar_AddsPriceFilter_WithCorrectItemsAndDefaultChecks()
    {
        // Arrange
        EventCalendar eventModel = new();

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        RefineByFilters priceFilter = result.Filters.FirstOrDefault(filter => filter.Name.Equals("price"));
        Assert.NotNull(priceFilter);
        Assert.Equal("Price", priceFilter.Label);
        Assert.True(priceFilter.Mandatory);
        Assert.Equal(2, priceFilter.Items.Count);

        RefineByFilterItems paidItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("paid"));
        Assert.NotNull(paidItem);
        Assert.Equal("Paid", paidItem.Label);
        Assert.True(paidItem.Checked);

        RefineByFilterItems freeItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("free"));
        Assert.NotNull(freeItem);
        Assert.Equal("Free", freeItem.Label);
        Assert.True(freeItem.Checked);
    }

    [Fact]
    public void RefineByBar_ChecksPriceFilterItems_WhenPriceIncludesPaidOrFree()
    {
        // Arrange
        EventCalendar eventModel = new()
        {
            Price = ["paid"]
        };

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        RefineByFilters priceFilter = result.Filters.FirstOrDefault(f => f.Name.Equals("price"));
        Assert.NotNull(priceFilter);

        RefineByFilterItems paidItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("paid"));
        Assert.NotNull(paidItem);
        Assert.True(paidItem.Checked);

        RefineByFilterItems freeItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("free"));
        Assert.NotNull(freeItem);
        Assert.False(freeItem.Checked);
    }

    [Fact]
    public void RefineByBar_UnchecksBothPriceFilterItems_WhenPriceIsEmpty()
    {
        // Arrange
        EventCalendar eventModel = new()
        {
            Price = Array.Empty<string>()
        };

        // Act
        RefineByBar result = eventModel.RefineByBar();

        // Assert
        RefineByFilters priceFilter = result.Filters.FirstOrDefault(f => f.Name.Equals("price"));
        Assert.NotNull(priceFilter);

        RefineByFilterItems paidItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("paid"));
        Assert.NotNull(paidItem);
        Assert.False(paidItem.Checked);

        RefineByFilterItems freeItem = priceFilter.Items.FirstOrDefault(i => i.Value.Equals("free"));
        Assert.NotNull(freeItem);
        Assert.False(freeItem.Checked);
    }
}