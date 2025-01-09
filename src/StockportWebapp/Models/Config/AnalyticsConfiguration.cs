namespace StockportWebapp.Models.Config;

public class AnalyticsConfiguration(IApplicationConfiguration configuration) : IAnalyticsConfiguration
{
    private readonly IApplicationConfiguration _configuration = configuration;

    public string GetApiUrl() =>
        _configuration.GetGoogleAnalyticsUri().ToString();

    public AnalyticsConfigurationModel GetTrackerCode() =>
        _configuration.GetAnalyticsConfig();
}

public class AnalyticsConfigurationModel
{
    public string SiteImprove { get; set; }

    public string TagManagerId { get; set; }

    public string AnalyticsTrackingCode { get; set; }
}