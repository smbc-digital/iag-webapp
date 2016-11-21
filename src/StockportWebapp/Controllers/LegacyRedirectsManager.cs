using System.Collections.Generic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebapp.Controllers
{
    public interface ILegacyRedirectsManager
    {
        string RedirectUrl();
    }

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
                var businessIdLegacyUrlRedirects = _legacyUrlRedirects.Redirects[_businessId.ToString()];

                if (businessIdLegacyUrlRedirects.ContainsKey(currentPath)) return businessIdLegacyUrlRedirects[currentPath];

                return GetShortUrlMatch(businessIdLegacyUrlRedirects, currentPath);
            }
            return string.Empty;
        }

        private string GetShortUrlMatch(RedirectDictionary businessIdLegacyUrlRedirects, string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            if (businessIdLegacyUrlRedirects.ContainsKey(string.Concat(url, "/*"))) return businessIdLegacyUrlRedirects[string.Concat(url, "/*")];

            return GetShortUrlMatch(businessIdLegacyUrlRedirects, GetShortenedUrl(url));
        }

        private string GetCurrentPath(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath;
        }

        private static string GetShortenedUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf('/'));
        }
    }
}