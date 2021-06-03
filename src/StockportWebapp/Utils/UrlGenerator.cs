using System;
using System.Collections.Generic;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories.Responses;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Utils
{
    public interface IStubToUrlConverter
    {
        string UrlFor<T>(string slug = "", List<Query> queries = null);
        string HealthcheckUrl();
    }

    public class UrlGenerator : IStubToUrlConverter
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        private readonly Dictionary<Type, string> _urls = new Dictionary<Type, string>()
        {
            {typeof(Topic), "topics/"},
            {typeof(List<TopicSitemap>), "topics/"},
            {typeof(Article), "articles/"},
            {typeof(Profile), "profiles/"},
            {typeof(ProfileResponse), "profiles/"},
            {typeof(List<Profile>), "profiles/"},
            {typeof(StartPage), "start-page/"},
            {typeof(List<StartPage>), "start-page/"},
            {typeof(Homepage), "homepage"},
            {typeof(News), "news/"},
            {typeof(Newsroom), "news"},
            {typeof(List<News>), "news"},
            {typeof(List<AtoZ>), "atoz/"},
            {typeof(Footer), "footer"},
            {typeof(Event), "events/"},
            {typeof(EventCalendar), "events"},
            {typeof(List<Event>), "events"},
            {typeof(EventHomepage), "eventhomepage"},
            {typeof(GroupHomepage), "grouphomepage"},
            {typeof(EventResponse), "events"},
            {typeof(Group), "groups/"},
            {typeof(List<Group>), "groups/"},
            {typeof(Payment), "payments/"},
            {typeof(List<Payment>), "payments/"},
            {typeof(ServicePayPayment), "service-pay-payments/"},
            {typeof(Section), "sections/"},
            {typeof(List<Section>), "sections/"},
            {typeof(Showcase), "showcases/"},
            {typeof(List<Showcase>), "showcases/"},
            {typeof(List<GroupCategory>), "group-categories/"},
            {typeof(List<EventCategory>), "event-categories/"},
            {typeof(GroupResults), "group-results/"},
            {typeof(ContactUsId), "contact-us-id/"},
            {typeof(List<ArticleSiteMap>), "articleSiteMap"},
            {typeof(List<SectionSiteMap>), "sectionSiteMap"},
            {typeof(SmartAnswers), "smart/"},
            {typeof(Organisation), "organisations/"},
            {typeof(GroupAdvisor), "groups/advisors/"},
            {typeof(Document), "documents/"},
            {typeof(SmartResult), "smart-result/"},
            {typeof(PrivacyNotice), "privacy-notices/"},
            {typeof(List<PrivacyNotice>), "privacy-notices/"},
            {typeof(ContactUsArea), "contactusarea"},
            {typeof(CommsHomepage), "comms"},
            {typeof(DocumentPage), "document-page/"}
        };

        public UrlGenerator(IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
        }

        public string UrlFor<T>(string slug = "", List<Query> queries = null)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], slug,
                CreateQueryString(queries));
        }

        public string UrlForLimit<T>(int limit)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/",
                limit.ToString());
        }

        public string UrlForLimitAndFeatured<T>(int limit, bool featured)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/",
                limit.ToString(), $"?featured={featured.ToString().ToLower()}");
        }

        private static string CreateQueryString(List<Query> queries)
        {
            if (queries == null || queries.Count < 1) return "";
            return "?" + string.Join("&", queries);
        }

        public string RedirectUrl()
        {
            return string.Concat(_config.GetContentApiUri(), "redirects");
        }

        public string AdministratorsGroups(string email)
        {
            return $"{_config.GetContentApiUri()}{_businessId}/groups/administrators/{email}";
        }

        public string ArticlesForSiteMap(string slug = "", List<Query> queries = null)
        {
            //return $"{_config.GetContentApiUri()}{_businessId}/articleSiteMap";
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", "articleSiteMap", slug, CreateQueryString(queries));
        }

        public string HealthcheckUrl()
        {
            var baseUrl = _config.GetContentApiUrlRoot();
            return new UriBuilder
            {
                Host = baseUrl.Host,
                Port = baseUrl.Port,
                Scheme = baseUrl.Scheme,
                Path = baseUrl.LocalPath + "_healthcheck"
            }.Uri.ToString();
        }
    }
}