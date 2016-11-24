using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public SearchViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id, string placeholder)
        {
            if (_featureToggles.Search)
            {
                return await Task.FromResult(View("New", new Tuple<string, string>(id, placeholder) { }));
            }

            return await Task.FromResult(View("Old", placeholder));
        }
    }
}
