using System;
using StockportWebapp.Config;

namespace StockportWebapp.AmazonSES
{
    public class AmazonSesClientConfiguration
    {
        private readonly AppSetting _hostSetting;
        private readonly AppSetting _regionSetting;
        private readonly AppSetting _emailFromSetting;
        private readonly AmazonSESKeys _awsKeys;

        public readonly string Host;
        public readonly string Region;
        public readonly string Endpoint;
        public readonly string AwsAccessKeyId;
        public readonly string AwsSecretAccessKey;
        public readonly string EmailFrom;

        public AmazonSesClientConfiguration(AppSetting hostSetting, AppSetting regionSetting, AppSetting emailFromSetting, AmazonSESKeys awsKeys)
        {
            _hostSetting = hostSetting;
            _regionSetting = regionSetting;
            _emailFromSetting = emailFromSetting;
            _awsKeys = awsKeys;

            Host = hostSetting.ToString();
            Region = regionSetting.ToString();
            Endpoint = $"https://{Host}";
            AwsAccessKeyId = awsKeys.Accesskey;
            AwsSecretAccessKey = awsKeys.SecretKey;
            EmailFrom = emailFromSetting.ToString();
        }

        public bool IsValid()
        {
            return _hostSetting.IsValid() && _regionSetting.IsValid() && _emailFromSetting.IsValid() && _awsKeys.IsValid();
        }

        public string ValidityToString()
        {
            return $"Host: {_hostSetting.IsValid()}, Region: {_regionSetting.IsValid()}, " +
                   $"EmailFrom: {_emailFromSetting.IsValid()}, AwsKeys: {_awsKeys.IsValid()}";
        }
    }
}
