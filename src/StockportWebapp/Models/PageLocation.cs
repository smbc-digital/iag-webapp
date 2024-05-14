using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
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