namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PageLocation(string slug, IEnumerable<string> parentSlugs)
{
    public string Slug { get; private set; } = slug;
    public IEnumerable<string> ParentSlugs { get; private set; } = parentSlugs;
}