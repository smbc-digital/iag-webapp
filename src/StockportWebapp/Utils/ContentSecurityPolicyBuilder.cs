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
			BuildManifestSource();
            BuildFrameSource();

			return _stringBuilder.ToString();
		}

		private void BuildDefaultSource()
		{
			_stringBuilder.Append(
				new ContentSecurityPolicyElement
					("default-src", containsSelf: false)
				.AddSource("https:")
				.AddSource("wss:", false)
				.AddSource("http:", false)
				.Finish());
		}

		private void BuildChildSourceAkaFrameSource()
		{
			_stringBuilder.Append(
				new ContentSecurityPolicyElement
					("child-src")
				.AddSource("http://s7.addthis.com")
				.AddSource("https://www.youtube.com")
				.AddSource("https://www.google.com/maps/")
				.AddSource("http://www.opinionstage.com/polls/")
				.AddSource("https://www.google.com/recaptcha/api2/anchor")
				.AddSource("https://www.google.com/recaptcha/api2/bframe")
				.AddSource("https://player.vimeo.com/")
				.AddSource("http://stage.midas-pps.tractivity.co.uk/")
				.AddSource("*.cloudfront.net/butotv/live/", false, true)
				.AddSource("https://y84kj.videomarketingplatform.co/", false, true)
				.AddSource("https://www.facebook.com/")
				.AddSource("*.stockport.gov.uk")
				.AddSource("*.smbcdigital.net")
				.AddSource("https://stockportmaps.github.io")
                .AddSource("blob:", false, true)
				.AddSource("https://vars.hotjar.com/")
				.AddSource("https://embed.buto.tv/")
				.AddSource("https://butoembed.twentythree.net/")
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
				.AddSource("stockportgov-design-system.s3-eu-west-1.amazonaws.com/")
				.AddSource("design-system.stockport.gov.uk/")
				.AddSource("static.tacdn.com")
				.AddSource("data:", false)
                .AddSource("s3-eu-west-1.amazonaws.com", true)
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
				.AddSource("images.ctfassets.net")
				.AddSource("www.google-analytics.com/r/collect")
				.AddSource("www.google-analytics.com/collect")
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
				.Finish());
		}

		private void BuildStyleSource()
		{
			_stringBuilder.Append(
				new ContentSecurityPolicyElement
					("style-src")
				.AddSource("'unsafe-inline'")
				.AddSource("cludo.com/css/")
				.AddSource("stockportgov-design-system.s3-eu-west-1.amazonaws.com/")
				.AddSource("s3-eu-west-1.amazonaws.com/")
				.AddSource("maxcdn.bootstrapcdn.com/font-awesome/")
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
				.AddSource("apis.google.com")
				.AddSource("m.addthisedge.com/live/boost/")
				.AddSource("v1.addthisedge.com/live/boost/")
				.AddSource("www.google-analytics.com/analytics.js")
                .AddSource("tagmanager.google.com/")
				.AddSource("api.cludo.com/scripts/")
				.AddSource("customer.cludo.com/scripts/")
				.AddSource("cdnjs.cloudflare.com/ajax/libs/cookieconsent2/")
				.AddSource("s3.eu-west-1.amazonaws.com/")
				.AddSource("s7.addthis.com/")
				.AddSource("maps.stockport.gov.uk/")
				.AddSource("js.buto.tv/video/")
				.AddSource("s7.addthis.com/js/300/addthis_widget.js")
				.AddSource("m.addthis.com/live/")
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
				.AddSource("https://core-api-eu1.cludo.com/")
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
				.AddSource("https://api.buto.tv/")
                .AddSource("spatialgeojson.s3.eu-west-1.amazonaws.com", true)
                .AddSource("spatialgeojson.s3-eu-west-1.amazonaws.com", true)
				.AddSource("https://report.23video.com/")
				.AddSource("https://plus.browsealoud.com/")
				.AddSource("https://www.browsealoud.com/")
				.AddSource("https://speech.speechstream.net/", true)
				.AddSource("https://www.google-analytics.com/")
                .Finish());
		}

		private void BuildMediaSource()
		{
			_stringBuilder.Append(
				new ContentSecurityPolicyElement
					("media-src")
				.AddSource("blob:")
				.AddSource("https://www.youtube.com/")
				.AddSource("*.cloudfront.net/butotv/live/", false, true)
				.AddSource("http://wpc.196c.planetstream.net/00196C/audio/")
				.AddSource("app.meetami.ai/")
				.AddSource("*.meetami.ai/", false)
				.Finish());
		}

	    private void BuildFrameSource()
	    {
	        _stringBuilder.Append(
	          new ContentSecurityPolicyElement
	                    ("frame-ancestors")
	                  .AddSource("*.stockport.gov.uk")
                    .AddSource("*.smbcdigital.net")
	                  .AddSource("*.meetami.ai/")
	                  .AddSource("*.chat.meetami.ai/")
                    .AddSource("*-formbuilder-origin.smbcdigital.net/", true)
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

		private void BuildManifestSource()
		{
			_stringBuilder.Append(
				new ContentSecurityPolicyElement
					("manifest-src")
				.AddSource("http://localhost:5000/assets/images/ui-images/sg/manifest.json")
				.Finish());
		}
	}
}
