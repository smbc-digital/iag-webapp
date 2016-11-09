using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class NewsCategoriesViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public NewsCategoriesViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(Newsroom newsroom)
        {
            if (_featureToggles.NewsCategory)
            {
                return await Task.FromResult(View("New", newsroom));
            }
            return await Task.FromResult(View("Old", newsroom));
        }
    }
}
