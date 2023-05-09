namespace StockportWebapp.Models.Config.AnalyticsConfiguration;

public interface IAnalyticsConfiguration
{
    AnalyticsConfigurationModel GetTrackerCode();

    string GetApiUrl();
}