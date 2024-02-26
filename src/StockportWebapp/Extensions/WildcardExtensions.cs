namespace StockportWebapp.Extensions;
public static class WildcardExtensions
{
    public static PageLocation ProcessSlug(string slug)
    {
        slug = slug.TrimEnd('/');
        var slugValues = slug?.Split('/') ?? Array.Empty<string>();
        return new PageLocation(slugValues.Last(), slugValues.SkipLast(1).ToList());
    }
}