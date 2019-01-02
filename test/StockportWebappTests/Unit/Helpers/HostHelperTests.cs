using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using StockportWebapp.Config;
using StockportWebapp.Utils;

namespace StockportWebappTests_Unit.Unit.Helpers
{
    public class HostHelperTests
    {
        [Fact]
        public void ShouldReturnHost()
        {
            var hostHelper = new HostHelper(new CurrentEnvironment("local"));
            var httpContext = new DefaultHttpContext();
                httpContext.Request.Host = new HostString("host");
                httpContext.Request.Scheme = "http";

            var request = new DefaultHttpRequest(httpContext);
            hostHelper.GetHost(request).Should().Be("http://host");
        }

        [Fact]
        public void ShouldReturnHttpsHostForNonLocal()
        {
            var hostHelper = new HostHelper(new CurrentEnvironment("nonlocal"));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = new HostString("host");
            httpContext.Request.Scheme = "https";

            var request = new DefaultHttpRequest(httpContext);
            hostHelper.GetHost(request).Should().Be("https://host");
        }

        [Fact]
        public void ShouldReturnHostAndQueryString()
        {
            //Arrange
            var hostHelper = new HostHelper(new CurrentEnvironment("local"));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = new HostString("host.com");
            httpContext.Request.Scheme = "http";
            httpContext.Request.QueryString = QueryString.Create(new List<KeyValuePair<string,string>>
            {
                new KeyValuePair<string, string>("test", "test")
            });

            //Act
            var request = new DefaultHttpRequest(httpContext);

            //Assert
            hostHelper.GetHostAndQueryString(request).Should().Be("http://host.com?test=test");
        }
    }
}
