using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class FilteredUrl
    {
        private readonly QueryUrl queryUrl;

        public FilteredUrl(QueryUrl queryUrl)
        {
            this.queryUrl = queryUrl;
        }

        public RouteValueDictionary WithoutCategory()
        {
            return queryUrl.WithoutQueryParam(new List<string>() {"category"});
        }

        public RouteValueDictionary AddCategoryFilter(string category)
        {
            return queryUrl.AddQueriesToUrl(new Dictionary<string, string>() { { "category", category } });
        }

        public bool HasNoCategoryFilter()
        {
            return !queryUrl.HasQueryParam("category");
        }

        public RouteValueDictionary AddMonthFilter(DateTime startDate)
        {
            return queryUrl.AddQueriesToUrl(new Dictionary<string, string>()
            {
                {"datefrom", startDate.ToString("dd/MM/yyyy")},
                {"dateto", startDate.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")}
            });
        }

        public RouteValueDictionary WithoutDateFilter()
        {
            return queryUrl.WithoutQueryParam(new List<string>() {"datefrom", "dateto","daterange"});
        }

        public bool HasNoDateFilter()
        {
            return !queryUrl.HasQueryParam("datefrom");
        }

        public RouteValueDictionary WithoutTagFilter()
        {
            return queryUrl.WithoutQueryParam(new List<string>() { "tag" });
        }
    }
}