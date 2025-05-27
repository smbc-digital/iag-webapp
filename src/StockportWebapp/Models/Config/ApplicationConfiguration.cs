namespace StockportWebapp.Models.Config;

public interface IApplicationConfiguration
{
    AppSetting GetEmailAlertsUrl(string businessId);
    AppSetting GetPostcodeSearchUrl(string businessId);
    AppSetting GetGoogleAnalyticsCode(string buisnessId);
    AppSetting GetRssEmail(string businessId);
    Uri GetContentApiUri();
    Uri GetContentApiUrlRoot();
    Uri GetStockportApiUri();
    Uri GetGoogleAnalyticsUri();
    AppSetting GetEmailHost(string businessId);
    AppSetting GetEmailRegion(string businessId);
    AppSetting GetEmailEmailFrom(string businessId);
    AppSetting GetEmailAlertsNewSubscriberUrl(string businessId);
    AppSetting GetReCaptchaKey();
    int GetFooterCache(string businessId);
    int GetHeaderCache(string businessId);
    string GetMyAccountUrl();
    string GetStaticAssetsRootUrl();
    int GetNewsDefaultPageSize(string businessId);
    int GetEventsDefaultPageSize(string businessId);
    string GetContentApiAuthenticationKey();
    string GetWebAppClientId();
    string GetDigitalStockportLink();
    StylesheetsConfiguration GetStylesheetConfig();
    AnalyticsConfigurationModel GetAnalyticsConfig();
    AnalyticsConfigurationModel GetAnalyticsConfig(string businessId);
}

public class ApplicationConfiguration(IConfiguration appsettings) : IApplicationConfiguration
{
    private readonly IConfiguration _appsettings = appsettings;

    public AppSetting GetEmailAlertsUrl(string businessid) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessid}:EmailAlerts"]);

    public AppSetting GetEmailAlertsNewSubscriberUrl(string businessid) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessid}:EmailAlertsNewSubscriber"]);

    public string GetStaticAssetsRootUrl() =>
        _appsettings["StaticAssetsRootUrl"];

    public AppSetting GetPostcodeSearchUrl(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:Postcode"]);

    public AppSetting GetGoogleAnalyticsCode(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:GoogleAnalytics"]);

    public Uri GetGoogleAnalyticsUri() =>
        new(_appsettings["stockportgov:GoogleAnalyticsApiUrl"]);

    public AppSetting GetRssEmail(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:RssEmail"]);

    public Uri GetContentApiUri() =>
        new(_appsettings["ContentApiUrl"]);

    public Uri GetContentApiUrlRoot() =>
        new(_appsettings["ContentApiUrlRoot"]);

    public Uri GetStockportApiUri() =>
        new(_appsettings["StockportApiUrl"]);

    public AppSetting GetEmailHost(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:Host"]);

    public AppSetting GetEmailRegion(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:Region"]);

    public AppSetting GetEmailEmailFrom(string businessId) =>
        AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:EmailFrom"]);

    public AppSetting GetReCaptchaKey() =>
        AppSetting.GetAppSetting(_appsettings[$"ReCaptcha:SiteKey"]);

    public StylesheetsConfiguration GetStylesheetConfig()
    {
        StylesheetsConfiguration stylesheetConfig = new();
        _appsettings.GetSection("stockportgov:StylesheetsConfiguration").Bind(stylesheetConfig);

        return stylesheetConfig;
    }

    public AnalyticsConfigurationModel GetAnalyticsConfig()
    {
        AnalyticsConfigurationModel config = new();
        _appsettings.GetSection("stockportgov:Analytics").Bind(config);

        return config;
    }

    public AnalyticsConfigurationModel GetAnalyticsConfig(string businessId)
    {
        AnalyticsConfigurationModel config = new();
        _appsettings.GetSection($"{businessId}:Analytics").Bind(config);
        return config;
    }

    public int GetFooterCache(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:FooterCache"], out int output);

        return output;
    }

    public int GetHeaderCache(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:HeaderCache"], out int output);

        return output;
    }

    public string GetMyAccountUrl() =>
        _appsettings["myAccountUrl"];

    public int GetNewsDefaultPageSize(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:NewsDefaultPageSize"], out int result);

        return result;
    }
    public int GetEventsDefaultPageSize(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:EventsDefaultPageSize"], out int result);

        return result;
    }
 
    public string GetContentApiAuthenticationKey() =>
        _appsettings["ContentApiAuthenticationKey"];
    
    public string GetWebAppClientId() =>
        _appsettings["WebAppClientId"];

    public string GetDigitalStockportLink() =>
        _appsettings["stockportgov:DigitalStockportLink"];
}