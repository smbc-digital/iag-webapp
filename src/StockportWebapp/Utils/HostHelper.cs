namespace StockportWebapp.Utils;

public class HostHelper(CurrentEnvironment currentEnvironment)
{
    readonly CurrentEnvironment _currentEnvironment = currentEnvironment;

    public string GetHost(HttpRequest request)
    {
        string scheme = _currentEnvironment.Name.Equals("local")
            ? "http"
            : "https";

        return string.Concat(scheme, "://", request?.Host);
    }

    public string GetHostAndQueryString(HttpRequest request)
    {
        string scheme = _currentEnvironment.Name.Equals("local")
            ? "http"
            : "https";
        
        StringBuilder builder = new();

        builder.Append(scheme);
        builder.Append("://");
        builder.Append(request?.Host);
        builder.Append(request.QueryString);

        return builder.ToString();
    }
}