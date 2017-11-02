using StockportWebapp.Models;
using System;
using System.Collections.Generic;

namespace StockportWebapp.Extensions
{
    public static class UrlGeneratorExtensions
    {
        public static string AddSlug(this string url, string slug)
        {
            return $"{url}{(url.EndsWith("/", StringComparison.Ordinal) ? "" : "/")}{slug}";
        }

        public static string AddExtraToUrl(this string url, string extra)
        {
            return $"{url}{(url.EndsWith("/", StringComparison.Ordinal) ? "" : "/")}{extra}";
        }

        public static string AddQueryStrings(this string url, List<Query> queries)
        {
            if (queries == null || queries.Count < 1) return url;
            return $"{url}{"?"}{string.Join("&", queries)}";
        }
    }
}
