using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class CurrentUrl
    {
        private RouteValueDictionary currentRouteData;
        IQueryCollection queries;

        public CurrentUrl(RouteValueDictionary currentRouteData, IQueryCollection queries)
        {
            this.currentRouteData = currentRouteData;
            this.queries = queries;
        }

        public RouteValueDictionary AddQueriesToUrl(Dictionary<string, string> querysToAdd)
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

        public bool MatchesQueryParam(string queryName, string queryValue)
        {
            var inRouteData = currentRouteData.ContainsKey(queryName) && (string)currentRouteData[queryName] == queryValue;
            var inQueries = queries.ContainsKey(queryName) && queries[queryName] == queryValue;
            return inRouteData || inQueries;
        }

        public RouteValueDictionary WithoutQueryParam(List<string> queryNames)
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

        public bool HasQueryParam(string queryName)
        {
            return currentRouteData.ContainsKey(queryName) || queries.ContainsKey(queryName);
        }
    }
}