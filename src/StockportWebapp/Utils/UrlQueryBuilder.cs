using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class UrlQueryBuilder
    {
        public static RouteValueDictionary AddQueryToUrl(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName, string queryValue)
        {
            var currentRouteValues = new RouteValueDictionary(currentRouteData);
            foreach (var key in queries.Keys)
            {
                currentRouteValues.Add(key, queries[key]);
            }
            currentRouteValues[queryName] = queryValue;
            return currentRouteValues;
        }
    }
}
