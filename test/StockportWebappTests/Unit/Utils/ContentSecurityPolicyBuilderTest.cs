using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class ContentSecurityPolicyBuilderTest
    {
        [Fact]
        public void CSPHasCorrectFinishedContent()
        {
			// Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();
            var expectedCSP =
                "default-src https:; "
                + "child-src 'self'"
                    + " http://s7.addthis.com" + " https://s7.addthis.com"
                    + " http://www.youtube.com" + " https://www.youtube.com; "
                + "font-src 'self'"
                    + " http://font.googleapis.com" + " https://font.googleapis.com"
                    + " http://maxcdn.bootstrapcdn.com/font-awesome/" + " https://maxcdn.bootstrapcdn.com/font-awesome/"
                    + " http://fonts.gstatic.com/" + " https://fonts.gstatic.com/; "
                + "img-src 'self'"
                    + " http://khms0.googleapis.com" + " https://khms0.googleapis.com"
                    + " http://khms1.googleapis.com" + " https://khms1.googleapis.com"
                    + " http://geo0.ggpht.com" + " https://geo0.ggpht.com"
                    + " http://geo1.ggpht.com" + " https://geo1.ggpht.com"
                    + " http://geo2.ggpht.com" + " https://geo2.ggpht.com"
                    + " http://geo3.ggpht.com" + " https://geo3.ggpht.com"
                    + " http://cbks0.googleapis.com" + " https://cbks0.googleapis.com"
                    + " http://csi.gstatic.com" + " https://csi.gstatic.com"
                    + " http://maps.gstatic.com" + " https://maps.gstatic.com"
                    + " http://maps.googleapis.com" + " https://maps.googleapis.com"
                    + " http://images.contentful.com/" + " https://images.contentful.com/"
                    + " http://www.google-analytics.com/r/collect" + " https://www.google-analytics.com/r/collect"
                    + " http://www.google-analytics.com/collect" + " https://www.google-analytics.com/collect"
                    + " http://stats.g.doubleclick.net/r/collect" + " https://stats.g.doubleclick.net/r/collect"
                    + " http://s3-eu-west-1.amazonaws.com/live-iag-static-assets/" + " https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/"
                    + " http://s3-eu-west-1.amazonaws.com/" + " https://s3-eu-west-1.amazonaws.com/"
                    + " http://share.typeform.com/" + " https://share.typeform.com/"
                    + " http://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/" + " https://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/"
                    + " http://customer.cludo.com/img/" + " https://customer.cludo.com/img/"
                    + " http://uk1.siteimprove.com/" + " https://uk1.siteimprove.com/"
                    + " http://stockportb.logo-net.co.uk/" + " https://stockportb.logo-net.co.uk/"
                    + " *.cloudfront.net/butotv/; "
               + "style-src 'self' 'unsafe-inline'"
                    + " *.cludo.com/css/"
                    + " http://s3-eu-west-1.amazonaws.com/" + " https://s3-eu-west-1.amazonaws.com/"
                    + " http://share.typeform.com/" + " https://share.typeform.com/"
                    + " http://maxcdn.bootstrapcdn.com/font-awesome/" + " https://maxcdn.bootstrapcdn.com/font-awesome/"
                    + " http://fonts.googleapis.com/" + " https://fonts.googleapis.com/"
                    + " http://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/" + " https://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/"
                    + " *.cloudfront.net/butotv/; "
                + "script-src 'self' 'unsafe-inline' 'unsafe-eval'"
                    + " http://ajax.googleapis.com/ajax/libs/jquery/" + " https://ajax.googleapis.com/ajax/libs/jquery/"
                    + " http://maps.googleapis.com" + " https://maps.googleapis.com"
                    + " http://m.addthisedge.com/live/boost/" + " https://m.addthisedge.com/live/boost/"
                    + " http://www.google-analytics.com/analytics.js" + " https://www.google-analytics.com/analytics.js"
                    + " http://api.cludo.com/scripts/" + " https://api.cludo.com/scripts/"
                    + " http://customer.cludo.com/scripts/" + " https://customer.cludo.com/scripts/"
                    + " http://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/" + " https://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/"
                    + " http://s3-eu-west-1.amazonaws.com/" + " https://s3-eu-west-1.amazonaws.com/"
                    + " http://share.typeform.com/" + " https://share.typeform.com/"
                    + " http://js.buto.tv/video/" + " https://js.buto.tv/video/"
                    + " http://s7.addthis.com/js/300/addthis_widget.js" + " https://s7.addthis.com/js/300/addthis_widget.js"
                    + " http://m.addthis.com/live/" + " https://m.addthis.com/live/"
                    + " http://siteimproveanalytics.com/js/" + " https://siteimproveanalytics.com/js/"
                    + " *.logo-net.co.uk/Delivery/; "
                + "connect-src 'self'"
                    + " http://api.cludo.com/" + " https://api.cludo.com/"
                    + " http://buto-ping-middleman.buto.tv/" + " https://buto-ping-middleman.buto.tv/"
                    + " http://m.addthis.com/live/" + " https://m.addthis.com/live/; "
                + "media-src 'self'"
                    + " http://www.youtube.com/" + " https://www.youtube.com/"
                    + " *.cloudfront.net/butotv/live/videos/; "
                + "object-src 'self'"
                    + " http://www.youtube.com" + " https://www.youtube.com"
                    + " http://www.youtube.com" + " https://www.youtube.com; ";

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

			// Assert
            Assert.Equal(expectedCSP, contentSecurityPolicy);
        }

        [Fact]
        public void CSPWillContainDefaultSrcElement()
        {
			// Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

			// Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

			// Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("child-src"));
        }

        [Fact]
        public void CSPWillContainChildSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("child-src"));
        }

        [Fact]
        public void CSPWillContainFontSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("font-src"));
        }

        [Fact]
        public void CSPWillContainImageSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("img-src"));
        }

        [Fact]
        public void CSPWillContainStyleSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("style-src"));
        }

        [Fact]
        public void CSPWillContainScriptSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("script-src"));
        }

        [Fact]
        public void CSPWillContainConnectSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("connect-src"));
        }

        [Fact]
        public void CSPWillContainMediaSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("media-src"));
        }

        [Fact]
        public void CSPWillContainObjectSrcElement()
        {
            // Arrange
            var cspBuilder = new ContentSecurityPolicyBuilder();

            // Act
            var contentSecurityPolicy = cspBuilder.BuildPolicy();

            // Assert
            Assert.Equal(true, contentSecurityPolicy.Contains("object-src"));
        }
    }
}
