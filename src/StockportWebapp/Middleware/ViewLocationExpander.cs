using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        private readonly string _businessId;
        private readonly FeatureToggles _featureToggles;

        public ViewLocationExpander(string businessId, FeatureToggles featureToggles)
        {
            _businessId = businessId;
            _featureToggles = featureToggles;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (_featureToggles.BusinessIdFromRequest)
            {
                return viewLocations.Select(f => f.Replace("/Views/", $"/Views/{context.Values["theme"]}/"))
                .Append("/Views/Shared/{0}.cshtml")
                .Append("/Views/Shared/{1}/{0}.cshtml");
            }

            return viewLocations.Select(f => f.Replace("/Views/", $"/Views/{_businessId}/"))
            .Append("/Views/Shared/{0}.cshtml")
            .Append("/Views/Shared/{1}/{0}.cshtml");
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["theme"] = context.ActionContext.HttpContext.Request.Headers["BUSINESS-ID"].ToString();
        }
    }
}