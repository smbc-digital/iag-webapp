﻿namespace StockportWebappTests_Unit.Unit.Models;

public class EventCalendarTests
{
    [Fact]
    public void DoesCategoryExist_ShouldReturnTrueForExistingCategory()
    {
        EventCalendar eventCalendar = new(new List<Event>(), new List<string> { "test", "test2" });

        Assert.True(eventCalendar.DoesCategoryExist("test"));
    }

    [Fact]
    public void DoesCategoryExist_ShouldReturnFalseForNonExistingCategory()
    {
        EventCalendar eventCalendar = new(new List<Event>(), new List<string> { "test1", "test2" });

        Assert.False(eventCalendar.DoesCategoryExist("test"));
    }

    [Fact]
    public void GetCustomEventFilterName_ShouldReturnDateRangeForCustomDate()
    {
        EventCalendar eventCalendar = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 10)
        };

        string result = eventCalendar.GetCustomEventFilterName();

        Assert.Equal("01/01/2016 to 10/01/2016", result);
    }

    [Fact]
    public void GetCustomEventFilterName_ShouldReturnEmptyStringForBlankDates()
    {
        EventCalendar eventCalendar = new();

        string result = eventCalendar.GetCustomEventFilterName();

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void AddHeroCarouselItems_ShouldAddItemsIfNotNull()
    {
        EventCalendar eventCalendar = new();
        Event eventToAdd = new()
        {
            Title = "Title",
            Slug = "Slug",
            EventDate = DateTime.Today,
            Teaser = "Teaser",
            ImageUrl = "ImageUrl"
        };

        eventCalendar.AddHeroCarouselItems(new List<Event> { eventToAdd });

        Assert.Single(eventCalendar.HeroCarouselItems);
    }

    [Fact]
    public void AddHeroCarouselItems_ShouldNotAddItemsIfNull()
    {
        EventCalendar eventCalendar = new();

        eventCalendar.AddHeroCarouselItems(null);

        Assert.Empty(eventCalendar.HeroCarouselItems);
    }

    [Fact]
    public void AddHeroCarouselItems_ShouldNotAddItemsIfEventsEmpty()
    {
        EventCalendar eventCalendar = new();

        eventCalendar.AddHeroCarouselItems(new List<Event>());

        Assert.Empty(eventCalendar.HeroCarouselItems);
    }
}