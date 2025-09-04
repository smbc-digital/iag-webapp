namespace StockportWebapp.Models.Config;

public interface IEmailConfigurationBuilder
{
    AmazonSesClientConfiguration Build(string businessId);
}

public class EmailConfigurationBuilder(AmazonSESKeys amazonKeys, IApplicationConfiguration config) : IEmailConfigurationBuilder
{
    private readonly AmazonSESKeys _amazonKeys = amazonKeys;
    private readonly IApplicationConfiguration _config = config;

    public AmazonSesClientConfiguration Build(string businessId) =>
        new(_config.GetEmailHost(businessId),
            _config.GetEmailRegion(businessId),
            _config.GetEmailEmailFrom(businessId),
            _amazonKeys);
}