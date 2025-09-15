namespace StockportWebapp.Utils.Extensions;

public static class FeatureCardViewModelMapperExtensions
{
    public static FeatureCardViewModel MapToFeatureCard(this Event featuredEvent) =>
        new()
        {
            Title = featuredEvent.Title,
            Teaser = featuredEvent.Teaser,
            Date = featuredEvent.EventDate,
            StartTime = featuredEvent.StartTime,
            Image = featuredEvent.ImageUrl,
            ButtonText = "View more events",
            Slug = string.Concat("/events/", featuredEvent.Slug),
            ButtonTargetController = "Events",
            HeaderText = "Upcoming event"
        };

    public static FeatureCardViewModel MapToFeatureCard(this News featuredNews) =>
        new()
        {
            Title = featuredNews.Title,
            Teaser = featuredNews.Teaser,
            Date = featuredNews.UpdatedAt,
            StartTime = string.Empty,
            Image = featuredNews.Image,
            ButtonText = "View more news",
            Slug = string.Concat("/news/", featuredNews.Slug),
            ButtonTargetController = "LandingPage",
            ButtonSlug = "news-media",
            HeaderText = "Latest news"
        };
}