using System.Net;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IContentApiRepository : IBaseRepository
    {
        Task<T> GetResponse<T>();
        Task<T> GetResponse<T>(string extra);
        Task<T> GetResponse<T>(List<Query> queries);
        Task<T> GetResponse<T>(string extra, List<Query> queries);
        Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent);
        Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra);
    }

    // TODO: Test this
    public class ContentApiRepository : BaseRepository, IContentApiRepository
    {
        private readonly IUrlGeneratorSimple _urlGeneratorSimple;

        public ContentApiRepository(IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple, ILogger<BaseRepository> logger) : base(httpClient, config, logger)
        {
            _urlGeneratorSimple = urlGeneratorSimple;
        }

        #region GET Methods
        public async Task<T> GetResponse<T>()
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().TrimEnd('/');

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(string extra)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra);

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(List<Query> queries)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddQueryStrings(queries);

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(string extra, List<Query> queries)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra).AddQueryStrings(queries);

            return await GetResponseAsync<T>(url);
        }

        #endregion

        #region PUT Methods

        public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>();
            return await PutResponseAsync<T>(url, httpContent);
        }


        public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra);
            return await PutResponseAsync<T>(url, httpContent);
        }


        #endregion
    }
}
