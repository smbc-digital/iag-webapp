using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SecurityHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FeatureToggles _featureToggles;

        public SecurityHeaderMiddleware(RequestDelegate next, FeatureToggles featureToggles)
        {
            _next = next;
            _featureToggles = featureToggles;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var host = httpContext.Request.Host.Value.ToLower();
            var isRemoteHost = host.StartsWith("www") || host.StartsWith("int-") || host.StartsWith("qa-") || host.StartsWith("stage-");

            if (_featureToggles.SecurityHeaders)
            {
                httpContext.Response.Headers.Add("Content-Security-Policy", new[] { GetContentSecurityHeaderValue() });
            }

            if (_featureToggles.SecurityHeaders && isRemoteHost)
            {
                httpContext.Response.Headers.Add("Strict-Transport-Security", new[] { "max-age=31536000" });
            }

            return _next(httpContext);
        }

        private static string GetContentSecurityHeaderValue()
        {
            var cspHeaders = new Dictionary<string, string>()
            {
                {"default-src", "https:"},
                {"font-src", "'self' https://font.googleapis.com"},
                {"img-src", "'self' https://images.contentful.com https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/"},
                {"style-src", "'self' 'unsafe-inline' https://customer.cludo.com/css/112/1144/ https://maxcdn.bootstrapcdn.com/font-awesome/"},
                {"script-src", "'self' 'unsafe-inline' https://ajax.googleapis.com/ajax/libs/jquery/ https://api.cludo.com/scripts/ https://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ https://s3-eu-west-1.amazonaws.com/share.typeform.com/ https://js.buto.tv/video/ https://s7.addthis.com/js/300/addthis_widget.js"},
                {"form-action", "'self'" },
                {"media-src", "'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/" }
            };

            var headerValue = string.Join("; ", cspHeaders.Select(x => $"{x.Key} {x.Value}"));

            return headerValue;
        }
    }
}
