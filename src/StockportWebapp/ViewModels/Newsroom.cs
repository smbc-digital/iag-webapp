namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class Newsroom(List<News> news,
                    News FeaturedNews,
                    NavCardList latestArticle,
                    NavCardList latestNews,
                    NavCardList newsItems,
                    List<Alert> alerts,
                    bool emailAlerts,
                    string emailAlertsTopicId,
                    List<string> categories,
                    List<DateTime> dates,
                    List<int> years,
                    CallToActionBanner callToAction,
                    int currentPageNumber = 1)
{
    public List<News> News { get; set; } = news;
    public News FeaturedNews { get; set; } = FeaturedNews;
    public NavCardList LatestArticle { get; set; } = latestArticle;
    public NavCardList LatestNews { get; set; } = latestNews;
    public NavCardList NewsItems { get; set; } = newsItems;
    public List<Alert> Alerts = alerts.Where(alert => !alert.Severity.Equals(Severity.Condolence)).ToList();
    public List<Alert> CondolenceAlerts = alerts.Where(alert => alert.Severity.Equals(Severity.Condolence)).ToList();
    public bool EmailAlerts { get; } = emailAlerts;
    public string EmailAlertsTopicId { get; } = emailAlertsTopicId;
    public List<string> Categories { get; } = categories;
    public List<DateTime> Dates { get; } = dates;
    public List<int> Years { get; } = years;
    public int CurrentPageNumber { get; set; } = currentPageNumber;
    public CallToActionBanner CallToAction { get; set; } = callToAction;

    public NavCardList ArchivedItems => new()
    {
        Items = News
            .Select(news =>
            {
                DateTime publishingDate = GetPublishingDateOrFallback(news);
                return new NavCard(
                    news.Title,
                    $"news-article/{news.Slug}",
                    news.Teaser,
                    news.ThumbnailImage,
                    news.Image,
                    string.Empty,
                    EColourScheme.Teal,
                    publishingDate,
                    string.Empty);
            })
            .ToList()
    };

    private static DateTime GetPublishingDateOrFallback(News news)
    {
        DateTime.TryParse(news.PublishingDate, out DateTime parsedDate);

        if (string.IsNullOrEmpty(news.PublishingDate) || news.PublishingDate.Equals(DateTime.MinValue.ToString("yyyy-MM-dd")))
            return news.SunriseDate;

        return parsedDate;
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
}