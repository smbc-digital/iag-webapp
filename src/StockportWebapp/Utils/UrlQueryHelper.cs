using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class UrlQueryHelper
    {
        public static RouteValueDictionary AddQueriesToUrl(RouteValueDictionary currentRouteData, IQueryCollection queries, Dictionary<string, string> querysToAdd)
        {
            var currentRouteValues = new RouteValueDictionary(currentRouteData);
            foreach (var key in queries.Keys)
            {
                if(!currentRouteValues.ContainsKey(key)) currentRouteValues.Add(key, queries[key]);
            }
            foreach (var query in querysToAdd)
            {
                currentRouteValues[query.Key] = query.Value;
            }
            return currentRouteValues;
        }

        public static bool QueryNameAndValueIsInQueryString(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName, string queryValue)
        {
            var inRouteData = currentRouteData.ContainsKey(queryName) && (string)currentRouteData[queryName] == queryValue;
            var inQueries = queries.ContainsKey(queryName) && queries[queryName] == queryValue;
            return inRouteData || inQueries;
        }

        public static RouteValueDictionary RemoveQueriesFromUrl(RouteValueDictionary currentRouteData, IQueryCollection queries, List<string> queryNames)
        {
            var currentRouteValues = new RouteValueDictionary(currentRouteData);
            foreach (var key in queries.Keys)
            {
                currentRouteValues.Add(key, queries[key]);
            }
            foreach (var queryName in queryNames)
            {
                currentRouteValues.Remove(queryName);
            }
            return currentRouteValues;
        }

        public static bool QueryNameIsInQueryString(RouteValueDictionary currentRouteData, IQueryCollection queries, string queryName)
        {
            return currentRouteData.ContainsKey(queryName) || queries.ContainsKey(queryName);
        }
    }
}