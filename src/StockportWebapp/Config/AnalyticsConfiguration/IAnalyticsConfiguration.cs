namespace StockportWebapp.Config.AnalyticsConfiguration
{
    public interface IAnalyticsConfiguration
    {
        AnalyticsConfigurationModel GetTrackerCode();

        string GetApiUrl();
    }
}