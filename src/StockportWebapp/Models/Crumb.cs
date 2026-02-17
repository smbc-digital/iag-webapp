namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Crumb
{
    private string _navigationLink;

    public string Title { get; set; }
    public string Slug { get; set; }
    public string Type { get; set; }
    public string NavigationLink
    {
        get => _navigationLink ?? TypeRoutes.GetUrlFor(Type, Slug?.ToLower());
        set => _navigationLink = value;
    }

    public Crumb() { }

    public Crumb(string title, string slug, string type)
    {
        Title = title;
        NavigationLink = TypeRoutes.GetUrlFor(type, slug.ToLower());
    }
}