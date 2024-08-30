namespace StockportWebapp.Models;

public class SubItem
{
    public readonly string Slug;
    public readonly string Title;
    public readonly string Icon;
    public readonly string Teaser;
    public readonly string Type;
    public readonly string ContentType;
    public readonly string NavigationLink;
    public readonly string Image;
    public string MailingListId;
    public string Body;
    public EColourScheme ColourScheme;
    public string Link;
    public string ButtonText;
    public readonly List<SubItem> SubItems;
    public string Statistic;
    public string StatisticSubheading;


    public SubItem(string slug, string title, string teaser, string icon, string type, string contentType, string image, string mailingListId, string body, List<SubItem> subItems, string link, string buttonText, EColourScheme colourScheme, string statistic, string statisticSubheading)
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
    }

    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");

    public string SolidBackgroundColourClass => $"bg-solid{CssClassMapper.GetCssClass(ColourScheme)}"; 
    public string BackgroundColourClass => $"bg{CssClassMapper.GetCssClass(ColourScheme)}";
    public string TextColourClass => $"text{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderClass => $"border{CssClassMapper.GetBoldCssClass(ColourScheme)}";
    public string BorderColourClass => $"border-colour{CssClassMapper.GetBoldCssClass(ColourScheme)}";
}