namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Redirects(BusinessIdRedirectDictionary shortUrlRedirects, BusinessIdRedirectDictionary legacyUrlRedirects)
{
    public readonly BusinessIdRedirectDictionary ShortUrlRedirects = shortUrlRedirects;
    public readonly BusinessIdRedirectDictionary LegacyUrlRedirects = legacyUrlRedirects;
}