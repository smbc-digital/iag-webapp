namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class SubItem(string slug,
                    string title,
                    string teaser,
                    string teaserImage,
                    string icon,
                    string type,
                    string image,
                    List<SubItem> subItems,
                    EColourScheme colourScheme)
{
    public readonly string Slug = slug;
    public readonly string Title = title;
    public readonly string Icon = icon;
    public readonly string Teaser = teaser;
    public readonly string TeaserImage = teaserImage;
    public readonly string Type = type;
    public readonly string NavigationLink = TypeRoutes.GetUrlFor(type, slug);
    public readonly string Image = image;
    public EColourScheme ColourScheme = colourScheme;
    public readonly List<SubItem> SubItems = subItems;

    public string GetNavigationLink(string additionalUrlContent) => TypeRoutes.GetUrlFor(Type, $"{additionalUrlContent}/{Slug}");
}