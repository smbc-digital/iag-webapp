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
        Uri GetContentApiUrlRoot();
        AppSetting GetEmailHost(string businessId);
        AppSetting GetEmailRegion(string businessId);
        AppSetting GetEmailEmailFrom(string businessId);
        AppSetting GetEmailAlertsNewSubscriberUrl(string businessId);
        AppSetting GetEventSubmissionEmail(string businessId);
        AppSetting GetGroupSubmissionEmail(string businessId);
        AppSetting GetGroupArchiveEmail(string businessId);
        AppSetting GetParisPamentLink(string businessId);
        AppSetting GetReCaptchaKey();
        int GetFooterCache(string businessId);
        bool GetUseRedisSessions();
        bool SendAmazonEmails();
        string GetMyAccountUrl();
        string GetStaticAssetsRootUrl();
        string GetExportHost();
        int GetNewsDefaultPageSize(string businessId);
        int GetEventsDefaultPageSize(string businessId);
        int GetConsultationsDefaultPageSize(string businessId);
        int GetGroupsDefaultPageSize(string businessId);
        string GetContentApiAuthenticationKey();
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

        public string GetExportHost()
        {
            return _appsettings["ExportHost"];
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

        public Uri GetContentApiUrlRoot()
        {
            return new Uri(_appsettings["ContentApiUrlRoot"]);
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

        public AppSetting GetGroupArchiveEmail(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:GroupArchiveEmail"]);
        }

        public AppSetting GetParisPamentLink(string businessId)
        {
            return AppSetting.GetAppSetting(_appsettings[$"{businessId}:ParisPayment"]);
        }

        public AppSetting GetReCaptchaKey()
        {
            return AppSetting.GetAppSetting(_appsettings[$"ReCaptcha:SiteKey"]);
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

        public bool SendAmazonEmails()
        {
            bool output;
            bool.TryParse(_appsettings["SendAmazonEmails"], out output);
            return output;
        }

        public string GetMyAccountUrl()
        {
            return _appsettings["myAccountUrl"];
        }

        public int GetNewsDefaultPageSize(string businessId)
        {
            int result;
            int.TryParse(_appsettings[$"{businessId}:NewsDefaultPageSize"], out result);
            return result;
        }
        public int GetEventsDefaultPageSize(string businessId)
        {
            int result;
            int.TryParse(_appsettings[$"{businessId}:EventsDefaultPageSize"], out result);
            return result;
        }
        public int GetConsultationsDefaultPageSize(string businessId)
        {
            int result;
            int.TryParse(_appsettings[$"{businessId}:ConsultationDefaultPageSize"], out result);
            return result;
        }
        public int GetGroupsDefaultPageSize(string businessId)
        {
            int result;
            int.TryParse(_appsettings[$"{businessId}:GroupsDefaultPageSize"], out result);
            return result;
        }
        public string GetContentApiAuthenticationKey()
        {
            return _appsettings["ContentApiAuthenticationKey"];
        }
    }
}