using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;

namespace StockportWebapp.ViewComponents
{
    public class HomepageEventsViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public HomepageEventsViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Event> events)
        {
            if (_featureToggles.LatestEventsHomepage) return await Task.FromResult(View("HomepageEventsWithListing", events));
            return await Task.FromResult(View("HomepageEventsWithoutListing"));

        }
    }
}
