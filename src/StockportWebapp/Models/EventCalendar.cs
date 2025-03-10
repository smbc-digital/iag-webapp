﻿namespace StockportWebapp.Models;

public class EventCalendar
{
    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [Display(Name = "End date")]
    [DataType(DataType.Date)]
    [EndDateLaterThanStartDateValidation("DateFrom", "End date should be on or after the start date")]
    public DateTime? DateTo { get; set; }
    public string Category { get; set; }
    public bool CategoryIsSelected =>
        !string.IsNullOrEmpty(Category);
    public EventCategory SelectedCategory =>
        CategoryIsSelected
            ? Homepage?.Categories?.SingleOrDefault(category => category.Slug.Equals(Category))
            : null;

    public string DateRange { get; set; }
    public string DateSelection { get; set; }
    public List<Event> Events { get; private set; } = new();
    public List<Event> FeaturedEvents { get; private set; } = new();
    public List<string> Categories { get; private set; } = new();
    public string Tag { get; set; }
    public bool Free { get; set; }
    public string[] Price { get; set; }
    public string HomepageTags { get; set; }
    public IFilteredUrl FilteredUrl { get; private set; }
    public QueryUrl CurrentUrl { get; private set; }
    public Pagination Pagination { get; set; }
    public EventHomepage Homepage { get; set; }
    public List<CarouselContent> CarouselContents { get; set; } = new();
    public bool ShouldScroll { get; set; } = false;
    public EventCalendar() { }

    public bool FromSearch { get; set; }
    public string KeepTag { get; set; }
    public string Location { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public EventCalendar(List<Event> events, List<string> categories)
    {
        Events = events;
        Categories = categories;
    }

    public bool IsFromSearch() => FromSearch 
        || Free
        || !string.IsNullOrWhiteSpace(Category)
        || !string.IsNullOrWhiteSpace(Tag)
        || DateFrom is not null
        || DateTo is not null;

    public bool DoesCategoryExist(string categoryItem) =>
        Categories.Contains(categoryItem);

    public void AddEvents(List<Event> events) =>
        Events = events;

    public void AddFeaturedEvents(List<Event> featuredEvents) =>
        FeaturedEvents = featuredEvents;
        
    public void AddCarouselContents(List<Event> events)
    {
        if (events is not null)
            CarouselContents = events
                .Select(evnt => new CarouselContent(evnt.Title,
                                                    evnt.Teaser,
                                                    evnt.ImageUrl,
                                                    evnt.Slug,
                                                    evnt.EventDate))
                .ToList();
    }

    public List<SelectListItem> CategoryOptions()
    {
        List<SelectListItem> result = new()
        {
            new SelectListItem { Text = "All categories", Value = string.Empty }
        };

        foreach (string cat in Categories)
        {
            result.Add(new SelectListItem { Text = cat, Value = cat });
        }

        return result;
    }

    public List<SelectListItem> EventCategoryOptions()
    {
        List<SelectListItem> result = new()
        {
            new SelectListItem { Text = "All categories", Value = string.Empty }
        };

        foreach (EventCategory cat in Homepage.Categories)
        {
            result.Add(new SelectListItem { Text = cat.Name, Value = cat.Slug });
        }

        return result;
    }

    public void AddCategories(List<string> categories) =>
        Categories = categories;

    public void AddFilteredUrl(IFilteredUrl filteredUrl) =>
        FilteredUrl = filteredUrl;

    public void AddQueryUrl(QueryUrl queryUrl) =>
        CurrentUrl = queryUrl;

    public string GetCustomEventFilterName() =>
        DateFrom.HasValue && DateTo.HasValue
            ? $"{DateFrom.Value:dd/MM/yyyy} to {DateTo.Value:dd/MM/yyyy}"
            : string.Empty;

    public RefineByBar RefineByBar()
    {
        RefineByBar bar = new()
        {
            ShowLocation = true,
            Filters = new List<RefineByFilters>()
        };

        if (!string.IsNullOrEmpty(KeepTag) || !string.IsNullOrEmpty(Tag))
        {
            RefineByFilters featured = new()
            {
                Label = "Featured events",
                Mandatory = false,
                Name = "tag",
                Items = new List<RefineByFilterItems>
                {
                    new() { Label = KeepTag, Checked = !string.IsNullOrEmpty(Tag), Value = KeepTag }
                }
            };

            bar.Filters.Add(featured);
        }

        RefineByFilters price = new()
        {
            Label = "Price",
            Mandatory = true,
            Name = "price",
            Items = new List<RefineByFilterItems>
            {
                new()
                {
                    Label = "Paid",
                    Checked = Price is null || Price.Any(p => p.Equals("paid")),
                    Value = "paid"
                },
                new()
                {
                    Label = "Free",
                    Checked = Price is null || Price.Any(p => p.Equals("free")),
                    Value = "free"
                }
            }
        };

        bar.Filters.Add(price);

        return bar;
    }
}