namespace StockportWebapp.Models
{
    public class LegacyUrlRedirects
    {
        public BusinessIdRedirectDictionary Redirects;

        public LegacyUrlRedirects(BusinessIdRedirectDictionary businessIdRedirectDictionary)
        {
            Redirects = businessIdRedirectDictionary;
        }
    }
}