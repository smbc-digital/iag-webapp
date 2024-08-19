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
    public int MailingListId;
    public string Body;
    public EColourScheme ColourScheme;
    public string Link;
    public readonly List<SubItem> SubItems;

    public SubItem(string slug, string title, string teaser, string icon, string type, string contentType, string image, int mailingListId, string body, List<SubItem> subItems, string link, EColourScheme colourScheme)
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
    }

    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");
}