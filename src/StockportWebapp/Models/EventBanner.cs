namespace StockportWebapp.Models
{
    public class EventBanner
    {
        public string Title { get; }
        public string Teaser { get; }
        public string Icon { get; }
        public string Link { get; }

        public EventBanner(string title, string teaser, string icon, string link)
        {
            Title = title;
            Teaser = teaser;
            Icon = icon;
            Link = link;
        }

        public class NullEventBanner : EventBanner
        {
            public NullEventBanner() : base(string.Empty, string.Empty, string.Empty, string.Empty) { }
        }
    }
}
