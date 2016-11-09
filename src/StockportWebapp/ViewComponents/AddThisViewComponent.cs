using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;

namespace StockportWebapp.ViewComponents
{
    public class AddThisViewComponent : ViewComponent
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        public AddThisViewComponent(IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var shareIdSetting = _config.GetAddThisShareId(_businessId.ToString());
            return await Task.FromResult(View(model: shareIdSetting));
        }
    }
}
