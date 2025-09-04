namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]

public class AtoZ(string title, string slug, string teaser, string type)
{
    public string Title { get; } = title;
    public string Teaser { get; } = teaser;
    public string NavigationLink { get; } = TypeRoutes.GetUrlFor(type, slug);
}