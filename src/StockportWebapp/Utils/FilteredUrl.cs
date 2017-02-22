using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
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
    }

    public class FilteredUrl : IFilteredUrl
    {
        private readonly ITimeProvider _timeProvider;
        private QueryUrl _queryUrl;

        public FilteredUrl(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void SetQueryUrl(QueryUrl queryUrl)
        {
            _queryUrl = queryUrl;
        }

        public RouteValueDictionary WithoutCategory()
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> {"Category"});
        }

        public RouteValueDictionary AddCategoryFilter(string category)
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.AddQueriesToUrl(new Dictionary<string, string> { { "Category", category } });
        }

        public bool HasNoCategoryFilter()
        {
            return !_queryUrl?.HasQueryParam("Category") ?? false;
        }

        public RouteValueDictionary AddMonthFilter(DateTime startDate)
        {
            var dateto = startDate.Month == _timeProvider.Now().Month ? _timeProvider.Now() : startDate.AddMonths(1).AddDays(-1);

            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.AddQueriesToUrl(new Dictionary<string, string>
            {
                {"DateFrom", startDate.ToString("yyyy-MM-dd")},
                {"DateTo", dateto.ToString("yyyy-MM-dd")},
                {"daterange", "month"}

            });
        }

        public RouteValueDictionary WithoutDateFilter()
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> {"DateFrom", "DateTo","daterange"});
        }

        public bool HasNoDateFilter()
        {
            return !_queryUrl?.HasQueryParam("DateFrom") ?? false;
        }

        public RouteValueDictionary WithoutTagFilter()
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> { "tag" });
        }
    }
}