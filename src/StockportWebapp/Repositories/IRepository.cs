using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;

namespace StockportWebapp.Repositories
{
    public interface IRepository
    {
        Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null);
        Task<HttpResponse> GetRedirects();
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