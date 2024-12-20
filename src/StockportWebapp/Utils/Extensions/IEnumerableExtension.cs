﻿namespace StockportWebapp.Utils.Extensions;

[ExcludeFromCodeCoverage]
public static class IEnumerableExtension
{
    public static bool AnyOrDefault<T>(this IEnumerable<T> source, bool defaultValue = false)
    {
        if (source == null)
            return defaultValue;

        return source.Any();
    }
}
