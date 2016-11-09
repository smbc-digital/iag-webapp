namespace StockportWebapp.Models
{
    public class CarouselContent
    {
        public string Title { get; }
        public string Teaser { get; }
        public string Image { get; }
        public string URL { get; }

        public CarouselContent(string title, string teaser, string image, string url)
        {
            Title = title;
            Teaser = teaser;
            Image = image;
            URL = url;
        }
    }

    public class NullCarouselContent : CarouselContent
    {
        public NullCarouselContent() : base(string.Empty, string.Empty, string.Empty, string.Empty) { }
    }
}

