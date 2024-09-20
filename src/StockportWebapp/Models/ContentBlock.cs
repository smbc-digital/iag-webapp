namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContentBlock
{
    public readonly string Slug;
    public readonly string Title;
    public readonly string Teaser;
    public readonly string Icon;
    public readonly string Type;
    public readonly string ContentType;
    public readonly string NavigationLink;
    public readonly string Image;
    public string MailingListId;
    public string Body;
    public EColourScheme ColourScheme;
    public string Link;
    public string ButtonText;
    public readonly List<ContentBlock> SubItems;
    public string Statistic;
    public string StatisticSubheading;
    public string VideoTitle;
    public string VideoToken;
    public string VideoPlaceholderPhotoId;
    public string AssociatedTagCategory;
    public News NewsArticle;
    public Profile Profile;
    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");
    public string SolidBackgroundColourClass => $"bg-solid{CssClassMapper.GetBoldCssClass(ColourScheme)}"; 
    public string BackgroundColourClass => $"bg{CssClassMapper.GetCssClass(ColourScheme)}";
    public string TextColourClass => $"text{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderClass => $"border{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderColourClass => $"border-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string PseudoBorderColourClass => $"border-pseudo-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public ContentBlock(string slug, string title, string teaser, string icon, string type, string contentType, string image, string mailingListId, string body, List<ContentBlock> subItems, string link, string buttonText, EColourScheme colourScheme,
                        string statistic, string statisticSubheading, string videoTitle, string videoToken, string videoPlaceholderPhotoId, string associatedTagCategory, News newsArticle)
    {
        Slug = slug;
        Title = title;
        Icon = icon;
        Teaser = teaser;
        Type = type;
        ContentType = contentType;
        NavigationLink = TypeRoutes.GetUrlFor(type, slug);
        Image = image;
        MailingListId = mailingListId;
        Body = MarkdownWrapper.ToHtml(body);
        SubItems = subItems;
        ColourScheme = colourScheme;
        Link = link;
        ButtonText = buttonText;
        Statistic = statistic;
        StatisticSubheading = statisticSubheading;
        VideoTitle = videoTitle;
        VideoToken = videoToken;
        VideoPlaceholderPhotoId = videoPlaceholderPhotoId;
        AssociatedTagCategory = associatedTagCategory;
        NewsArticle = newsArticle;
    }
}