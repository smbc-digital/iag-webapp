namespace StockportWebapp.Config
{
    public class AmazonSESKeys
    {
        public readonly string Accesskey;
        public readonly string SecretKey;
        public AmazonSESKeys(string accessKey, string secretKey)
        {
            SecretKey = secretKey;
            Accesskey = accessKey;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Accesskey) && !string.IsNullOrWhiteSpace(SecretKey);
        }
    }
}
