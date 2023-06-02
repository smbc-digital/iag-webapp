namespace StockportWebapp.Models.Config;

public interface IApplicationConfiguration
{
    AppSetting GetEmailAlertsUrl(string businessId);
    AppSetting GetPostcodeSearchUrl(string businessId);
    AppSetting GetGoogleAnalyticsCode(string buisnessId);
    AppSetting GetAddThisShareId(string businessId);
    AppSetting GetRssEmail(string businessId);
    Uri GetContentApiUri();
    Uri GetContentApiUrlRoot();
    Uri GetStockportApiUri();
    Uri GetGoogleAnalyticsUri();
    AppSetting GetEmailHost(string businessId);
    AppSetting GetEmailRegion(string businessId);
    AppSetting GetEmailEmailFrom(string businessId);
    AppSetting GetEmailAlertsNewSubscriberUrl(string businessId);
    AppSetting GetGroupSubmissionEmail(string businessId);
    AppSetting GetGroupArchiveEmail(string businessId);
    AppSetting GetReCaptchaKey();
    int GetFooterCache(string businessId);
    string GetGroupManageContactUrl();
    string GetMyAccountUrl();
    string GetStaticAssetsRootUrl();
    int GetNewsDefaultPageSize(string businessId);
    int GetEventsDefaultPageSize(string businessId);
    int GetGroupsDefaultPageSize(string businessId);
    string GetContentApiAuthenticationKey();
    string GetWebAppClientId();
    string GetDigitalStockportLink();
    List<ArchiveEmailPeriod> GetArchiveEmailPeriods();
    StylesheetsConfiguration GetStylesheetConfig();
    AnalyticsConfigurationModel GetAnalyticsConfig();
    string GetStaleGroupsSecret();
}

public class ApplicationConfiguration : IApplicationConfiguration
{
    private readonly IConfiguration _appsettings;

    public ApplicationConfiguration(IConfiguration appsettings)
    {
        _appsettings = appsettings;
    }

    public AppSetting GetEmailAlertsUrl(string businessid)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessid}:EmailAlerts"]);
    }

    public AppSetting GetEmailAlertsNewSubscriberUrl(string businessid)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessid}:EmailAlertsNewSubscriber"]);
    }

    public string GetStaticAssetsRootUrl()
    {
        return _appsettings["StaticAssetsRootUrl"];
    }

    public AppSetting GetPostcodeSearchUrl(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Postcode"]);
    }

    public AppSetting GetGoogleAnalyticsCode(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GoogleAnalytics"]);
    }

    public Uri GetGoogleAnalyticsUri()
    {
        return new Uri(_appsettings["stockportgov:GoogleAnalyticsApiUrl"]);
    }

    public AppSetting GetAddThisShareId(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:AddThis"]);
    }

    public AppSetting GetRssEmail(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:RssEmail"]);
    }

    public Uri GetContentApiUri()
    {
        return new Uri(_appsettings["ContentApiUrl"]);
    }

    public Uri GetContentApiUrlRoot()
    {
        return new Uri(_appsettings["ContentApiUrlRoot"]);
    }

    public Uri GetStockportApiUri()
    {
        return new Uri(_appsettings["StockportApiUrl"]);
    }

    public AppSetting GetEmailHost(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:Host"]);
    }

    public AppSetting GetEmailRegion(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:Region"]);
    }

    public AppSetting GetEmailEmailFrom(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Email:EmailFrom"]);
    }

    public AppSetting GetGroupSubmissionEmail(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GroupSubmissionEmail"]);
    }

    public AppSetting GetGroupArchiveEmail(string businessId)
    {
        return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GroupArchiveEmail"]);
    }

    public AppSetting GetReCaptchaKey()
    {
        return AppSetting.GetAppSetting(_appsettings[$"ReCaptcha:SiteKey"]);
    }

    public List<ArchiveEmailPeriod> GetArchiveEmailPeriods()
    {
        var emailPeriods = new List<ArchiveEmailPeriod>();
        _appsettings.GetSection("stockportgov:GroupArchiveEmailPeriods").Bind(emailPeriods);
        return emailPeriods;
    }

    public StylesheetsConfiguration GetStylesheetConfig()
    {
        var stylesheetConfig = new StylesheetsConfiguration();
        _appsettings.GetSection("stockportgov:StylesheetsConfiguration").Bind(stylesheetConfig);
        return stylesheetConfig;
    }

    public AnalyticsConfigurationModel GetAnalyticsConfig()
    {
        var config = new AnalyticsConfigurationModel();
        _appsettings.GetSection("stockportgov:Analytics").Bind(config);
        return config;
    }

    public int GetFooterCache(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:FooterCache"], out int output);
        return output;
    }

    public string GetGroupManageContactUrl()
    {
        return _appsettings["GroupManageContactUrl"];
    }

    public string GetMyAccountUrl()
    {
        return _appsettings["myAccountUrl"];
    }

    public string GetStaleGroupsSecret()
    {
        return _appsettings["staleGroupsSecret"];
    }

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
    public int GetGroupsDefaultPageSize(string businessId)
    {
        int.TryParse(_appsettings[$"{businessId}:GroupsDefaultPageSize"], out int result);
        return result;
    }
    public string GetContentApiAuthenticationKey()
    {
        return _appsettings["ContentApiAuthenticationKey"];
    }
    public string GetWebAppClientId()
    {
        return _appsettings["WebAppClientId"];
    }

    public string GetDigitalStockportLink()
    {
        return _appsettings["stockportgov:DigitalStockportLink"];
    }
}