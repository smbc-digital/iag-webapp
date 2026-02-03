namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Crumb
{
    public string Title { get; set; }
    public string NavigationLink { get; set; }

    public Crumb() { }

    public Crumb(string title, string slug, string type)
    {
        Title = title;
        NavigationLink = TypeRoutes.GetUrlFor(type, slug.ToLower());
    }
}