namespace StockportWebapp.Extensions;
public static class WildcardSlugExtensions
{
    public static PageLocation ProcessAsWildcardSlug(this string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return new PageLocation(string.Empty, new List<string>()) ;

        slug = slug.TrimEnd('/');
        string[] slugValues = slug?.Split('/') ?? [];
        slugValues = slugValues.Select(slug => slug.Trim(['/', '\\'])).ToArray();

        return new PageLocation(slugValues.Last(), slugValues.Where(slug => !string.IsNullOrEmpty(slug)).SkipLast(1).ToList());
    }
}