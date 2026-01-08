namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContentBlock
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Teaser { get; set; }
    public string Icon { get; set; }
    public string Type { get; set; }
    public string ContentType { get; set; }
    public string NavigationLink { get; set; }
    public string Image { get; set; }
    public string MailingListId { get; set; }
    public string Body { get; set; }
    public EColourScheme ColourScheme { get; set; }
    public string Link { get; set; }
    public string ButtonText { get; set; }
    public List<ContentBlock> SubItems { get; set; } = new();
    public string Statistic { get; set; }
    public string StatisticSubheading { get; set; }
    public string VideoTitle { get; set; }
    public string VideoToken { get; set; }
    public string VideoPlaceholderPhotoId { get; set; }
    public string AssociatedTagCategory { get; set; }
    public News NewsArticle { get; set; }
    public Profile Profile { get; set; }
    public List<Event> Events { get; set; } = new();
    public List<News> News { get; set; } = new();
    public string ScreenReader { get; set; }
    public string AccountName { get; set; }

    // Optional: computed/view properties can live here or in a separate ViewModel
}
