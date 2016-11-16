using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Controllers
{
    public class LegacyRedirects : ILegacyRedirects
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LegacyRedirects(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string RedirectUrl()
        {
            // #624 stubbed for over-arching test in RoutesTestHealthyStockportIntegrationOnly
            if (_httpContextAccessor.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath == "/services/councildemocracy/counciltax/difficultypaying")
            {
                return "/council-tax";
            }
            return string.Empty;
        }
    }
}