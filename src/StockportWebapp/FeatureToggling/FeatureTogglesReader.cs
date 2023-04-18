using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace StockportWebapp.FeatureToggling
{
    public class FeatureTogglesReader
    {
        private readonly string _path;
        private readonly string _appEnvironment;
        private readonly ILogger<FeatureTogglesReader> _logger;

        public FeatureTogglesReader(string path, string appEnvironment, ILogger<FeatureTogglesReader> logger)
        {
            _path = path;
            _appEnvironment = appEnvironment;
            _logger = logger;
        }

        public T Build<T>() where T : new()
        {
            if (!File.Exists(_path))
            {
                _logger.LogWarning($"No feature toggle configuration file found ({_path}). Setting all features to false.");
                return new T();
            }

            Dictionary<string, T> featureToggles;
            try
            {
                featureToggles = new DeserializerBuilder()
                    .IgnoreUnmatchedProperties()
                    .Build()
                    .Deserialize<Dictionary<string, T>>(ReadYaml());
            }
            catch (SemanticErrorException)
            {
                _logger.LogWarning($"Cannot parse feature toggles in {_path}. Setting all features to false.");
                return new T();
            }

            return AssignFeatureTogglesForCurrentEnvironment(featureToggles);
        }

        private T AssignFeatureTogglesForCurrentEnvironment<T>(Dictionary<string, T> featureTogglesResponse) where T : new()
        {
            featureTogglesResponse.TryGetValue(_appEnvironment, out T featureToggles);
            if (featureToggles is not null)
            {
                LogFeatureTogglesInfo(featureToggles);
            }
            else
            {
                _logger.LogWarning($"No feature toggle configuration found for environment: {_appEnvironment}. Setting all features to false.");
                featureToggles = new T();
            }

            return featureToggles;
        }

        private void LogFeatureTogglesInfo<T>(T featureToggles)
        {
            var featureTogglesDescription = new StringBuilder();

            foreach (var propertyInfo in featureToggles.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(featureToggles);
                featureTogglesDescription.Append($"{propertyInfo.Name}: {value}, ");
            }

            _logger?.LogInformation($"Feature Toggles for: {_appEnvironment}\n{featureTogglesDescription}");
        }

        private IParser ReadYaml()
        {
            var yaml = File.ReadAllText(_path);
            var innerParser = new Parser(new StringReader(yaml));
            return new MergingParser(innerParser);
        }
    }
}