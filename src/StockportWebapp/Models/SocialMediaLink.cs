namespace StockportWebapp.Models
{
    public class SocialMediaLink
    {
        public string Title { get; }
        public string Slug { get; }
        public string Url { get; }
        public string Icon { get; }
        public string Platform { get; }

        public SocialMediaLink(string title, string slug, string url, string icon, string platform)
        {
            Title = title;
            Slug = slug;
            Url = url;
            Icon = icon;
            Platform = platform;
        }
    }

    public class NullSocialMediaLink : SocialMediaLink
    {
        public NullSocialMediaLink()
            : base(
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        { }
    }
}
