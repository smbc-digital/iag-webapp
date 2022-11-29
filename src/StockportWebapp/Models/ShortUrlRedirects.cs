namespace StockportWebapp.Models
{
    public class ShortUrlRedirects
    {
        public BusinessIdRedirectDictionary Redirects;
        public DateTime LastUpdated;

        public ShortUrlRedirects(BusinessIdRedirectDictionary redirects)
        {
            Redirects = redirects;
        }

        public bool HasExpired() => LastUpdated < DateTime.Now.Subtract(new TimeSpan(0, 30, 0));
    }
}
