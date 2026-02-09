namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContentBlock
{
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Teaser { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public string NavigationLink => TypeRoutes.GetUrlFor(Type, Slug);
    public string Image { get; init; } = string.Empty;
    public string MailingListId { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public EColourScheme ColourScheme { get; init; }
    public string Link { get; init; } = string.Empty;
    public string ButtonText { get; init; } = string.Empty;
    public List<ContentBlock> SubItems { get; init; } = new();
    public string Statistic { get; init; } = string.Empty;
    public string StatisticSubheading { get; init; } = string.Empty;
    public string VideoTitle { get; init; } = string.Empty;
    public string VideoToken { get; init; } = string.Empty;
    public string VideoPlaceholderPhotoId { get; init; } = string.Empty;
    public string AssociatedTagCategory { get; init; } = string.Empty;

    public bool UseTag { get; init; }
    public bool IsLatest { get; init; }

    public News? NewsArticle { get; init; }
    public Profile? Profile { get; init; }
    public List<Event> Events { get; init; } = new();
    public List<News> News { get; init; } = new();

    public string ScreenReader { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;

    public string GetNavigationLink(string additionalUrlContent) =>
        TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");

    public string SolidBackgroundColourClass =>
        $"bg-solid{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public string SolidBackgroundColourHoverClass =>
        $"bg-solid{CssClassMapper.GetBoldCssClass(ColourScheme)}-hover";

    public string BackgroundColourClass =>
        $"bg{CssClassMapper.GetCssClass(ColourScheme)}";

    public string TextColourClass =>
        $"text{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public string BorderClass =>
        $"border{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public string BorderColourClass =>
        $"border-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public string PseudoBorderColourClass =>
        $"border-pseudo-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";

    public bool IsDefaultColourScheme =>
        ColourScheme is EColourScheme.None
        or EColourScheme.Multi
        or EColourScheme.Light_Overlay
        or EColourScheme.Dark_Overlay;

    public string OutlineButtonColour =>
        IsDefaultColourScheme
            ? "btn--teal btn--teal-outline"
            : $"btn-{CssClassMapper.GetBoldCssClass(ColourScheme)} btn-{CssClassMapper.GetBoldCssClass(ColourScheme)}-outline";

    public string ImageBannerOverlayTheme =>
        ColourScheme is EColourScheme.Dark_Overlay or EColourScheme.Light_Overlay
            ? CssClassMapper.GetCssClass(ColourScheme)
            : string.Empty;

    public string Href =>
        SubItems.Any()
            ? $"href={SubItems.First().NavigationLink}"
            : !string.IsNullOrEmpty(Link)
                ? $"href={Link} target=_blank"
                : string.Empty;
}