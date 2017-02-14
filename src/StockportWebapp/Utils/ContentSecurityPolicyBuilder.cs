using System.Text;

namespace StockportWebapp.Utils
{
    public class ContentSecurityPolicyBuilder
    {
        private string BuildDefaultSource()
        {
            return new ContentSecurityPolicyElement("default-src")
                .AddSource("https:")
                .Finish();
        }

        private string BuildChildSource()
        {
            return new ContentSecurityPolicyElement("child-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildFontSource()
        {
            return new ContentSecurityPolicyElement("font-src")
                .AddSource("font.googleapis.com")
                .AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
                .AddSource("fonts.gstatic.com/")
                .Finish();
        }

        private string BuildImageSource()
        {
            /*khms0.googleapis.com khms1.googleapis.com geo0.ggpht.com geo1.ggpht.com geo2.ggpht.com geo3.ggpht.com  cbks0.googleapis.com csi.gstatic.com " +
                                 "maps.gstatic.com maps.googleapis.com images.contentful.com/ www.google-analytics.com/r/collect www.google-analytics.com/collect stats.g.doubleclick.net/r/collect " +
                                 "https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/" +
                                 "s3-eu-west-1.amazonaws.com/share.typeform.com/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "customer.cludo.com/img/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/; " +*/
            return new ContentSecurityPolicyElement("img-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildStyleSource()
        {
            /*'unsafe-inline' *.cludo.com/css/ s3-eu-west-1.amazonaws.com/share.typeform.com/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ " +
                                 "cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ *.cloudfront.net/butotv/; " +*/
            return new ContentSecurityPolicyElement("style-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildScriptSource()
        {
            /*'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ maps.googleapis.com m.addthisedge.com/live/boost/ " +
                                 "www.google-analytics.com/analytics.js api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "s3-eu-west-1.amazonaws.com/share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ " +
                                 "siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/; " +*/
            return new ContentSecurityPolicyElement("script-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildConnectSource()
        {
            // https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/; " +
            return new ContentSecurityPolicyElement("connect-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildMediaSource()
        {
            // https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/; " +
            return new ContentSecurityPolicyElement("media-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        private string BuildObjectSource()
        {
            // https://www.youtube.com http://www.youtube.com
            return new ContentSecurityPolicyElement("object-src")
                .AddSource("http://s7.addthis.com")
                .Finish();
        }

        public string BuildPolicy()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(BuildDefaultSource());
            stringBuilder.Append(BuildChildSource());
            stringBuilder.Append(BuildFontSource());
            stringBuilder.Append(BuildImageSource());
            stringBuilder.Append(BuildStyleSource());
            stringBuilder.Append(BuildScriptSource());
            stringBuilder.Append(BuildConnectSource());
            stringBuilder.Append(BuildMediaSource());
            stringBuilder.Append(BuildObjectSource());

            //return stringBuilder.ToString();

            string childSource = BuildChildSource();

            return "default-src https:; " +
                   childSource +
                   "font-src 'self' font.googleapis.com maxcdn.bootstrapcdn.com/font-awesome/ fonts.gstatic.com/; " +
                   "img-src 'self'  khms0.googleapis.com khms1.googleapis.com geo0.ggpht.com geo1.ggpht.com geo2.ggpht.com geo3.ggpht.com  cbks0.googleapis.com csi.gstatic.com " +
                   "maps.gstatic.com maps.googleapis.com images.contentful.com/ www.google-analytics.com/r/collect www.google-analytics.com/collect stats.g.doubleclick.net/r/collect " +
                   "https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/" +
                   "s3-eu-west-1.amazonaws.com/share.typeform.com/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                   "customer.cludo.com/img/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/; " +
                   "style-src 'self' 'unsafe-inline' *.cludo.com/css/ s3-eu-west-1.amazonaws.com/share.typeform.com/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ " +
                   "cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ *.cloudfront.net/butotv/; " +
                   "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ maps.googleapis.com m.addthisedge.com/live/boost/ " +
                   "www.google-analytics.com/analytics.js api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                   "s3-eu-west-1.amazonaws.com/share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ " +
                   "siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/; " +
                   "connect-src 'self' https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/; " +
                   "media-src 'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/; " +
                   "object-src 'self' https://www.youtube.com http://www.youtube.com; "; 
        }
    }
}