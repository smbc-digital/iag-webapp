namespace StockportWebapp.Models;

public class LegacyUrlRedirects
{
    public BusinessIdRedirectDictionary Redirects;
    public DateTime LastUpdated;

    public LegacyUrlRedirects(BusinessIdRedirectDictionary businessIdRedirectDictionary)
    {
        Redirects = businessIdRedirectDictionary;
    }

    public bool HasExpired() => LastUpdated < DateTime.Now.Subtract(new TimeSpan(0, 30, 0));
}