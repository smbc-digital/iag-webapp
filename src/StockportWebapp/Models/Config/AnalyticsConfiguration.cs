namespace StockportWebapp.Models.Config;

public class AnalyticsConfiguration : IAnalyticsConfiguration
{
    private readonly IApplicationConfiguration _configuration;

    public AnalyticsConfiguration(IApplicationConfiguration configuration) => _configuration = configuration;
   
    public string GetApiUrl() => _configuration.GetGoogleAnalyticsUri().ToString();

    public AnalyticsConfigurationModel GetTrackerCode() => _configuration.GetAnalyticsConfig();

    public AnalyticsConfigurationModel GetTrackerCode(string businessId) => _configuration.GetAnalyticsConfig(businessId);
}

public class AnalyticsConfigurationModel
{
    public string SiteImprove { get; set; }

    public string TagManagerId { get; set; }

    public string AnalyticsTrackingCode { get; set; }
}