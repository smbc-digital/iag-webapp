using System.Text;

namespace StockportWebapp.Utils
{
    public class ContentSecurityPolicyBuilder
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        public string BuildPolicy()
        {
            BuildDefaultSource();
            BuildChildSource();
            BuildFontSource();
            BuildImageSource();
            BuildStyleSource();
            BuildScriptSource();
            BuildConnectSource();
            BuildMediaSource();
            BuildObjectSource();

            return _stringBuilder.ToString();
        }

        private void BuildDefaultSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("default-src", containsSelf: false)
                .AddSource("https:")
                .Finish());
        }

        private void BuildChildSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("child-src")
                .AddSource("http://s7.addthis.com")
                .Finish());
        }

        private void BuildFontSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("font-src")
                .AddSource("font.googleapis.com")
                .AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
                .AddSource("fonts.gstatic.com/")
                .Finish());
        }

        private void BuildImageSource()
        {
            /*khms0.googleapis.com khms1.googleapis.com geo0.ggpht.com geo1.ggpht.com geo2.ggpht.com geo3.ggpht.com  cbks0.googleapis.com csi.gstatic.com " +
                                 "maps.gstatic.com maps.googleapis.com images.contentful.com/ www.google-analytics.com/r/collect www.google-analytics.com/collect stats.g.doubleclick.net/r/collect " +
                                 "https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/" +
                                 "s3-eu-west-1.amazonaws.com/ share.typeform.com/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "customer.cludo.com/img/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/; " +*/
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("img-src")
                .AddSource("khms0.googleapis.com")
                .AddSource("khms1.googleapis.com")
                .AddSource("geo0.ggpht.com")
                .AddSource("geo1.ggpht.com")
                .AddSource("geo2.ggpht.com")
                .AddSource("geo3.ggpht.com")
                .AddSource("cbks0.googleapis.com")
                .AddSource("csi.gstatic.com")
                .AddSource("maps.gstatic.com")
                .AddSource("maps.googleapis.com")
                .AddSource("images.contentful.com/")
                .AddSource("www.google-analytics.com/r/collect")
                .AddSource("www.google-analytics.com/collect")
                .AddSource("stats.g.doubleclick.net/r/collect")
                .AddSource("https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/")
                .AddSource("s3-eu-west-1.amazonaws.com/")
                .AddSource("share.typeform.com/")
                .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
                .AddSource("customer.cludo.com/img/")
                .AddSource("uk1.siteimprove.com/")
                .AddSource("stockportb.logo-net.co.uk/")
                .AddSource("*.cloudfront.net/butotv/")
                .Finish());
        }

        private void BuildStyleSource()
        {
            /*'unsafe-inline' *.cludo.com/css/ s3-eu-west-1.amazonaws.com/ share.typeform.com/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ " +
                                 "cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ *.cloudfront.net/butotv/; " +*/
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("style-src")
                .AddSource("'unsafe-inline'")
                .AddSource("*.cludo.com/css/")
                .AddSource("s3-eu-west-1.amazonaws.com/")
                .AddSource("share.typeform.com/")
                .AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
                .AddSource("fonts.googleapis.com/")
                .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
                .AddSource("*.cloudfront.net/butotv/")
                .Finish());
        }

        private void BuildScriptSource()
        {
            /*'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ maps.googleapis.com m.addthisedge.com/live/boost/ " +
                                 "www.google-analytics.com/analytics.js api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "s3-eu-west-1.amazonaws.com/ share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ " +
                                 "siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/; " +*/
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("script-src")
                .AddSource("'unsafe-inline'")
                .AddSource("'unsafe-eval'")
                .AddSource("https://ajax.googleapis.com/ajax/libs/jquery/")
                .AddSource("maps.googleapis.com m.addthisedge.com/live/boost/")
                .AddSource("www.google-analytics.com/analytics.js")
                .AddSource("api.cludo.com/scripts/")
                .AddSource("customer.cludo.com/scripts/")
                .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
                .AddSource("s3-eu-west-1.amazonaws.com/")
                .AddSource("share.typeform.com/")
                .AddSource("js.buto.tv/video/")
                .AddSource("s7.addthis.com/js/300/addthis_widget.js")
                .AddSource("m.addthis.com/live/")
                .AddSource("siteimproveanalytics.com/js/")
                .AddSource("*.logo-net.co.uk/Delivery/")
                .Finish());
        }

        private void BuildConnectSource()
        {
            // https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/; " +
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("connect-src")
                .AddSource("https://api.cludo.com/")
                .AddSource("buto-ping-middleman.buto.tv/")
                .AddSource("m.addthis.com/live/")
                .Finish());
        }

        private void BuildMediaSource()
        {
            // https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/; " +
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("media-src")
                .AddSource("https://www.youtube.com/")
                .AddSource("*.cloudfront.net/butotv/live/videos/")
                .Finish());
        }

        private void BuildObjectSource()
        {
            // https://www.youtube.com http://www.youtube.com
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("object-src")
                .AddSource("https://www.youtube.com")
                .AddSource("http://www.youtube.com")
                .Finish());
        }
    }
}