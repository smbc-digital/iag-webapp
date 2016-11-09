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

        public Healthcheck(string appVersion, string sha, FeatureToggles featureToggles,
            Dictionary<string, Healthcheck> dependencies)
        {
            AppVersion = appVersion;
            SHA = sha;
            FeatureToggles = featureToggles;
            Dependencies = dependencies ?? new Dictionary<string, Healthcheck>();
        }
    }

    public class UnavailableHealthcheck : Healthcheck
    {
        public UnavailableHealthcheck()
            : base("Not available", "Not available", null, new Dictionary<string, Healthcheck>())
        {
        }
    }
}