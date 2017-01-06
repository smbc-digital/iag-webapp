using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{   
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
                {"default-src", "http:"},
                {"font-src", "'self' font.googleapis.com maxcdn.bootstrapcdn.com/font-awesome/ fonts.gstatic.com/"},
                {"img-src", "'self' images.contentful.com/ s3-eu-west-1.amazonaws.com/live-iag-static-assets/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/"},
                {"style-src", "'self' 'unsafe-inline' *.cludo.com/css/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ *.cloudfront.net/butotv/"},
                {"script-src", "'self' 'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ s3-eu-west-1.amazonaws.com/share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/"},
                {"connect-src", "'self' https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/" },
                {"media-src", "'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/" },
                {"object-src", "'self'  https://www.youtube.com http://www.youtube.com" }
            };

            var headerValue = string.Join("; ", cspHeaders.Select(x => $"{x.Key} {x.Value}"));

            return headerValue;
        }
    }
}
