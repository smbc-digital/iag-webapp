namespace StockportWebapp.Utils;

public interface IFilteredUrl
{
    void SetQueryUrl(QueryUrl queryUrl);
    RouteValueDictionary WithoutCategory();
    RouteValueDictionary AddCategoryFilter(string category);
    bool HasNoCategoryFilter();
    RouteValueDictionary AddMonthFilter(DateTime startDate);
    RouteValueDictionary WithoutDateFilter();
    bool HasNoDateFilter();
    RouteValueDictionary WithoutTagFilter();
    RouteValueDictionary AddDateFilter(string DateFrom, string DateTo, string DateRange);
}

public class FilteredUrl(ITimeProvider timeProvider) : IFilteredUrl
{
    private readonly ITimeProvider _timeProvider = timeProvider;
    private QueryUrl _queryUrl;

    public void SetQueryUrl(QueryUrl queryUrl) =>
        _queryUrl = queryUrl;

    public RouteValueDictionary WithoutCategory() =>
        _queryUrl is null
            ? new RouteValueDictionary()
            : _queryUrl.WithoutQueryParam(new List<string> { "Category", "Page" });

    public RouteValueDictionary AddCategoryFilter(string category) =>
        _queryUrl is null
            ? new RouteValueDictionary()
            : _queryUrl.AddQueriesToUrl(new Dictionary<string, string> { { "Category", category }, { "Page", "1" } });

    public bool HasNoCategoryFilter() => 
        !_queryUrl?.HasQueryParam("Category") ?? false;

    public RouteValueDictionary AddMonthFilter(DateTime startDate)
    {
        DateTime dateto = startDate.Month.Equals(_timeProvider.Now().Month) && startDate.Year.Equals(_timeProvider.Now().Year)
            ? _timeProvider.Now()
            : startDate.AddMonths(1).AddDays(-1);

        return _queryUrl is null
            ? new RouteValueDictionary()
            : _queryUrl.AddQueriesToUrl(new Dictionary<string, string>
            {
                {"DateFrom", startDate.ToString("yyyy-MM-dd")},
                {"DateTo", dateto.ToString("yyyy-MM-dd")},
                {"daterange", "month"},
                {"Page", "1"}
            });
    }

    public RouteValueDictionary WithoutDateFilter() =>
        _queryUrl is null
            ? new RouteValueDictionary()
            : _queryUrl.WithoutQueryParam(new List<string> { "DateFrom", "DateTo", "daterange", "Page" });

    public RouteValueDictionary AddDateFilter(string DateFrom, string DateTo, string DateRange) =>
        _queryUrl.AddQueriesToUrl(new Dictionary<string, string>
        {
            {"DateFrom", DateFrom},
            {"DateTo", DateTo},
            {"daterange", DateRange},
            {"Page", "1" }
        });

    public bool HasNoDateFilter() =>
        !_queryUrl?.HasQueryParam("DateFrom") ?? false;

    public RouteValueDictionary WithoutTagFilter() =>
        _queryUrl is null
            ? new RouteValueDictionary()
            : _queryUrl.WithoutQueryParam(new List<string> { "tag", "Page" });
}