using System.Collections.Generic;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Models
{
    public class Healthcheck
    {
        public readonly string AppVersion;
        public readonly string SHA;
        public readonly FeatureToggles FeatureToggles;
        public readonly Dictionary<string, Healthcheck> Dependencies;
        public readonly string Environment;
        public readonly List<RedisValueData> RedisValueData;

        public Healthcheck(string appVersion, string sha, FeatureToggles featureToggles,
            Dictionary<string, Healthcheck> dependencies, string environment, List<RedisValueData> redisValueData)
        {
            AppVersion = appVersion;
            SHA = sha;
            FeatureToggles = featureToggles;
            Dependencies = dependencies ?? new Dictionary<string, Healthcheck>();
            Environment = environment;
            RedisValueData = redisValueData;
        }
    }

    public class UnavailableHealthcheck : Healthcheck
    {
        public UnavailableHealthcheck()
            : base("Not available", "Not available", null, new Dictionary<string, Healthcheck>(), "Not available", new List<RedisValueData>())
        {
        }
    }
}