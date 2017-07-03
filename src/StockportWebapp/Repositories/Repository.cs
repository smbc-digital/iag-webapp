using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public class Repository : IRepository
    {
        private readonly UrlGenerator _urlGenerator;
        private readonly IHttpClient _httpClient;

        public Repository(UrlGenerator urlGenerator, IHttpClient httpClient)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
        }

        public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
        {
            var url = _urlGenerator.UrlFor<T>(slug, queries);
            var httpResponse = await _httpClient.Get(url);
            return HttpResponse.Build<T>(httpResponse);
        }

        public async Task<HttpResponse> Put<T>(HttpContent content, string slug = "")
        {
            var url = $"{_urlGenerator.UrlFor<T>(slug)}";
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponse> Delete<T>(string slug = "")
        {
            var url = $"{_urlGenerator.UrlFor<T>(slug)}";
            return await _httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponse> Archive<T>(HttpContent content, string slug = "")
        {
            var url = $"{_urlGenerator.UrlFor<T>(slug)}";
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponse> Publish<T>(HttpContent content, string slug = "")
        {
            var url = $"{_urlGenerator.UrlFor<T>(slug)}";
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponse> GetLatest<T>(int limit)
        {
            var url = _urlGenerator.UrlForLimit<T>(limit);
            var httpResponse = await _httpClient.Get(url);
            return HttpResponse.Build<T>(httpResponse);
        }

        public async Task<HttpResponse> RemoveAdministrator(string slug, string email)
        {
            var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
            return await _httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponse> UpdateAdministrator(HttpContent permission, string slug, string email)
        {
            var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
            return await _httpClient.PutAsync(url, permission);
        }

        public async Task<HttpResponse> AddAdministrator(StringContent permission, string slug, string email)
        {
            var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
            return await _httpClient.PostAsync(url, permission);
        }

        public async Task<HttpResponse> GetLatestOrderByFeatured<T>(int limit)
        {
            var url = _urlGenerator.UrlForLimitAndFeatured<T>(limit, true);
            var httpResponse = await _httpClient.Get(url);
            return HttpResponse.Build<T>(httpResponse);
        }

        public async Task<HttpResponse> GetRedirects()
        {
            var url = _urlGenerator.RedirectUrl();
            var httpResponse = await _httpClient.Get(url);
            return HttpResponse.Build<Redirects>(httpResponse);
        }

        public async Task<HttpResponse> GetAdministratorsGroups(string email)
        {
            var url = _urlGenerator.AdministratorsGroups(email);
            var httpResponse = await _httpClient.Get(url);
            return HttpResponse.Build<List<Group>>(httpResponse);
        }
    }
}