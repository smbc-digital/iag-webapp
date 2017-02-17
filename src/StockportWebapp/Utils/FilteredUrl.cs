using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class FilteredUrl
    {
        private readonly QueryUrl _queryUrl;

        public FilteredUrl(QueryUrl queryUrl)
        {
            this._queryUrl = queryUrl;
        }

        public RouteValueDictionary WithoutCategory()
        {
            return _queryUrl.WithoutQueryParam(new List<string>() {"category"});
        }

        public RouteValueDictionary AddCategoryFilter(string category)
        {
            return _queryUrl.AddQueriesToUrl(new Dictionary<string, string>() { { "category", category } });
        }

        public bool HasNoCategoryFilter()
        {
            return !_queryUrl.HasQueryParam("category");
        }

        public RouteValueDictionary AddMonthFilter(DateTime startDate)
        {
            var dateto = startDate.Month == DateTime.Now.Month ? DateTime.Now : startDate.AddMonths(1).AddDays(-1);

            return _queryUrl.AddQueriesToUrl(new Dictionary<string, string>()
            {
                {"datefrom", startDate.ToString("yyyy-MM-dd")},
                {"dateto", dateto.ToString("yyyy-MM-dd")},
                {"daterange", "month"}

            });
        }

        public RouteValueDictionary WithoutDateFilter()
        {
            return _queryUrl.WithoutQueryParam(new List<string>() {"datefrom", "dateto","daterange"});
        }

        public bool HasNoDateFilter()
        {
            return !_queryUrl.HasQueryParam("datefrom");
        }

        public RouteValueDictionary WithoutTagFilter()
        {
            return _queryUrl.WithoutQueryParam(new List<string>() { "tag" });
        }
    }
}