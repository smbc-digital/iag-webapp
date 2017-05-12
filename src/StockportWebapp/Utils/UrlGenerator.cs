using System;
using System.Collections.Generic;
using StockportWebapp.Config;
using StockportWebapp.Models;
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
            {typeof(Topic), "topic/"},
            {typeof(Article), "article/"},
            {typeof(Profile), "profile/"},
            {typeof(StartPage), "start-page/"},
            {typeof(Homepage), "homepage"},
            {typeof(News), "news/"},
            {typeof(Newsroom), "news"},
            {typeof(List<News>), "news/latest/"},
            {typeof(List<AtoZ>), "atoz/"},
            {typeof(Footer), "footer"},
            {typeof(Event), "events/"},
            {typeof(EventCalendar), "events"},
            {typeof(EventResponse), "events"},
            {typeof(Group), "group/"},
            {typeof(Payment), "payment/"},
            {typeof(Showcase), "showcase/"},
            {typeof(List<GroupCategory>), "groupCategory/"},
            {typeof(GroupResults), "groupResults/"}
        };

        public UrlGenerator(IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
        }

        public string UrlFor<T>(string slug = "", List<Query> queries = null)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], slug, CreateQueryString(queries));
        }

        public string UrlForLimit<T>(int limit)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/", limit.ToString());
        }

        public string UrlForLimitAndFeatured<T>(int limit, bool featured)
        {
            return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/", limit.ToString(), $"?featured={featured.ToString().ToLower()}");
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

        public string HealthcheckUrl()
        {
            var baseUrl = _config.GetContentApiUrlRoot();
            return  new UriBuilder
            {
                Host   = baseUrl.Host,
                Port   = baseUrl.Port,
                Scheme = baseUrl.Scheme,
                Path   = baseUrl.LocalPath + "_healthcheck"
            }.Uri.ToString();
        }
    }
}