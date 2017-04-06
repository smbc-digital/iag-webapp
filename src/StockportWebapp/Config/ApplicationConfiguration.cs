using System;
using Microsoft.Extensions.Configuration;

namespace StockportWebapp.Config
{
    public interface IApplicationConfiguration
    {
        AppSetting GetEmailAlertsUrl(string businessId);
        AppSetting GetSearchUrl(string businessId);
        AppSetting GetPostcodeSearchUrl(string businessId);
        AppSetting GetGoogleAnalyticsCode(string businessId);
        AppSetting GetAddThisShareId(string businessId);
        AppSetting GetRssEmail(string businessId);
        Uri GetContentApiUri();
        AppSetting GetEmailHost(string businessId);
        AppSetting GetEmailRegion(string businessId);
        AppSetting GetEmailEmailFrom(string businessId);
        AppSetting GetEmailAlertsNewSubscriberUrl(string businessId);
        AppSetting GetEventSubmissionEmail(string businessId);
        AppSetting GetGroupSubmissionEmail(string businessId);
        AppSetting GetParisPamentLink(string businessId);
        int GetFooterCache(string businessId);
        bool GetUseRedisSessions();

        string GetStaticAssetsRootUrl();
    }

    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly IConfigurationRoot _appsettings;

        public ApplicationConfiguration(IConfigurationRoot appsettings)
        {
            _appsettings = appsettings;

            Ensure.ArgumentIsAValidUri(_appsettings["ContentApiUrl"], "ContentApiUrl");
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

        public AppSetting GetSearchUrl(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Search"]);
        }

        public AppSetting GetPostcodeSearchUrl(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:Postcode"]);
        }

        public AppSetting GetGoogleAnalyticsCode(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GoogleAnalytics"]);
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

        public AppSetting GetEventSubmissionEmail(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:EventSubmissionEmail"]);
        }

        public AppSetting GetGroupSubmissionEmail(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GroupSubmissionEmail"]);
        }

        public AppSetting GetParisPamentLink(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:ParisPayment"]);
        }

        public int GetFooterCache(string businessId)
        {
            int output;
            int.TryParse(_appsettings[$"{businessId}:FooterCache"], out output);
            return output;
        }

        public bool GetUseRedisSessions()
        {
            bool output;
            bool.TryParse(_appsettings["UseRedisSessions"], out output);
            return output;
        }
    }
}