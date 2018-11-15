using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        /**
         * NOTE: There is an issue in AWS ElasticBeanstalk that means our environment variables aren't set.
         * To address this we need to manually parse a configuration on the box.
         * See http://stackoverflow.com/questions/40127703/aws-elastic-beanstalk-environment-in-asp-net-core-1-0
         **/
        public string EnvironmentName(IHostingEnvironment env)
        {
            // If we haven't set an ASPNETCORE_ENVIRONMENT then it defaults to Production.
            // This is the case on Int / QA / Stage / Prod
            if (env.EnvironmentName == "Production")
            {
                var iisEnvironmentConfiguration = AwsEnvironmentConfiguration();
                return iisEnvironmentConfiguration["ASPNETCORE_ENVIRONMENT"];
            }

            return env.EnvironmentName;
        }

        private static Dictionary<string, string> AwsEnvironmentConfiguration()
        {
            var awsConfig = new ConfigurationBuilder()
                .SetBasePath("C:\\Program Files\\Amazon\\ElasticBeanstalk\\config")
                .AddJsonFile("containerconfiguration", optional: true, reloadOnChange: true)
                .Build();

            var iisSection = awsConfig.GetSection("iis:env");
            var iisEnvironmentConfiguration = iisSection.GetChildren()
                .Aggregate(new Dictionary<string, string>(), (dict, entry) =>
                {
                    var envVarParts = entry.Value.Split('=');
                    dict.Add(envVarParts[0], envVarParts[1]);
                    return dict;
                });
            return iisEnvironmentConfiguration;
        }

        public IConfigurationRoot LoadConfiguration(IHostingEnvironment env, string contentRootPath)
        {
            var envName = EnvironmentName(env);

            var configuration = LoadAppSettings(envName, contentRootPath);
            var secretsLocation = configuration["secrets-location"];

            return LoadConfigurationWithSecrets(envName, secretsLocation, contentRootPath);
        }

        private IConfigurationRoot LoadConfigurationWithSecrets(string envName, string secretsLocation, string contentRootPath)
        {
            var secretConfig = Path.Combine(secretsLocation, $"appsettings.{envName}.secrets.json");
            var appConfig = Path.Combine(ConfigPath, "appsettings.json");
            var envConfig = Path.Combine(ConfigPath, $"appsettings.{envName}.json");

            ConfigBuilder.SetBasePath(contentRootPath);
            ConfigBuilder.AddJsonFile(appConfig);
            ConfigBuilder.AddJsonFile(envConfig);
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