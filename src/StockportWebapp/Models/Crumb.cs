namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Crumb
{
    public string Title;
    public string NavigationLink;

    public Crumb()
    { }

    public Crumb(string title, string slug, string type) : this()
    {
        Title = title;
        NavigationLink = TypeRoutes.GetUrlFor(type, slug.ToLower());
    }
}