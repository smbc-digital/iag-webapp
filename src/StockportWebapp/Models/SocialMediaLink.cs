namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class SocialMediaLink(string title,
                            string slug,
                            string link,
                            string icon,
                            string accountName,
                            string screenReader)
{
    public string Title { get; } = title;
    public string Slug { get; } = slug;
    public string Link { get; } = link;
    public string Icon { get; } = icon;
    public string AccountName { get; } = accountName;
    public string ScreenReader { get; } = screenReader;
}