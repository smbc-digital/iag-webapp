namespace StockportWebapp.Utils
{
    public class QueryUrl
    {
        private readonly RouteValueDictionary _currentRouteData;
        private readonly IQueryCollection _queries;

        public QueryUrl(RouteValueDictionary currentRouteData, IQueryCollection queries)
        {
            _currentRouteData = currentRouteData;
            _queries = queries;
        }

        public RouteValueDictionary AddQueriesToUrl(Dictionary<string, string> querysToAdd)
        {
            var currentRouteValues = new RouteValueDictionary(_currentRouteData);
            foreach (var key in _queries.Keys)
            {
                if (!currentRouteValues.ContainsKey(key)) currentRouteValues.Add(key, _queries[key]);
            }
            foreach (var query in querysToAdd)
            {
                currentRouteValues[query.Key] = query.Value;
            }
            return currentRouteValues;
        }

        public bool MatchesQueryParam(string queryName, string queryValue)
        {
            var inRouteData = _currentRouteData.ContainsKey(queryName) && (string)_currentRouteData[queryName] == queryValue;
            var inQueries = _queries.ContainsKey(queryName) && _queries[queryName] == queryValue;
            return inRouteData || inQueries;
        }

        public RouteValueDictionary WithoutQueryParam(List<string> queryNames)
        {
            var currentRouteValues = new RouteValueDictionary(_currentRouteData);
            foreach (var key in _queries.Keys)
            {
                currentRouteValues.Add(key, _queries[key]);
            }
            foreach (var queryName in queryNames)
            {
                currentRouteValues.Remove(queryName);
            }
            return currentRouteValues;
        }

        public bool HasQueryParam(string queryName)
        {
            return _currentRouteData.ContainsKey(queryName) || _queries.ContainsKey(queryName);
        }
    }
}