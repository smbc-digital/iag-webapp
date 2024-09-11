namespace StockportWebapp.Models;

public class SubItem
{
    public readonly string Slug;
    public readonly string Title;
    public readonly string Icon;
    public readonly string Teaser;
    public readonly string Type;
    public readonly string NavigationLink;
    public readonly string Image;
    public EColourScheme ColourScheme;
    public readonly List<SubItem> SubItems;

    public SubItem(string slug, string title, string teaser, string icon, string type, string image, List<SubItem> subItems, EColourScheme colourScheme)
    {
        Slug = slug;
        Title = title;
        Icon = icon;
        Teaser = teaser;
        Type = type;
        NavigationLink = TypeRoutes.GetUrlFor(type, slug);
        Image = image;
        SubItems = subItems;
        ColourScheme = colourScheme;
    }

    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");
}