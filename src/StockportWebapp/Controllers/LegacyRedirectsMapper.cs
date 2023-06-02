namespace StockportWebapp.Controllers;

public interface ILegacyRedirectsManager
{
    Task<string> RedirectUrl(string url);
}

public class LegacyRedirectsMapper : ILegacyRedirectsManager
{
    private readonly BusinessId _businessId;
    private readonly LegacyUrlRedirects _legacyUrlRedirects;
    private readonly ShortUrlRedirects _shortUrlRedirects;
    private readonly IRepository _repository;

    public LegacyRedirectsMapper(BusinessId businessId, LegacyUrlRedirects legacyUrlRedirects, ShortUrlRedirects shortUrlRedirects, IRepository repository)
    {
        _businessId = businessId;
        _legacyUrlRedirects = legacyUrlRedirects;
        _shortUrlRedirects = shortUrlRedirects;
        _repository = repository;
    }

    public async Task<string> RedirectUrl(string url)
    {
        if (_legacyUrlRedirects.HasExpired())
        {
            var response = await _repository.GetRedirects();
            var redirects = response.Content as Redirects;
            _shortUrlRedirects.Redirects = redirects.ShortUrlRedirects;
            _shortUrlRedirects.LastUpdated = System.DateTime.Now;
            _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
            _legacyUrlRedirects.LastUpdated = System.DateTime.Now;
        }
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