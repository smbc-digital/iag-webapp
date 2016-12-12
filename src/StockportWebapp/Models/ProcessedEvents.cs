namespace StockportWebapp.Models
{
    public class ProcessedEvents : IProcessedContentType
    {
        public string Title { get; }
        public string Slug { get; }
        public string Teaser { get; }
        public string Image { get; }
        public string ThumbnailImage { get; }
        public string Description { get; set; }

        public ProcessedEvents(string title, string slug, string teaser, string image, string thumbnailImage, string description)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Image = image;
            Description = description;
            ThumbnailImage = thumbnailImage;
        }
    }
}
