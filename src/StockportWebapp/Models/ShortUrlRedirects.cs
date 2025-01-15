namespace StockportWebapp.Models;

public class ShortUrlRedirects(BusinessIdRedirectDictionary redirects)
{
    public BusinessIdRedirectDictionary Redirects = redirects;
    public DateTime LastUpdated;

    public bool HasExpired() => LastUpdated < DateTime.Now.Subtract(new TimeSpan(0, 30, 0));
}