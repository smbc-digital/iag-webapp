using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public class NewsFilter
    {
        private readonly QueryUrl queryUrl;

        public NewsFilter(QueryUrl queryUrl)
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

        public RouteValueDictionary AddMonthFilter(DateTime startDate)
        {
            return queryUrl.AddQueriesToUrl(new Dictionary<string, string>()
            {
                {"datefrom", startDate.ToString("yyyy-MM-dd")},
                {"dateto", startDate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}
            });
        }

        public RouteValueDictionary WithoutDateFilter()
        {
            return queryUrl.WithoutQueryParam(new List<string>() {"datefrom", "dateto"});
        }
    }
}