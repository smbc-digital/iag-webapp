using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
namespace StockportWebapp.ViewComponents
{
    public class LiveChatViewComponent : ViewComponent
    {
        private readonly FeatureToggles _featureToggles;

        public LiveChatViewComponent(FeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title, string text)
        {
            if (_featureToggles.LiveChat)
            {
                return await Task.FromResult(View("LiveChat", new LiveChat(title, text)));
            }
            return await Task.FromResult(View());
        }
    }
}

