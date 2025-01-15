namespace StockportWebapp.Utils.Extensions;

public static class UrlGeneratorExtensions
{
    public static string AddSlug(this string url, string slug) =>
        $"{url}{(url.EndsWith("/", StringComparison.Ordinal)
            ? string.Empty
            : "/")}{slug}";

    public static string AddExtraToUrl(this string url, string extra) =>
        $"{url}{(url.EndsWith("/", StringComparison.Ordinal)
            ? string.Empty
            : "/")}{extra}";

    public static string AddQueryStrings(this string url, List<Query> queries) =>
        queries is null || queries.Count < 1
            ? url
            : $"{url}{"?"}{string.Join("&", queries)}";

    public static string AddQueryStrings(this string url, Query query) =>
        string.IsNullOrEmpty(query.Name) || string.IsNullOrEmpty(query.Value)
            ? url
            : $"{url}{"?"}{string.Join("&", query)}";
}