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
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> {"category"});
        }

        public RouteValueDictionary AddCategoryFilter(string category)
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.AddQueriesToUrl(new Dictionary<string, string> { { "category", category } });
        }

        public bool HasNoCategoryFilter()
        {
            return !_queryUrl?.HasQueryParam("category") ?? false;
        }

        public RouteValueDictionary AddMonthFilter(DateTime startDate)
        {
            var dateto = startDate.Month == _timeProvider.Now().Month ? _timeProvider.Now() : startDate.AddMonths(1).AddDays(-1);

            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.AddQueriesToUrl(new Dictionary<string, string>
            {
                {"datefrom", startDate.ToString("yyyy-MM-dd")},
                {"dateto", dateto.ToString("yyyy-MM-dd")},
                {"daterange", "month"}

            });
        }

        public RouteValueDictionary WithoutDateFilter()
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> {"datefrom", "dateto","daterange"});
        }

        public bool HasNoDateFilter()
        {
            return !_queryUrl?.HasQueryParam("datefrom") ?? false;
        }

        public RouteValueDictionary WithoutTagFilter()
        {
            return _queryUrl == null ? new RouteValueDictionary() : _queryUrl.WithoutQueryParam(new List<string> { "tag" });
        }
    }
}