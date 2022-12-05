using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StockportWebapp.Http
{
    public class HttpResponse : StatusCodeResult
    {
        public readonly object Content;
        public readonly string Error;

        public HttpResponse(int statusCode, object content, string error) : base(statusCode)
        {
            Content = content;
            Error = error;
        }

        public static HttpResponse Successful(int statusCode, object content)
        {
            return new HttpResponse(statusCode, content, string.Empty);
        }

        public static HttpResponse Failure(int statusCode, string error)
        {
            return new HttpResponse(statusCode, string.Empty, error);
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static HttpResponse Build<T>(HttpResponse httpResponse)
        {
            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var content = JsonConvert.DeserializeObject<T>(httpResponse.Content as String);
            return Successful(httpResponse.StatusCode, content);
        }
    }

    public static class StatusCodeResultExtensions
    {
        internal static bool IsSuccessful(this StatusCodeResult result)
        {
            return result.StatusCode == 200;
        }

        internal static bool IsNotFound(this StatusCodeResult result)
        {
            return result.StatusCode == 404;
        }

        internal static bool IsNotAuthorised(this StatusCodeResult result)
        {
            return result.StatusCode == 401;
        }
    }
}
