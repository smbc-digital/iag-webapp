namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class SocialMediaLink
{
    public string Title { get; }
    public string Slug { get; }
    public string Link { get; }
    public string Icon { get; }
    public string AccountName { get; }
    public string ScreenReader { get; }

    public SocialMediaLink(string title, string slug, string link, string icon, string accountName, string screenReader)
    {
        Title = title;
        Slug = slug;
        Link = link;
        Icon = icon;
        AccountName = accountName;
        ScreenReader = screenReader;
    }
}