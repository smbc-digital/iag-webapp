namespace StockportWebapp.Models.Config;

public class AmazonSESKeys(string accessKey, string secretKey)
{
    public readonly string Accesskey = accessKey;
    public readonly string SecretKey = secretKey;

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Accesskey) && !string.IsNullOrWhiteSpace(SecretKey);
}