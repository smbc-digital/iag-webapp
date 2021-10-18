namespace StockportWebapp.Models
{
    public class SocialMediaLink
    {
        public string Title { get; }
        public string Slug { get; }
        public string Url { get; }
        public string Icon { get; }
        public string AccountName { get; }
        public string ScreenReader { get; }

        public SocialMediaLink(string title, string slug, string url, string icon, string accountName, string screenReader)
        {
            Title = title;
            Slug = slug;
            Url = url;
            Icon = icon;
            AccountName = accountName;
            ScreenReader = screenReader;
        }
    }

    public class NullSocialMediaLink : SocialMediaLink
    {
        public NullSocialMediaLink()
            : base(
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        { }
    }
}
