using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class UrlQueryHelper
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

        public static bool QueryNameAndValueIsInQueryString(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName, string queryValue)
        {
            var inRouteData = currentRouteData.ContainsKey(queryName) && (string)currentRouteData[queryName] == queryValue;
            var inQueries = queries.ContainsKey(queryName) && queries[queryName] == queryValue;
            return inRouteData || inQueries;
        }

        public static RouteValueDictionary RemoveQueryToUrl(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName)
        {
            var currentRouteValues = new RouteValueDictionary(currentRouteData);
            foreach (var key in queries.Keys)
            {
                currentRouteValues.Add(key, queries[key]);
            }
            currentRouteValues.Remove(queryName);
            return currentRouteValues;
        }

        public static bool QueryNameIsInQueryString(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName)
        {
            return currentRouteData.ContainsKey(queryName) || queries.ContainsKey(queryName);
        }
    }
}
