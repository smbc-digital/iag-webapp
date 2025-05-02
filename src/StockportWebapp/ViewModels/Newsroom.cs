namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class Newsroom(List<News> news,
                    List<Alert> alerts,
                    bool emailAlerts,
                    string emailAlertsTopicId,
                    List<string> categories,
                    List<DateTime> dates)
{
    public List<News> News { get; set; } = news;
    public List<Alert> Alerts { get; } = alerts;
    public bool EmailAlerts { get; } = emailAlerts;
    public string EmailAlertsTopicId { get; } = emailAlertsTopicId;
    public List<string> Categories { get; } = categories;
    public List<DateTime> Dates { get; } = dates;

    public NavCardList LatestNews => new()
    {
        Items = News.Select(news => new NavCard(news.Title,
                                                news.Slug,
                                                news.Teaser,
                                                news.ThumbnailImage,
                                                news.Image,
                                                string.Empty,
                                                EColourScheme.Teal,
                                                news.UpdatedAt,
                                                string.Empty)).Take(3).ToList()
    };
}