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
