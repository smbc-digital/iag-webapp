namespace StockportWebapp.Config.AnalyticsConfiguration
{
    public class AnalyticsConfiguration : IAnalyticsConfiguration
    {
        private readonly IApplicationConfiguration _configuration;

        public AnalyticsConfiguration(IApplicationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetApiUrl()
        {
            return _configuration.GetGoogleAnalyticsUri().AbsoluteUri;
        }

        public AnalyticsConfigurationModel GetTrackerCode()
        {
            return _configuration.GetAnalyticsConfig();
        }
    }

    public class AnalyticsConfigurationModel
    {
        public string SiteImprove { get; set; }

        public string TagManagerId { get; set; }

        public string AnalyticsTrackingCode { get; set; }
    }
}