using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockportWebapp.Http;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public class SimpleRepository<T> : IRepository<T>
    {
        private readonly IStubToUrlConverter _urlGenerator;
        private readonly IHttpClient _httpClient;

        public SimpleRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
        }

        public async Task<IRepositoryResponse<T>> Get(string slug = "")
        {
            var url = _urlGenerator.UrlFor<T>(slug);
            var httpResponse = await _httpClient.Get(url);
            if (!httpResponse.IsSuccessful())
                return new Error<T>(httpResponse.StatusCode);

            var content = JsonConvert.DeserializeObject<T>(httpResponse.Content as string);
            return new Success<T>(content);
        }
    }
}