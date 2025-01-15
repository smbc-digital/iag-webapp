namespace StockportWebapp.Controllers;

public interface ILegacyRedirectsManager
{
    Task<string> RedirectUrl(string url);
}

public class LegacyRedirectsMapper(BusinessId businessId,
                                LegacyUrlRedirects legacyUrlRedirects,
                                ShortUrlRedirects shortUrlRedirects,
                                IRepository repository) : ILegacyRedirectsManager
{
    private readonly BusinessId _businessId = businessId;
    private readonly LegacyUrlRedirects _legacyUrlRedirects = legacyUrlRedirects;
    private readonly ShortUrlRedirects _shortUrlRedirects = shortUrlRedirects;
    private readonly IRepository _repository = repository;

    public async Task<string> RedirectUrl(string url)
    {
        if (_legacyUrlRedirects.HasExpired())
        {
            HttpResponse response = await _repository.GetRedirects();
            Redirects redirects = response.Content as Redirects;

            _shortUrlRedirects.Redirects = redirects.ShortUrlRedirects;
            _shortUrlRedirects.LastUpdated = DateTime.Now;
            _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
            _legacyUrlRedirects.LastUpdated = DateTime.Now;
        }

        if (!DictionaryContainsBusinessId(_legacyUrlRedirects.Redirects, _businessId.ToString()))
            return string.Empty;

        RedirectDictionary businessIdLegacyUrlRedirects = _legacyUrlRedirects.Redirects[_businessId.ToString()];

        if (url.EndsWith("/"))
            url = url.Substring(0, url.Length - 1);

        return businessIdLegacyUrlRedirects.ContainsKey(url)
            ? businessIdLegacyUrlRedirects[url]
            : GetWildcardShortUrlMatch(businessIdLegacyUrlRedirects, url);
    }

    private static bool DictionaryContainsBusinessId(BusinessIdRedirectDictionary redirects, string businessId) =>
        redirects.ContainsKey(businessId);

    private static string GetWildcardShortUrlMatch(RedirectDictionary businessIdLegacyUrlRedirects, string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        return businessIdLegacyUrlRedirects.ContainsKey(ConcatWithWildcard(url))
            ? businessIdLegacyUrlRedirects[ConcatWithWildcard(url)]
            : GetWildcardShortUrlMatch(businessIdLegacyUrlRedirects, GetShortenedUrl(url));
    }

    private static string ConcatWithWildcard(string url) =>
        string.Concat(url, "/*");

    private static string GetShortenedUrl(string url) =>
        url.Substring(0, url.LastIndexOf('/'));
}