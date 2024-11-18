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

    public string DateRange { get; set; }

    public List<Event> Events { get; private set; } = new();
    public List<string> Categories { get; private set; } = new();
    public string Tag { get; set; }
    public string[] Price { get; set; }
    public string HomepageTags { get; set; }
    public IFilteredUrl FilteredUrl { get; private set; }
    public QueryUrl CurrentUrl { get; private set; }
    public Pagination Pagination { get; set; }

    public EventHomepage Homepage { get; set; }
    public List<HeroCarouselItem> HeroCarouselItems { get; set; } = new();

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

    public bool DoesCategoryExist(string categoryItem) =>
        Categories.Contains(categoryItem);

    public void AddEvents(List<Event> events) =>
        Events = events;

    public void AddHeroCarouselItems(List<Event> events)
    {
        if (events is not null)
            HeroCarouselItems = events.Select(evnt => new HeroCarouselItem
            {
                Title = evnt.Title,
                Link = evnt.Slug,
                Date = evnt.EventDate.ToString("dddd dd MMMM yyyy"),
                Teaser = evnt.Teaser,
                ImageSrc = evnt.ImageUrl
            }).ToList();
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
                new() { Label = "Paid", Checked = Price is null || Price.Any(p => p.Equals("paid")), Value = "paid" },
                new() { Label = "Free", Checked = Price is null || Price.Any(p => p.Equals("free")), Value = "free" }
            }
        };

        bar.Filters.Add(price);

        return bar;
    }
}