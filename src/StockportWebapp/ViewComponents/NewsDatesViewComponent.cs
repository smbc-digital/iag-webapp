using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.ViewComponents
{
    public class NewsDatesViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public NewsDatesViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<DateTime> dates)
        {
            if (_featureToggles.NewsDateFilter)
            {
                return await Task.FromResult(View("New", dates));
            }
            return await Task.FromResult(View("Old"));
        }
    }
}
