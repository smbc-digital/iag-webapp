namespace StockportWebapp.Utils.Extensions;

[ExcludeFromCodeCoverage]
public static class IEnumerableExtension
{
    public static bool AnyOrDefault<T>(this IEnumerable<T> source, bool defaultValue = false) =>
        source is not null && source.Any();
}