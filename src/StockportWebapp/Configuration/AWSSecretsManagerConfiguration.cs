namespace StockportWebapp.Configuration
{
    public class AWSSecretsManagerConfiguration
    {
        public string GlobalSecretPrefix { get; set; }
        public string EnvironmentGroupPrefix  { get; set; }
        public string SharedSecretPrefix { get; set; }
        public ICollection<string> SecretGroups { get; set; } = new List<string>();
    }
}
