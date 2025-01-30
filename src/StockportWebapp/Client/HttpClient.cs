namespace StockportWebapp.Client;

public interface IHttpClient
{
    Task<HttpResponse> Get(string url, Dictionary<string, string> headers);
    Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content);
    Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers);
    Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers);
    Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers);
    Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers);
    Task PostMessage(string requestURI, HttpContent content, Dictionary<string, string> headers);
}

[ExcludeFromCodeCoverage]
public class HttpClient(System.Net.Http.HttpClient client) : IHttpClient
{
    private readonly System.Net.Http.HttpClient _client = client;

    public async Task<HttpResponse> Get(string url, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });

        try
        {
            HttpResponseMessage task = await _client.GetAsync(url);

            string content = await task.Content.ReadAsStringAsync();

            return new HttpResponse((int)task.StatusCode,
                content,
                task.ReasonPhrase);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content) =>
        _client.PostAsync(requestURI, content);

    public async Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });

        HttpResponseMessage task = await _client.PostAsync(requestURI, content);

        return new HttpResponse((int)task.StatusCode,
                                content,
                                task.ReasonPhrase);
    }

    public async Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });

        return await _client.PostAsync(requestURI, content);
    }

    public async Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });

        HttpResponseMessage task = await _client.PutAsync(requestURI, content);

        return new HttpResponse((int)task.StatusCode,
                                content,
                                task.ReasonPhrase);
    }

    public async Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });

        HttpResponseMessage task = await _client.DeleteAsync(requestURI);

        return new HttpResponse((int)task.StatusCode,
                                null,
                                task.ReasonPhrase);
    }

    public Task PostMessage(string requestURI, HttpContent content, Dictionary<string, string> headers)
    {
        headers.ToList().ForEach(header =>
        {
            _client.DefaultRequestHeaders.Remove(header.Key);
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        });
        
        return _client.PostAsync(requestURI, content);
    }
}