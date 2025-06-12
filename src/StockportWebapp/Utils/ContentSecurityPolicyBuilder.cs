namespace StockportWebapp.Utils;

public class ContentSecurityPolicyBuilder
{
    private StringBuilder _stringBuilder = new();

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
        BuildManifestSource();
        BuildFrameSource();
        //BuildFormAction();

        return _stringBuilder.ToString();
    }

    private void BuildDefaultSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("default-src", containsSelf: false)
            .AddSource("https:")
            .AddSource("wss:", false)
            .AddSource("http:", false)
            .Finish());

    private void BuildChildSourceAkaFrameSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("child-src")
            .AddSource("https://www.youtube.com")
            .AddSource("https://www.google.com/")
            .AddSource("https://www.google.com/maps/")
            .AddSource("http://www.opinionstage.com/polls/")
            .AddSource("https://www.google.com/recaptcha/api2/anchor")
            .AddSource("https://www.google.com/recaptcha/api2/bframe")
            .AddSource("https://player.vimeo.com/")
            .AddSource("http://stage.midas-pps.tractivity.co.uk/")
            .AddSource("*.cloudfront.net/butotv/live/", false, true)
            .AddSource("https://y84kj.videomarketingplatform.co/", false, true)
            .AddSource("https://www.facebook.com/")
            .AddSource("https://www.instagram.com/")
            .AddSource("*.stockport.gov.uk")
            .AddSource("*.smbcdigital.net")
            .AddSource("https://stockportmaps.github.io")
            .AddSource("blob:", false, true)
            .AddSource("https://vars.hotjar.com/")
            .AddSource("https://embed.buto.tv/")
            .AddSource("https://butoembed.twentythree.net/")
            .AddSource("forms-eu1.hsforms.com")
            .AddSource("my.matterport.com")
            .AddSource("lookinglocal.cdn.spotlightr.com")
            .AddSource("https://api-bridge.azurewebsites.net/")
            .Finish());

    private void BuildFontSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("font-src")
            .AddSource("font.googleapis.com")
            .AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
            .AddSource("cdnjs.cloudflare.com/ajax/libs/font-awesome/")
            .AddSource("kit.fontawesome.com/")
            .AddSource("ka-p.fontawesome.com/")
            .AddSource("fonts.gstatic.com/")
            .AddSource("stockportgov-design-system.s3-eu-west-1.amazonaws.com/")
            .AddSource("design-system.stockport.gov.uk/")
            .AddSource("static.tacdn.com")
            .AddSource("data:", false)
            .AddSource("s3-eu-west-1.amazonaws.com", true)
            .AddSource("api.reciteme.com/assets/")
            .Finish());

    private void BuildImageSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("img-src")
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
            .AddSource("images.ctfassets.net")
            .AddSource("www.google-analytics.com/r/collect")
            .AddSource("www.google-analytics.com/collect")
            .AddSource("streetviewpixels-pa.googleapis.com")
            .AddSource("stats.g.doubleclick.net/r/collect")
            .AddSource("s3-eu-west-1.amazonaws.com/")
            .AddSource("maps.stockport.gov.uk/")
            .AddSource("interactive.stockport.gov.uk/")
            .AddSource("ads.astuntechnology.com/")
            .AddSource("s3-eu-west-1.amazonaws.com/")
            .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
            .AddSource("customer.cludo.com/img/")
            .AddSource("uk1.siteimprove.com/")
            .AddSource("stockportb.logo-net.co.uk/")
            .AddSource("cloudfront.net/butotv/")
            .AddSource("data:")
            .AddSource("www.tripadvisor.co.uk/")
            .AddSource("syndication.twitter.com/i/")
            .AddSource("platform.twitter.com/css/")
            .AddSource("pbs.twimg.com/")
            .AddSource("https://1.bp.blogspot.com/-v6yARqgGaBc/WKL2ZtO9lhI/AAAAAAAAEDU/0CJfMgpdnWg0i6-Wd87E1vTtdKk4TeikQCLcB/s1600/Fake-or-Counterfeit-Bathmate-Pumps.png")
            .AddSource("https://content.govdelivery.com/attachments/fancy_images/UKSMBC/2018/01/1741761/reviewoverlay_original.png")
            .AddSource("https://app.meetami.ai")
            .AddSource("*.cloudfront.net/butotv/live/", false, true)
            .AddSource("https://www.facebook.com/")
            .AddSource("*.siteimproveanalytics.io/")
            .AddSource("https://www.googletagmanager.com/")
            .AddSource("blob:", false, true)
            .AddSource("spatial.stockport.gov.uk/")
            .AddSource("https://ssl.gstatic.com/")
            .AddSource("https://www.gstatic.com/")
            .AddSource("https://lh3.googleusercontent.com/")
            .AddSource("api.reciteme.com/")
            .AddSource("aomg-sr-app-live.s3.eu-west-1.amazonaws.com/")
            .AddSource("forms.hsforms.com")
            .AddSource("forms-eu1.hsforms.com")
            .AddSource("lh3.ggpht.com")
            .AddSource("cdn.jsdelivr.net", true)
            .AddSource("https://img.freepik.com/free-vector/")
            .Finish());

    private void BuildStyleSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("style-src")
            .AddSource("'unsafe-inline'")
            .AddSource("cludo.com/css/")
            .AddSource("customer.cludo.com/css/")
            .AddSource("stockportgov-design-system.s3-eu-west-1.amazonaws.com/")
            .AddSource("s3-eu-west-1.amazonaws.com/")
            .AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
            .AddSource("kit.fontawesome.com/")
            .AddSource("ka-p.fontawesome.com/")
            .AddSource("cdnjs.cloudflare.com/ajax/libs/font-awesome/")
            .AddSource("fonts.googleapis.com/")
            .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
            .AddSource("maps.stockport.gov.uk/")
            .AddSource("design-system.stockport.gov.uk/")
            .AddSource("cloudfront.net/butotv/")
            .AddSource("tripadvisor.com")
            .AddSource("tripadvisor.co.uk")
            .AddSource("static.tacdn.com")
            .AddSource("data:")
            .AddSource("platform.twitter.com/css/")
            .AddSource("stockportb.logo-net.co.uk/Delivery/")
            .AddSource("*.cloudfront.net/butotv/live/", false, true)
            .AddSource("tagmanager.google.com/")
            .AddSource("https://cdn.websitepolicies.io/lib/cookieconsent/cookieconsent.min.css")
            .AddSource("unpkg.com/")
            .AddSource("api.mapbox.com/")
            .AddSource("api.reciteme.com/")
            .AddSource("cdn.jsdelivr.net", true)
            .Finish());

    private void BuildScriptSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("script-src")
            .AddSource("'unsafe-inline'")
            .AddSource("'unsafe-eval'")
            .AddSource("https://ajax.googleapis.com/ajax/libs/jquery/")
            .AddSource("maps.googleapis.com")
            .AddSource("apis.google.com")
            .AddSource("www.google-analytics.com/analytics.js")
            .AddSource("tagmanager.google.com/")
            .AddSource("api.cludo.com/scripts/")
            .AddSource("customer.cludo.com/scripts/")
            .AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
            .AddSource("design-system.stockport.gov.uk/")
            .AddSource("s3.eu-west-1.amazonaws.com/")
            .AddSource("maps.stockport.gov.uk/")
            .AddSource("js.buto.tv/video/")
            .AddSource("siteimproveanalytics.com/js/")
            .AddSource("logo-net.co.uk/Delivery/")
            .AddSource("https://www.opinionstage.com/assets/loader.js")
            .AddSource("https://www.google.com/recaptcha/api.js")
            .AddSource("https://www.gstatic.com/recaptcha/")
            .AddSource("https://www.googletagmanager.com/")
            .AddSource("https://d26b395fwzu5fz.cloudfront.net/keen-tracking-1.1.3.min.js")
            .AddSource("https://www.jscache.com/")
            .AddSource("tripadvisor.com")
            .AddSource("tripadvisor.co.uk")
            .AddSource("static.tacdn.com")
            .AddSource("http://cdnjs.cloudflare.com/ajax/libs/clipboard.js/1.7.1/clipboard.min.js")
            .AddSource("platform.twitter.com/")
            .AddSource("cdn.syndication.twimg.com/timeline/")
            .AddSource("platform.twitter.com/css/")
            .AddSource("local.tractivity.co.uk/wp-includes/js/")
            .AddSource("stage.midas-pps.tractivity.co.uk/")
            .AddSource("https://content.govdelivery.com/overlay/js/4939.js")
            .AddSource("https://core-api-eu1.cludo.com/")
            .AddSource("app.meetami.ai/")
            .AddSource("stockportgov-design-system.s3-eu-west-1.amazonaws.com/")
            .AddSource("wss://chat.meetami.ai/", false, true)
            .AddSource("wss://chat.meetami.ai/socket.io/", false, true)
            .AddSource("https://cdn.trackjs.com/releases/current/tracker.js")
            .AddSource("http://feed2js.org/feed2js.php")
            .AddSource("https://connect.facebook.net/")
            .AddSource("widget.wheredoivote.co.uk/")
            .AddSource("https://static.hotjar.com/")
            .AddSource("https://cdn.websitepolicies.io/lib/cookieconsent/cookieconsent.min.js")
            .AddSource("unpkg.com/")
            .AddSource("api.mapbox.com/")
            .AddSource("https://script.hotjar.com/")
            .AddSource("spatialgeojson.s3.eu-west-1.amazonaws.com", true)
            .AddSource("spatialgeojson.s3-eu-west-1.amazonaws.com", true)
            .AddSource("https://www.browsealoud.com/")
            .AddSource("https://plus.browsealoud.com/")
            .AddSource("https://speech.speechstream.net/", true)
            .AddSource("https://www.google-analytics.com/", true)
            .AddSource("api.reciteme.com")
            .AddSource("js-eu1.hsforms.net")
            .AddSource("js-eu1.hs-scripts.com")
            .AddSource("js-eu1.hscollectedforms.net")
            .AddSource("js-eu1.hs-analytics.net")
            .AddSource("js-eu1.hs-banner.com")
            .AddSource("js-eu1.hsadspixel.net")
            .AddSource("forms-eu1.hsforms.com")
            .AddSource("www.freeprivacypolicy.com")
            .AddSource("unpkg.com/@googlemaps/")
            .AddSource("https://kit.fontawesome.com/")
            .AddSource("ka-p.fontawesome.com/")
            .AddSource("cdn.jsdelivr.net", true)
            .AddSource("https://static.qa.conesso.io/scripts/signup.js")
            .Finish());

    private void BuildConnectSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("connect-src")
            .AddSource("https://api.cludo.com/")
            .AddSource("buto-ping-middleman.buto.tv/")
            .AddSource("http://kinesis-ping-middleman.buto.tv")
            .AddSource("https://kinesis.eu-west-1.amazonaws.com/")
            .AddSource("https://zldiarvaya.execute-api.eu-west-1.amazonaws.com/prod/")
            .AddSource("https://13bg9nmobj.execute-api.eu-west-1.amazonaws.com/production/player-analytics")
            .AddSource("https://core-api-eu1.cludo.com/")
            .AddSource("https://api-eu1.cludo.com/")
            .AddSource("https://event-collector.buto.tv/")
            .AddSource("app.meetami.ai/")
            .AddSource("https://chat.meetami.ai/")
            .AddSource("wss://chat.meetami.ai/", false, true)
            .AddSource("wss://chat.meetami.ai/socket.io/", false, true)
            .AddSource("http://localhost/sitereplier/chats/enabled/")
            .AddSource("*.stockport.gov.uk")
            .AddSource("*.smbcdigital.net")
            .AddSource("api.mapbox.com/")
            .AddSource("events.mapbox.com/")
            .AddSource("https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css")
            .AddSource("https://kit.fontawesome.com/")
            .AddSource("https://api.buto.tv/")
            .AddSource("spatialgeojson.s3.eu-west-1.amazonaws.com", true)
            .AddSource("spatialgeojson.s3-eu-west-1.amazonaws.com", true)
            .AddSource("https://report.23video.com/")
            .AddSource("https://plus.browsealoud.com/")
            .AddSource("https://www.browsealoud.com/")
            .AddSource("https://speech.speechstream.net/", true)
            .AddSource("https://www.google-analytics.com/")
            .AddSource("https://region1.google-analytics.com/g/collect")
            .AddSource("maps.googleapis.com")
            .AddSource("stats.reciteme.com")
            .AddSource("api.reciteme.com")
            .AddSource("events.reciteme.com")
            .AddSource("https://s3.eu-west-1.amazonaws.com/maps.stockport.gov.uk/")
            .AddSource("https://raw.githubusercontent.com/OrdnanceSurvey/")
            .AddSource("https://api.os.uk/")
            .AddSource("forms-eu1.hsforms.com")
            .AddSource("forms.hsforms.com")
            .AddSource("forms-eu1.hscollectedforms.net")
            .AddSource("api-eu1.hubapi.com")
            .AddSource("hubspot-forms-static-embed-eu1.s3.amazonaws.com/")
            .AddSource("ka-p.fontawesome.com/")
            .AddSource("lookinglocal.cdn.spotlightr.com/")
            .AddSource("https://api.conesso.io/v2/signup-forms/contact")
            .Finish());

    private void BuildMediaSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("media-src")
            .AddSource("blob:", true, true)
            .AddSource("https://www.youtube.com/")
            .AddSource("*.cloudfront.net/butotv/live/", false, true)
            .AddSource("http://wpc.196c.planetstream.net/00196C/audio/")
            .AddSource("app.meetami.ai/")
            .AddSource("*.meetami.ai/", false)
            .AddSource("https://api.reciteme.com/")
            .AddSource("lookinglocal.cdn.spotlightr.com/")
            .Finish());

    private void BuildFrameSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("frame-ancestors")
            .AddSource("*.stockport.gov.uk")
            .AddSource("*.smbcdigital.net")
            .AddSource("*.meetami.ai/")
            .AddSource("*.chat.meetami.ai/")
            .AddSource("forms.stockport.gov.uk", true)
            .AddSource("app.contentful.com")
            .AddSource("forms-eu1.hsforms.com")
            .AddSource("my.matterport.com")
            .AddSource("int-formbuilder-origin.smbcdigital.net", true)
            .AddSource("lookinglocal.cdn.spotlightr.com/")
            .Finish());

    private void BuildObjectSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("object-src")
            .AddSource("https://www.youtube.com")
            .AddSource("http://www.youtube.com")
            .Finish());

    private void BuildManifestSource() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("manifest-src")
            .AddSource("http://localhost:5000/assets/images/ui-images/sg/manifest.json")
            .Finish());

    private void BuildFormAction() =>
        _stringBuilder.Append(
            new ContentSecurityPolicyElement("form-action")
            .AddSource("forms-eu1.hsforms.com", true)
            .Finish());
}