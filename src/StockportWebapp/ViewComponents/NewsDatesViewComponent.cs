using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class NewsDatesViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public NewsDatesViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(Newsroom newsroom)
        {
            if (_featureToggles.NewsDateFilter)
            {
                return await Task.FromResult(View("New", newsroom));
            }
            return await Task.FromResult(View("Old", newsroom));
        }
    }
}
