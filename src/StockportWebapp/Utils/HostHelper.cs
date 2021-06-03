using System.Text;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Config;

namespace StockportWebapp.Utils
{
    public class HostHelper
    {
        readonly CurrentEnvironment _currentEnvironment;

        public HostHelper(CurrentEnvironment currentEnvironment)
        {
            _currentEnvironment = currentEnvironment;
        }

        public string GetHost(HttpRequest request)
        {
            var scheme = _currentEnvironment.Name == "local" ? "http" : "https";
            return string.Concat(scheme, "://", request?.Host);
        }

        public string GetHostAndQueryString(HttpRequest request)
        {
            var scheme = _currentEnvironment.Name == "local" ? "http" : "https";
            var builder = new StringBuilder();
            builder.Append(scheme);
            builder.Append("://");
            builder.Append(request?.Host);
            builder.Append(request.QueryString);
            return builder.ToString();
        }
    }
}
