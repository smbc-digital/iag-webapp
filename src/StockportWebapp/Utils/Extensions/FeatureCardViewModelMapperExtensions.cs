namespace StockportWebapp.Utils.Extensions
{
    public static class FeatureCardViewModelMapperExtensions
    {
        public static FeatureCardViewModel MapToFeatureCard(this Event featuredEvent) => 
            new FeatureCardViewModel
            {
                Title = featuredEvent.Title,
                Teaser = featuredEvent.Teaser,
                Date = featuredEvent.EventDate,
                StartTime = featuredEvent.StartTime,
                Image = featuredEvent.ImageUrl,
                ButtonText = "View more events",
                Slug = String.Concat("/events/", featuredEvent.Slug),
                ButtonUrl = "Events",
                HeaderText = "Upcoming event"
            };

        public static FeatureCardViewModel MapToFeatureCard(this News featuredNews) =>
            new FeatureCardViewModel
            {
                Title = featuredNews.Title, 
                Teaser = featuredNews.Teaser, 
                Date = featuredNews.UpdatedAt, 
                StartTime = string.Empty, 
                Image = featuredNews.Image, 
                ButtonText = "View more news",
                Slug = String.Concat("/news/", featuredNews.Slug),
                ButtonUrl = "Comms",
                HeaderText = "Latest news"
            };
    }
}

