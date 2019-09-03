namespace StockportWebapp.Models
{
    public class SpotlightBanner
    {
        public string Title { get; }
        public string Teaser { get; }
        public string Link { get; }

        public SpotlightBanner(string title, string teaser, string link)
        {
            Title = title;
            Teaser = teaser;
            Link = link;
        }
    }
}
