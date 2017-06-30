using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;

namespace StockportWebapp.Repositories
{
    public interface IRepository
    {
        Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null);
        Task<HttpResponse> GetLatest<T>(int limit);
        Task<HttpResponse> GetLatestOrderByFeatured<T>(int limit);
        Task<HttpResponse> GetRedirects();
        Task<HttpResponse> GetAdministratorsGroups(string email);
        Task<HttpResponse> Put<T>(HttpContent content, string slug = "");
        Task<HttpResponse> RemoveAdministrator(string slug, string email);
        Task<HttpResponse> UpdateAdministrator(HttpContent permission, string slug, string email);
        Task<HttpResponse> AddAdministrator(StringContent permission, string modelSlug, string email);
        Task<HttpResponse> Delete<T>(string slug = "");
        Task<HttpResponse> Archive<T>(HttpContent content, string slug = "");
        Task<HttpResponse> Publish<T>(HttpContent content, string slug = "");
    }

    public interface IRepository<T>
    {
        Task<IRepositoryResponse<T>> Get(string slug = "");
    }

    public interface IRepositoryResponse<T>
    {
        bool IsError();
    }

    public class Success<T> : IRepositoryResponse<T>
    {
        public readonly T Content;

        public Success(T content)
        {
            Content = content;
        }

        public bool IsError()
        {
            return false;
        }
    }

    public class Error<T> : StatusCodeResult, IRepositoryResponse<T>
    {
        public bool IsError()
        {
            return true;
        }

        public Error(int statusCode) : base(statusCode){}
    }
}