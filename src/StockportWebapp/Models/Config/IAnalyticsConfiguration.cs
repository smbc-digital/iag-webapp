namespace StockportWebapp.Models.Config;

public interface IAnalyticsConfiguration
{
    AnalyticsConfigurationModel GetTrackerCode();

    string GetApiUrl();
}