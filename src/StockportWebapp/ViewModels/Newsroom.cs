namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class Newsroom(List<News> news,
                    List<Alert> alerts,
                    bool emailAlerts,
                    string emailAlertsTopicId,
                    List<string> categories,
                    List<DateTime> dates,
                    int currentPageNumber = 1)
{
    public List<News> News { get; set; } = news;
    public List<Alert> Alerts { get; } = alerts;
    public bool EmailAlerts { get; } = emailAlerts;
    public string EmailAlertsTopicId { get; } = emailAlertsTopicId;
    public List<string> Categories { get; } = categories;
    public List<DateTime> Dates { get; } = dates;
    public int CurrentPageNumber { get; } = currentPageNumber;

    public NavCardList LatestNews => new()
    {
        Items = News.Select(news => new NavCard(news.Title,
                                                $"news-article/{news.Slug}",
                                                news.Teaser,
                                                news.ThumbnailImage,
                                                news.Image,
                                                string.Empty,
                                                EColourScheme.Teal,
                                                news.UpdatedAt,
                                                string.Empty)).Take(3).ToList()
    };

    public NavCardList NewsItems => new()
    {
        Items = News
            .Skip(CurrentPageNumber == 1 ? 3 : 0)
            .Select(news => new NavCard(
                news.Title,
                $"news-article/{news.Slug}",
                news.Teaser,
                news.ThumbnailImage,
                news.Image,
                string.Empty,
                EColourScheme.Teal,
                news.UpdatedAt,
                string.Empty))
            .ToList()
    };

    public NavCardList Article2NewsItems => new()
    {
        Items = News.Select(news => new NavCard(news.Title,
                                                $"news-article/{news.Slug}",
                                                news.Teaser,
                                                news.ThumbnailImage,
                                                news.Image,
                                                string.Empty,
                                                EColourScheme.Teal,
                                                news.UpdatedAt,
                                                string.Empty)).ToList()
    };

    public NavCardList ArchivedItems => new()
    {
        Items = News.Select(news => new NavCard(news.Title,
                                                $"news-article/{news.Slug}",
                                                news.Teaser,
                                                news.ThumbnailImage,
                                                news.Image,
                                                string.Empty,
                                                EColourScheme.Teal,
                                                news.UpdatedAt,
                                                string.Empty)).ToList()
    };

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