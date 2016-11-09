using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class RobotsTxtMiddlewareTest
    {
        private readonly RobotsTxtMiddleware _robotsTxtMiddleware;

        public RobotsTxtMiddlewareTest()
        {
            var requestDelegate = new Mock<RequestDelegate>();
            _robotsTxtMiddleware = new RobotsTxtMiddleware(requestDelegate.Object);
        }

        [Fact]
        public void ShouldSetRobotsTxtToBusinessIdSpecificIfPathIsRobots()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/robots.txt";
            var businessId = new BusinessId("businessid");
            _robotsTxtMiddleware.Invoke(context, businessId);

            context.Request.Path.ToString().Should().Be($"/robots-{businessId}.txt");
        }

        [Fact]
        public void ShouldNotSetRobotsTxtToBusinessIdSpecificIfPathIsNotRobots()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/notrobots";
            var businessId = new BusinessId("businessid");
            _robotsTxtMiddleware.Invoke(context, businessId);

            context.Request.Path.ToString().Should().Be("/notrobots");
        }
    }
}
