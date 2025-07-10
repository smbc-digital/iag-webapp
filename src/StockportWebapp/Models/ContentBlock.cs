namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContentBlock(string slug,
                        string title,
                        string teaser,
                        string icon,
                        string type,
                        string contentType,
                        string image,
                        string mailingListId,
                        string body,
                        List<ContentBlock> subItems,
                        string link,
                        string buttonText,
                        EColourScheme colourScheme,
                        string statistic,
                        string statisticSubheading,
                        string videoTitle,
                        string videoToken,
                        string videoPlaceholderPhotoId,
                        string associatedTagCategory,
                        News newsArticle,
                        List<Event> events,
                        List<News> news,
                        string screenReader,
                        string accountName)
{
    public readonly string Slug = slug;
    public readonly string Title = title;
    public readonly string Teaser = teaser;
    public readonly string Icon = icon;
    public readonly string Type = type;
    public readonly string ContentType = contentType;
    public readonly string NavigationLink = TypeRoutes.GetUrlFor(type, slug);
    public readonly string Image = image;
    public string MailingListId = mailingListId;
    public string Body = MarkdownWrapper.ToHtml(body);
    public EColourScheme ColourScheme = colourScheme;
    public string Link = link;
    public string ButtonText = buttonText;
    public readonly List<ContentBlock> SubItems = subItems;
    public string Statistic = statistic;
    public string StatisticSubheading = statisticSubheading;
    public string VideoTitle = videoTitle;
    public string VideoToken = videoToken;
    public string VideoPlaceholderPhotoId = videoPlaceholderPhotoId;
    public string AssociatedTagCategory = associatedTagCategory;
    public bool UseTag;
    public bool IsLatest;
    public News NewsArticle = newsArticle;
    public Profile Profile;
    public List<Event> Events = events;
    public List<News> News = news;
    public string ScreenReader = screenReader;
    public string AccountName = accountName;
    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");
    public string SolidBackgroundColourClass => $"bg-solid{CssClassMapper.GetBoldCssClass(ColourScheme)}"; 
    public string SolidBackgroundColourHoverClass => $"bg-solid{CssClassMapper.GetBoldCssClass(ColourScheme)}-hover"; 
    public string BackgroundColourClass => $"bg{CssClassMapper.GetCssClass(ColourScheme)}";
    public string TextColourClass => $"text{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderClass => $"border{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderColourClass => $"border-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string PseudoBorderColourClass => $"border-pseudo-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public bool IsDefaultColourScheme =>
        ColourScheme is EColourScheme.None
        || ColourScheme is EColourScheme.Multi
        || ColourScheme is EColourScheme.Light_Overlay
        || ColourScheme is EColourScheme.Dark_Overlay;
    
    public string OutlineButtonColour => IsDefaultColourScheme 
        ? "btn--teal btn--teal-outline"
        : $"btn-{CssClassMapper.GetBoldCssClass(ColourScheme)} btn-{CssClassMapper.GetBoldCssClass(ColourScheme)}-outline";

    public string ImageBannerOverlayTheme => ColourScheme is EColourScheme.Dark_Overlay || ColourScheme is EColourScheme.Light_Overlay
        ? CssClassMapper.GetCssClass(ColourScheme)
        : string.Empty;

    public string Href => SubItems.Any()
        ? $"href={SubItems.First().NavigationLink}"
        : !string.IsNullOrEmpty(Link)
            ? $"href={Link} target=_blank"
            : string.Empty;
}