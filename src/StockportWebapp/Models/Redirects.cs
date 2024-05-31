namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Redirects
{
    public readonly BusinessIdRedirectDictionary ShortUrlRedirects;
    public readonly BusinessIdRedirectDictionary LegacyUrlRedirects;

    public Redirects(BusinessIdRedirectDictionary shortUrlRedirects, BusinessIdRedirectDictionary legacyUrlRedirects)
    {
        ShortUrlRedirects = shortUrlRedirects;
        LegacyUrlRedirects = legacyUrlRedirects;
    }
}