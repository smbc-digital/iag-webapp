namespace StockportWebapp.Models
{
    public class ShortUrlRedirects
    {
        public BusinessIdRedirectDictionary Redirects;

        public ShortUrlRedirects(BusinessIdRedirectDictionary redirects)
        {
            Redirects = redirects;
        }
    }
}
