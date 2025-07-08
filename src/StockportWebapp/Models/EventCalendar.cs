namespace StockportWebapp.Models;

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

    public EventCalendar(List<Event> events)
    {
        Events = events;
    }

    public bool IsFromSearch() => FromSearch 
        || Free
        || !string.IsNullOrWhiteSpace(Category)
        || !string.IsNullOrWhiteSpace(Tag)
        || DateFrom is not null
        || DateTo is not null;

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

    public void AddFilteredUrl(IFilteredUrl filteredUrl) =>
        FilteredUrl = filteredUrl;

    public void AddQueryUrl(QueryUrl queryUrl) =>
        CurrentUrl = queryUrl;

    public bool ShowPagination =>
        Pagination is not null && Pagination.TotalItems > Pagination.MaxItemsPerPage && IsFromSearch();

    public string DisplayTitle =>
        string.IsNullOrEmpty(Category)
            ? "What's on in Stockport"
            : "Results for " + SelectedCategory?.Name ?? string.Empty;

    public string PageTitle =>
        $"{DisplayTitle}{(ShowPagination
            ? $" (page {Pagination.CurrentPageNumber} of {Pagination.TotalPages})"
            : string.Empty)}";
}