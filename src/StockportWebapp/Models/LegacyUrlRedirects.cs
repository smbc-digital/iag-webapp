namespace StockportWebapp.Models;

public class LegacyUrlRedirects(BusinessIdRedirectDictionary businessIdRedirectDictionary)
{
    public BusinessIdRedirectDictionary Redirects = businessIdRedirectDictionary;
    public DateTime LastUpdated;

    public bool HasExpired() => LastUpdated < DateTime.Now.Subtract(new TimeSpan(0, 30, 0));
}