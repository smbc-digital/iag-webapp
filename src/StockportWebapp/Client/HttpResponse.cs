namespace StockportWebapp.Client;

[ExcludeFromCodeCoverage]
public class HttpResponse(int statusCode,
                        object content,
                        string error) : StatusCodeResult(statusCode)
{
    public readonly object Content = content;
    public readonly string Error = error;

    public static HttpResponse Successful(int statusCode, object content) =>
        new(statusCode, content, string.Empty);

    public static HttpResponse Failure(int statusCode, string error) =>
        new(statusCode, string.Empty, error);
    
    public override string ToString() =>
        JsonConvert.SerializeObject(this);

    public static HttpResponse Build<T>(HttpResponse httpResponse)
    {
        if (!httpResponse.IsSuccessful())
            return httpResponse;

        T content = JsonConvert.DeserializeObject<T>(httpResponse.Content as string);

        return Successful(httpResponse.StatusCode, content);
    }
}

[ExcludeFromCodeCoverage]
public static class StatusCodeResultExtensions
{
    internal static bool IsSuccessful(this StatusCodeResult result) =>
        result.StatusCode.Equals(200);

    internal static bool IsNotFound(this StatusCodeResult result) =>
        result.StatusCode.Equals(404);

    internal static bool IsNotAuthorised(this StatusCodeResult result) =>
        result.StatusCode.Equals(401);
}