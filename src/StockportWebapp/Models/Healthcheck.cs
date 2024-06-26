namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Healthcheck
{
    public readonly string AppVersion;
    public readonly string SHA;
    public readonly Dictionary<string, Healthcheck> Dependencies;
    public readonly string Environment;
    public readonly List<RedisValueData> RedisValueData;
    public readonly string BusinessId;

    public Healthcheck(
        string appVersion, 
        string businessId, 
        string sha,
        Dictionary<string, Healthcheck> dependencies, 
        string environment, 
        List<RedisValueData> redisValueData)
    {
        AppVersion = appVersion;
        BusinessId = businessId;
        SHA = sha;
        Dependencies = dependencies ?? new Dictionary<string, Healthcheck>();
        Environment = environment;
        RedisValueData = redisValueData;
    }
}

[ExcludeFromCodeCoverage]
public class UnavailableHealthcheck : Healthcheck
{
    public UnavailableHealthcheck()
        : base("Not available", "Not available", "Not available", new Dictionary<string, Healthcheck>(), "Not available", new List<RedisValueData>())
    {
    }
}