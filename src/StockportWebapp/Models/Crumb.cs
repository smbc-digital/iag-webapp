namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Crumb(string title, string slug, string type)
{
    public string Title = title;
    public string NavigationLink = TypeRoutes.GetUrlFor(type, slug.ToLower());
}