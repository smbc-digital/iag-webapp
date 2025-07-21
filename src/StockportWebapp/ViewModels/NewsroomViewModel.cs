namespace StockportWebapp.ViewModels;

public class NewsroomViewModel
{
    // data
    public string EmailAlertsUrl { get; private set; }
    public Newsroom Newsroom { get; private set; }

    public List<string> Categories =>
        Newsroom?.Categories?.OrderBy(category => category).ToList();

    public CallToActionBanner ArchiveCallToAction { get; set; } = new CallToActionBanner
    {
        Title = "Send us a media enquiry",
        Teaser = "You can contact us if you'd like to be added to our media distribution list for our news releases, you're a journalist with an enquiry or you have an enquiry about a press release or photo opportunity.",
        ButtonText = "Send an enquiry",
        Link = "https://www.stockport.gov.uk/start/send-a-media-enquiry",
        Colour = EColourScheme.Pink,
        Image = "//images.ctfassets.net/ii3xdrqc6nfw/7GxI1Llr4k2EuQ2WAS8CkQ/fe18c2ba9cf7faa7274e1b13481ac2fe/Working_on_a_laptop.jpg",
        AltText = "Send us a media enquiry"
    };

    // filters
    public string Tag { get; set; }
    public string Category { get; set; }
    public string DateRange { get; set; }

    // form elements
    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [Display(Name = "End date")]
    [DataType(DataType.Date)]
    [EndDateLaterThanStartDateValidation("DateFrom", "End date should be on or after the start date")]
    public DateTime? DateTo { get; set; }

    // urls
    public IFilteredUrl FilteredUrl { get; private set; }
    public QueryUrl CurrentUrl { get; private set; }
    public Pagination Pagination { get; set; }

    public NewsroomViewModel() { }

    public NewsroomViewModel(Newsroom newsroom, string emailAlertsUrl)
    {
        Newsroom = newsroom;
        EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(newsroom, emailAlertsUrl);
    }

    private static string SetEmailAlertsUrlWithTopicId(Newsroom newsroom, string url) =>
        !string.IsNullOrEmpty(newsroom.EmailAlertsTopicId)
            ? string.Concat(url, "?topic_id=", newsroom.EmailAlertsTopicId)
            : url;

    internal void AddNews(Newsroom newsRoom) =>
        Newsroom = newsRoom;

    internal void AddUrlSetting(AppSetting urlSetting, string topicId) =>
        EmailAlertsUrl = !string.IsNullOrEmpty(topicId)
            ? string.Concat(urlSetting.ToString(), "?topic_id=", topicId)
            : urlSetting.ToString();

    public string GetActiveDateFilter()
    {
        if (!DateFrom.HasValue || !DateTo.HasValue)
            return string.Empty;

        if (DateRange is "customdate")
            return DateFrom.Value.Equals(DateTo.Value)
                ? DateFrom.Value.ToString("dd/MM/yyyy")
                : $"{DateFrom.Value:dd/MM/yyyy} to {DateTo.Value:dd/MM/yyyy}";

        return DateFrom.Value.ToString("MMMM yyyy");
    }

    public string GetActiveYearFilter()
    {
        if (!DateFrom.HasValue || !DateTo.HasValue)
            return string.Empty;

        if (DateRange is "customdate")
            return DateFrom.Value.Equals(DateTo.Value)
                ? DateFrom.Value.ToString("dd/MM/yyyy")
                : $"{DateFrom.Value:dd/MM/yyyy} to {DateTo.Value:dd/MM/yyyy}";

        return DateFrom.Value.ToString("yyyy");
    }

    public void AddFilteredUrl(IFilteredUrl filteredUrl) =>
        FilteredUrl = filteredUrl;

    public void AddQueryUrl(QueryUrl queryUrl) =>
        CurrentUrl = queryUrl;

    public bool HasActiveFilter() =>
        !string.IsNullOrEmpty(Category) ||
            !string.IsNullOrEmpty(Tag) ||
            DateFrom.HasValue && DateTo.HasValue && (DateFrom <= DateTo);

    public string PageTitle =>
        ShowPagination
            ? $"- Page {Pagination.CurrentPageNumber} of {Pagination.TotalPages}"
            : string.Empty;

    private bool ShowPagination =>
        Pagination is not null && Pagination.TotalItems > Pagination.MaxItemsPerPage;

    public bool IsFirstPage => Pagination?.CurrentPageNumber <= 1;

    public bool HasLatestArticle => Newsroom?.LatestArticle?.Items?.Any() is true;

    public bool HasLatestNews => Newsroom?.LatestNews?.Items?.Any() is true;

    public bool HasNewsItems => Newsroom?.NewsItems?.Items?.Any() is true;

    public bool HasCallToAction => Newsroom?.CallToAction is not null;

    public bool ShowFeaturedNews => IsFirstPage && HasLatestArticle && !IsFromSearch();

    public bool ShowLatestNews => IsFirstPage && HasLatestNews;

    public bool IsFromSearch() =>
        !string.IsNullOrWhiteSpace(Category)
        || !string.IsNullOrWhiteSpace(Tag);
}