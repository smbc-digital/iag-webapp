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
            var expectedCSP = "default-src https:; " +
                "child-src 'self' http://s7.addthis.com; " +
                "font-src 'self' font.googleapis.com maxcdn.bootstrapcdn.com/font-awesome/ fonts.gstatic.com/; " +
                "img-src 'self' khms0.googleapis.com khms1.googleapis.com geo0.ggpht.com geo1.ggpht.com geo2.ggpht.com geo3.ggpht.com cbks0.googleapis.com csi.gstatic.com " +
                                 "maps.gstatic.com maps.googleapis.com images.contentful.com/ www.google-analytics.com/r/collect www.google-analytics.com/collect stats.g.doubleclick.net/r/collect " +
                                 "https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/ " +
                                 "s3-eu-west-1.amazonaws.com/ share.typeform.com/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "customer.cludo.com/img/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/; " +
                "style-src 'self' 'unsafe-inline' *.cludo.com/css/ s3-eu-west-1.amazonaws.com/ share.typeform.com/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ " +
                                 "cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ *.cloudfront.net/butotv/; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ maps.googleapis.com m.addthisedge.com/live/boost/ " +
                                 "www.google-analytics.com/analytics.js api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ " +
                                 "s3-eu-west-1.amazonaws.com/ share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ " +
                                 "siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/; " +
                "connect-src 'self' https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/; " +
                "media-src 'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/; " +
                "object-src 'self' https://www.youtube.com http://www.youtube.com; "; 

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
