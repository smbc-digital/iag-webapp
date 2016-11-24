using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly FeatureToggles _featureToggles;

        public SearchController(IApplicationConfiguration config, BusinessId businessId, FeatureToggles featureToggles)
        {
            _businessId = businessId;
            _featureToggles = featureToggles;
            _config = config;
        }

        [Route("/search")]
        public async Task<IActionResult> Index(string query)
        {
            if (_featureToggles.Search) return NotFound();

            var urlSetting = _config.GetSearchUrl(_businessId.ToString());
            if (urlSetting.IsValid())
            {
                var url = string.Concat(urlSetting, query);
                return await Task.FromResult(Redirect(url));
            }
            return NotFound();
        }

        [Route("/postcode")]
        public async Task<IActionResult> Postcode(string query)
        {
            var urlSetting = _config.GetPostcodeSearchUrl(_businessId.ToString());
            if (urlSetting.IsValid())
            {
                var url = string.Concat(urlSetting, query);
                return await Task.FromResult(Redirect(url));
            }
            return NotFound();
        }
    }
}