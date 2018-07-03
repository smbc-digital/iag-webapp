using System;
using System.Linq;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace StockportWebapp.Config
{
    public class ElasticSearchLogConfigurator
    {
        private const string ElasticsearchConfigurationKey = "ElasticSearchAwsConfiguration";
        private const string ElasticsearchSecretsConfigurationKey = "ElasticSearchAwsSecretsConfiguration";
        private readonly IConfigurationRoot _configuration;
        private ElasticSearchLogConfiguration _elasticSearchLogConfiguration = new ElasticSearchLogConfiguration();
        private ElasticSearchLogSecretConfiguration _elasticSearchLogSecretConfiguration = new ElasticSearchLogSecretConfiguration();
        private Microsoft.Extensions.Logging.ILogger Logger { get; set; }

        public ElasticSearchLogConfigurator(IConfigurationRoot configurationRoot, Microsoft.Extensions.Logging.ILogger logger)
        {
            Logger = logger;

            _configuration = configurationRoot;

            var elasticSearchLogConfigurationSection = _configuration.GetSection(ElasticsearchConfigurationKey);
            var elasticSearchLogSecretConfigurationSection = _configuration.GetSection(ElasticsearchSecretsConfigurationKey);

            if (elasticSearchLogConfigurationSection.AsEnumerable().Any() && elasticSearchLogSecretConfigurationSection.AsEnumerable().Any())
            {
                elasticSearchLogConfigurationSection.Bind(_elasticSearchLogConfiguration);
                elasticSearchLogSecretConfigurationSection.Bind(_elasticSearchLogSecretConfiguration);
                Logger.LogInformation("ElasticSearch is configured");
            }
            else
            {
                _elasticSearchLogConfiguration.Enabled = false;
                Logger.LogWarning("ElasticSearch is not configured");
            }
        }

        public void Configure(LoggerConfiguration loggerConfiguration)
        { 
            if(!_elasticSearchLogConfiguration.Enabled)
            {
                Logger.LogWarning("ElasticSearch logging is not enabled");
                return;
            }
            
            var options = ElasticSearchLogConfigurator.CreateElasticsearchSinkOptions(_elasticSearchLogConfiguration, _elasticSearchLogSecretConfiguration);
            if (options != null)
            {
                loggerConfiguration.WriteTo.Elasticsearch(options);   
            }
        }

        public static ElasticsearchSinkOptions CreateElasticsearchSinkOptions(ElasticSearchLogConfiguration elasticSearchLogConfiguration, ElasticSearchLogSecretConfiguration elasticSearchLogSecretConfiguration)
        {
            if (elasticSearchLogConfiguration == null || elasticSearchLogSecretConfiguration == null)
            {
                return null;
            }

            var singleNodeConnectionPool = new SingleNodeConnectionPool(elasticSearchLogConfiguration.Uri);

            var awsHttpConnection = new AwsHttpConnection(elasticSearchLogConfiguration.Region, new StaticCredentialsProvider(
                new AwsCredentials
                {
                    AccessKey = elasticSearchLogSecretConfiguration.AccessKey,
                    SecretKey = elasticSearchLogSecretConfiguration.SecretKey
                }));

            var options = new ElasticsearchSinkOptions(elasticSearchLogConfiguration.Uri)
            {
                IndexFormat = elasticSearchLogConfiguration.IndexFormat,
                InlineFields = elasticSearchLogConfiguration.InlineFields,
                MinimumLogEventLevel = elasticSearchLogConfiguration.MinimumLogLevel,
                ModifyConnectionSettings = conn =>
                {
                    return new ConnectionConfiguration(singleNodeConnectionPool, awsHttpConnection);
                }
            };

            return options;
        }
    }
}