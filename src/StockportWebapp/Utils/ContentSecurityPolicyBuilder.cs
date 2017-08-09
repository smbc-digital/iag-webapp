using System.Text;

namespace StockportWebapp.Utils
{
    public class ContentSecurityPolicyBuilder
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        public string BuildPolicy()
        {
            BuildDefaultSource();
            BuildChildSourceAkaFrameSource();
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

        private void BuildChildSourceAkaFrameSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("child-src")
                .AddSource("http://s7.addthis.com")
                .AddSource("https://www.youtube.com")
                .AddSource("https://stockportcouncil.typeform.com")
                .AddSource("https://www.google.com/maps/")
                .AddSource("http://www.opinionstage.com/polls/")
                .AddSource("https://www.google.com/recaptcha/api2/anchor")
                .AddSource("https://www.google.com/recaptcha/api2/bframe")
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
                .AddSource("static.tacdn.com")
                .Finish());
        }

        private void BuildImageSource()
        {
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
                .AddSource("http://maps.stockport.gov.uk/")
                .AddSource("*.ads.astuntechnology.com/")
                .AddSource("s3-eu-west-1.amazonaws.com/")
                .AddSource("share.typeform.com/")
                .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
                .AddSource("customer.cludo.com/img/")
                .AddSource("uk1.siteimprove.com/")
                .AddSource("stockportb.logo-net.co.uk/")
                .AddSource("*.cloudfront.net/butotv/")
                .AddSource("data:")
                .AddSource("www.tripadvisor.co.uk/")
                .Finish());
        }

        private void BuildStyleSource()
        {
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
                .AddSource("http://maps.stockport.gov.uk/")
                .AddSource("*.cloudfront.net/butotv/")
                .AddSource("*.tripadvisor.com")
                .AddSource("*.tripadvisor.co.uk")
                .AddSource("*.tripadvisor.co.uk")
                .AddSource("static.tacdn.com")
                .AddSource("data:")
                .Finish());
        }

        private void BuildScriptSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("script-src")
                .AddSource("'unsafe-inline'")
                .AddSource("'unsafe-eval'")
                .AddSource("https://ajax.googleapis.com/ajax/libs/jquery/")
                .AddSource("maps.googleapis.com")
                .AddSource("m.addthisedge.com/live/boost/")
                .AddSource("www.google-analytics.com/analytics.js")
                .AddSource("api.cludo.com/scripts/")
                .AddSource("customer.cludo.com/scripts/")
                .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
                .AddSource("s3-eu-west-1.amazonaws.com/")
                .AddSource("http://s7.addthis.com/")
                .AddSource("https://stockportcouncil.typeform.com/")
                .AddSource("http://maps.stockport.gov.uk/")
                .AddSource("share.typeform.com/")
                .AddSource("js.buto.tv/video/")
                .AddSource("s7.addthis.com/js/300/addthis_widget.js")
                .AddSource("m.addthis.com/live/")
                .AddSource("siteimproveanalytics.com/js/")
                .AddSource("*.logo-net.co.uk/Delivery/")
                .AddSource("https://www.opinionstage.com/assets/loader.js")
                .AddSource("https://www.google.com/recaptcha/api.js")
                .AddSource("https://www.gstatic.com/recaptcha/api2/")
                .AddSource("https://d26b395fwzu5fz.cloudfront.net/keen-tracking-1.1.3.min.js")
                .AddSource("https://www.jscache.com/")
                .AddSource("*.tripadvisor.com")
                .AddSource("*.tripadvisor.co.uk")
                .AddSource("static.tacdn.com")
                .AddSource("http://cdnjs.cloudflare.com/ajax/libs/clipboard.js/1.7.1/clipboard.min.js")
                .Finish());
        }

        private void BuildConnectSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("connect-src")
                .AddSource("https://api.cludo.com/")
                .AddSource("buto-ping-middleman.buto.tv/")
                .AddSource("m.addthis.com/live/")
                .AddSource("http://kinesis-ping-middleman.buto.tv")
                .AddSource("https://kinesis.eu-west-1.amazonaws.com/")
                .AddSource("https://zldiarvaya.execute-api.eu-west-1.amazonaws.com/prod/")
                .AddSource("https://13bg9nmobj.execute-api.eu-west-1.amazonaws.com/production/player-analytics")
                .Finish());
        }

        private void BuildMediaSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("media-src")
                .AddSource("https://www.youtube.com/")
                .AddSource("*.cloudfront.net/butotv/live/videos/")
                .Finish());
        }

        private void BuildObjectSource()
        {
            _stringBuilder.Append(
                new ContentSecurityPolicyElement
                    ("object-src")
                .AddSource("https://www.youtube.com")
                .AddSource("http://www.youtube.com")
                .Finish());
        }
    }
}