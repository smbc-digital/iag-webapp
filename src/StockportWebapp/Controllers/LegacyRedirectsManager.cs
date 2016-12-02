using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebapp.Controllers
{
    public interface ILegacyRedirectsManager
    {
        string RedirectUrl(string url);
    }

    public class LegacyRedirectsManager : ILegacyRedirectsManager
    {
        private readonly BusinessId _businessId;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;

        public LegacyRedirectsManager(BusinessId businessId, LegacyUrlRedirects legacyUrlRedirects)
        {
            _businessId = businessId;
            _legacyUrlRedirects = legacyUrlRedirects;
        }

        public string RedirectUrl(string url)
        {
            if (!DictionaryContainsBusinessId(_legacyUrlRedirects.Redirects, _businessId.ToString())) return string.Empty;

            var businessIdLegacyUrlRedirects = _legacyUrlRedirects.Redirects[_businessId.ToString()];

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return businessIdLegacyUrlRedirects.ContainsKey(url) 
                ? businessIdLegacyUrlRedirects[url] 
                : GetWildcardShortUrlMatch(businessIdLegacyUrlRedirects, url);
        }

        private static bool DictionaryContainsBusinessId(BusinessIdRedirectDictionary redirects, string businessId)
        {
            return redirects.ContainsKey(businessId);
        }

        private static string GetWildcardShortUrlMatch(RedirectDictionary businessIdLegacyUrlRedirects, string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            return businessIdLegacyUrlRedirects.ContainsKey(ConcatWithWildcard(url)) 
                ? businessIdLegacyUrlRedirects[ConcatWithWildcard(url)] 
                : GetWildcardShortUrlMatch(businessIdLegacyUrlRedirects, GetShortenedUrl(url));
        }

        private static string ConcatWithWildcard(string url)
        {
            return string.Concat(url, "/*");
        }

        private static string GetShortenedUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf('/'));
        }
    }
}