namespace StockportWebapp.Models;

public class News
{
    public string Title { get; }
    public string Slug { get; }
    public string Teaser { get; }
    public string Purpose { get; set; }
    public string Image { get; }
    public string ThumbnailImage { get; }
    public string Body { get; }
    public List<Crumb> Breadcrumbs { get; }
    public DateTime SunriseDate { get; }
    public DateTime SunsetDate { get; }
    public DateTime UpdatedAt { get; }
    public List<Alert> Alerts { get; }
    public List<string> Tags { get; }
    public IEnumerable<Document> Documents { get; set; }

    public News(string title, string slug, string teaser, string purpose, string image, string thumbnailImage, string body, List<Crumb> breadcrumbs, DateTime sunriseDate, DateTime sunsetDate, DateTime updatedAt, List<Alert> alerts, List<string> tags, IEnumerable<Document> documents)
    {
        Title = title;
        Slug = slug;
        Teaser = teaser;
        Purpose = purpose;
        Image = image;
        ThumbnailImage = thumbnailImage;
        Body = body;
        Breadcrumbs = breadcrumbs;
        SunriseDate = sunriseDate;
        SunsetDate = sunsetDate;
        UpdatedAt = updatedAt;
        Alerts = alerts;
        Tags = tags;
        Documents = documents;
    }
}
