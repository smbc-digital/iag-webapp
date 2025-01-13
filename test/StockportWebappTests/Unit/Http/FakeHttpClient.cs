namespace StockportWebappTests_Unit.Unit.Http;

public class FakeHttpClient : IHttpClient
{
    private string _url;
    private readonly Dictionary<string, HttpResponse> _responses = new();
    private readonly Dictionary<string, HttpResponseMessage> _postAsyncresponses = new();
    private Exception _exception;
    public string invokedUrl;

    public FakeHttpClient For(string url)
    {
        _url = url;
        return this;
    }

    public void Return(HttpResponse response)
    {
        if (!_responses.ContainsKey(_url))
            _responses.Add(_url, response);
    }

    public void Throw(Exception exception) =>
        _exception = exception;

    public Task<HttpResponse> Get(string url, Dictionary<string, string> headers)
    {
        invokedUrl = url;
        if (_exception is not null)
            throw _exception;

        try
        {
            return Task.FromResult(_responses[url]);
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine($"No response found for: {url}");
            throw new KeyNotFoundException($"No response found for: {url}");
        }
    }

    public Task<HttpResponseMessage> PostRecaptchaAsync(string requestURI, HttpContent content)
    {
        invokedUrl = requestURI;
        if (_exception is not null)
            throw _exception;

        try
        {
            return Task.FromResult(_postAsyncresponses[requestURI]);
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine($"No response found for: {requestURI}");
            throw new KeyNotFoundException($"No response found for: {requestURI}");
        }
    }

    public Task<HttpResponse> PostAsync(string requestURI, HttpContent content, Dictionary<string, string> headers) =>
        throw new NotImplementedException();

    public Task<HttpResponse> PutAsync(string requestURI, HttpContent content, Dictionary<string, string> headers) =>
        throw new NotImplementedException();

    public Task<HttpResponse> DeleteAsync(string requestURI, Dictionary<string, string> headers) =>
        throw new NotImplementedException();

    public Task<HttpResponseMessage> PostAsyncMessage(string requestURI, HttpContent content, Dictionary<string, string> headers) =>
        throw new NotImplementedException();

    public Task PostMessage(string requestURI, HttpContent content, Dictionary<string, string> headers) =>
        throw new NotImplementedException();
}