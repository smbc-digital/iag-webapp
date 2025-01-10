namespace StockportWebapp.Models.Config;

public interface IAnalyticsConfiguration
{
    AnalyticsConfigurationModel GetTrackerCode();

    AnalyticsConfigurationModel GetTrackerCode(string key);

    string GetApiUrl();
}