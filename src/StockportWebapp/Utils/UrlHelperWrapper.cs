using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public interface IUrlHelperWrapper
    {
        string RouteUrl(RouteValueDictionary routeValueDictionary);
    }

    public class UrlHelperWrapper: IUrlHelperWrapper
    {
        private readonly IUrlHelper _actualUrlHelper;

        public UrlHelperWrapper(IUrlHelper actualUrlHelper)
        {
            _actualUrlHelper = actualUrlHelper;
        }

        public string RouteUrl(RouteValueDictionary routeValueDictionary)
        {
            return _actualUrlHelper.RouteUrl(routeValueDictionary);
        }
    }
}