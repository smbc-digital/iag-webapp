using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebapp.Controllers
{
    public class LegacyRedirectsManager : ILegacyRedirectsManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BusinessId _businessId;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;

        public LegacyRedirectsManager(IHttpContextAccessor httpContextAccessor, BusinessId businessId, LegacyUrlRedirects legacyUrlRedirects)
        {
            _httpContextAccessor = httpContextAccessor;
            _businessId = businessId;
            _legacyUrlRedirects = legacyUrlRedirects;
        }

        public string RedirectUrl()
        {
            if (_legacyUrlRedirects.Redirects.ContainsKey(_businessId.ToString()))
            {
                var currentPath = GetCurrentPath(_httpContextAccessor);
                if (_legacyUrlRedirects.Redirects[_businessId.ToString()].ContainsKey(currentPath))
                {
                    return _legacyUrlRedirects.Redirects[_businessId.ToString()][currentPath];
                }
            }
            return string.Empty;
        }

        private string GetCurrentPath(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath;
        }
    }
}