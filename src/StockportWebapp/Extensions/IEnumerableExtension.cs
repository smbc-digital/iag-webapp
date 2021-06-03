using System.Collections.Generic;
using System.Linq;

namespace StockportWebapp.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool AnyOrDefault<T>(this IEnumerable<T> source, bool defaultValue = false)
        {
            if (source == null)
                return defaultValue;

            return source.Any();
        }
    }
}
