using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.ViewComponents
{
    public class LiveChatViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public LiveChatViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_featureToggles.LiveChat)
            {
                return await Task.FromResult(View("LiveChat"));
            }
            return await Task.FromResult(View());
        }
    }
}
