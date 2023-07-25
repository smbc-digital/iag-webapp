namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedNews : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string Purpose;
        public readonly string Image;
        public readonly string ThumbnailImage;
        public readonly string Body;
        public readonly List<Crumb> Breadcrumbs;
        public readonly DateTime SunriseDate;
        public readonly DateTime SunsetDate;
        public readonly DateTime UpdatedAt;
        public readonly List<Alert> Alerts;
        public readonly List<string> Tags;

        public ProcessedNews(string title, string slug, string teaser, string purpose, string image, string thumbnailImage, string body, List<Crumb> breadcrumbs, DateTime sunriseDate, DateTime sunsetDate, DateTime updatedAt, List<Alert> alerts, List<string> tags)
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
            UpdatedAt = updatedAt;
            SunsetDate = sunsetDate;
            Alerts = alerts;
            Tags = tags;
        }
    }
}
