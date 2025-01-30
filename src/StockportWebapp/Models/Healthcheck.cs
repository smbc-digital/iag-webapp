namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Healthcheck(string appVersion,
                        string businessId,
                        string sha,
                        Dictionary<string, Healthcheck> dependencies,
                        string environment,
                        List<RedisValueData> redisValueData)
{
    public readonly string AppVersion = appVersion;
    public readonly string SHA = sha;
    public readonly Dictionary<string, Healthcheck> Dependencies = dependencies ?? new();
    public readonly string Environment = environment;
    public readonly List<RedisValueData> RedisValueData = redisValueData;
    public readonly string BusinessId = businessId;
}

[ExcludeFromCodeCoverage]
public class UnavailableHealthcheck : Healthcheck
{
    public UnavailableHealthcheck() : base("Not available",
                                        "Not available",
                                        "Not available",
                                        new Dictionary<string, Healthcheck>(),
                                        "Not available",
                                        new List<RedisValueData>())
    { }
}