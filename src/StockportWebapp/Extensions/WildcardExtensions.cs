namespace StockportWebapp.Extensions;
public static class WildcardExtensions
{
    public static PageLocation ProcessSlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return new PageLocation(string.Empty, new List<string>()) ;

        slug = slug.TrimEnd('/');
        var slugValues = slug?.Split('/') ?? Array.Empty<string>();
        slugValues = slugValues.Select(slug => slug.Trim(new char[] { '/', '\\' })).ToArray();
        return new PageLocation(slugValues.Last(), slugValues.Where(slug => !string.IsNullOrEmpty(slug)).SkipLast(1).ToList());
    }
}