using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StockportWebapp.Config;

namespace StockportWebapp.ViewComponents
{
    public class GoogleAnalyticsViewComponent : ViewComponent
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        public GoogleAnalyticsViewComponent(IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var googleAnalyticsCode = _config.GetGoogleAnalyticsCode(_businessId.ToString());
            return await Task.FromResult(View(model: googleAnalyticsCode));
        }
    }
}