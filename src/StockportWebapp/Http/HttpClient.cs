using System.Net.Http;
using System.Threading.Tasks;

namespace StockportWebapp.Http
{
    public interface IHttpClient
    {
        Task<HttpResponse> Get(string url);
        Task<HttpResponseMessage> PostAsync(string requestURI, HttpContent content);
    }

    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient(System.Net.Http.HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponse> Get(string url)
        {
            var task = await _client.GetAsync(url);

            var content = await task.Content.ReadAsStringAsync();
            
            return new HttpResponse((int)task.StatusCode,
                                    content,
                                    task.ReasonPhrase);
        }

        public Task<HttpResponseMessage> PostAsync(string requestURI, HttpContent content)
        {
            return _client.PostAsync(requestURI, content);
        }
    }
}