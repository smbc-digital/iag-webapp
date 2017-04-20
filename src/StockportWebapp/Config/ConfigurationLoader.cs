using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace StockportWebapp.Config
{
    public class ConfigurationLoader
    {
        public ConfigurationLoader(IConfigurationBuilder configBuilder, string configPath)
        {
            ConfigPath = configPath;
            ConfigBuilder = configBuilder;
        }

        public string ConfigPath { get; }
        public readonly IConfigurationBuilder ConfigBuilder;

        public IConfigurationRoot LoadConfiguration(string envName, string contentRootPath)
        {

            var configuration = LoadAppSettings(envName, contentRootPath);
            var secretsLocation = configuration["secrets-location"];
            
            return LoadConfigurationWithSecrets(envName, secretsLocation);
        }

        private IConfigurationRoot LoadConfigurationWithSecrets(string envName, string secretsLocation)
        {
            var secretConfig = Path.Combine(secretsLocation, $"appsettings.{envName}.secrets.json");
            ConfigBuilder.AddJsonFile(secretConfig, true);
            ConfigBuilder.AddEnvironmentVariables();

            return ConfigBuilder.Build();
        }

        private IConfigurationRoot LoadAppSettings(string envName, string contentRootPath)
        {
            var appConfig = Path.Combine(ConfigPath, "appsettings.json");
            var envConfig = Path.Combine(ConfigPath, $"appsettings.{envName}.json");
            ConfigBuilder.SetBasePath(contentRootPath);
            ConfigBuilder.AddJsonFile(appConfig);
            ConfigBuilder.AddJsonFile(envConfig);

            return ConfigBuilder.Build();
        }
    }
}