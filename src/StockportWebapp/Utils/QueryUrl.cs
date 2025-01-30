using System.Linq;

namespace StockportWebapp.Utils;

public class QueryUrl(RouteValueDictionary currentRouteData, IQueryCollection queries)
{
    private readonly RouteValueDictionary _currentRouteData = currentRouteData;
    private readonly IQueryCollection _queries = queries;

    public RouteValueDictionary AddQueriesToUrl(Dictionary<string, string> querysToAdd)
    {
        RouteValueDictionary currentRouteValues = new(_currentRouteData);

        _queries.Keys
            .Where(key => !currentRouteValues.ContainsKey(key))
            .ToList()
            .ForEach(key => currentRouteValues.Add(key, _queries[key]));

        querysToAdd
            .ToList()
            .ForEach(query => currentRouteValues[query.Key] = query.Value);

        return currentRouteValues;
    }

    public bool MatchesQueryParam(string queryName, string queryValue)
    {
        bool inRouteData = _currentRouteData.ContainsKey(queryName) && ((string)_currentRouteData[queryName]).Equals(queryValue);
        bool inQueries = _queries.ContainsKey(queryName) && _queries[queryName].Equals(queryValue);
        
        return inRouteData || inQueries;
    }

    public RouteValueDictionary WithoutQueryParam(List<string> queryNames)
    {
        RouteValueDictionary currentRouteValues = new(_currentRouteData);

        _queries.Keys.ToList().ForEach(key => currentRouteValues.Add(key, _queries[key]));
        queryNames.ForEach(queryName => currentRouteValues.Remove(queryName));

        return currentRouteValues;
    }

    public bool HasQueryParam(string queryName) =>
        _currentRouteData.ContainsKey(queryName) || _queries.ContainsKey(queryName);
}