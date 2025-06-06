﻿namespace StockportWebapp.ViewModels;

public class NewsroomViewModel
{
    // data
    public string EmailAlertsUrl { get; private set; }
    public Newsroom Newsroom { get; private set; }
    public List<string> Categories =>
        Newsroom?.Categories?.OrderBy(c => c).ToList();

    // filters
    public string Tag { get; set; }
    public string Category { get; set; }
    public string DateRange { get; set; }

    // form elements
    [Required]
    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [Required]
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
}