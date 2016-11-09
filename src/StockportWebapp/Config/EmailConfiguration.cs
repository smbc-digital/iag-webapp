using Microsoft.Extensions.Configuration;
using StockportWebapp.AmazonSES;
using System;

namespace StockportWebapp.Config
{
    public interface IEmailConfigurationBuilder
    {
        AmazonSesClientConfiguration Build(string businessId);
    }

    public class EmailConfigurationBuilder : IEmailConfigurationBuilder
    {
        private readonly AmazonSESKeys _amazonKeys;
        private readonly IApplicationConfiguration _config;

        public EmailConfigurationBuilder(AmazonSESKeys amazonKeys, IApplicationConfiguration config)
        {
            _amazonKeys = amazonKeys;
            _config = config;
        }

        public AmazonSesClientConfiguration Build(string businessId)
        {
            var host = _config.GetEmailHost(businessId);
            var region = _config.GetEmailRegion(businessId);
            var emailFrom = _config.GetEmailEmailFrom(businessId);       

            return new AmazonSesClientConfiguration(host, region,
                                                    emailFrom, _amazonKeys);
        }
    }
}
