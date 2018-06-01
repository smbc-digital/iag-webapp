using System;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
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

        public ElasticSearchLogConfigurator(IConfigurationRoot configurationRoot)
        {
            _configuration = configurationRoot;

            var elasticSearchLogConfigurationSection = _configuration.GetSection(ElasticsearchConfigurationKey);
            var elasticSearchLogSecretConfigurationSection = _configuration.GetSection(ElasticsearchSecretsConfigurationKey);

            if (elasticSearchLogConfigurationSection != null && elasticSearchLogSecretConfigurationSection != null)
            {
                elasticSearchLogConfigurationSection.Bind(_elasticSearchLogConfiguration);
                elasticSearchLogSecretConfigurationSection.Bind(_elasticSearchLogSecretConfiguration);
            }
        }

        public void Configure(LoggerConfiguration loggerConfiguration)
        {
            if (_elasticSearchLogConfiguration != null
                && _elasticSearchLogSecretConfiguration != null
                && _elasticSearchLogConfiguration.Enabled)
            {
                var options = ElasticSearchLogConfigurator.CreateElasticsearchSinkOptions(_elasticSearchLogConfiguration, _elasticSearchLogSecretConfiguration);

                if (options != null)
                {
                    loggerConfiguration.WriteTo.Elasticsearch(options);
                    Log.Logger.Warning("ElasticSearch logging configuration added");
                }
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