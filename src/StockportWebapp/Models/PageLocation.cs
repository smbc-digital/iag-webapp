namespace StockportWebapp.Models;

public class PageLocation
{
    public string Slug { get; private set; }
    public IEnumerable<string> ParentSlugs { get; private set; }

    public PageLocation(string slug, IEnumerable<string> parentSlugs)
    {
        Slug = slug;
        ParentSlugs = parentSlugs;
    }
}