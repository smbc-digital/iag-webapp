﻿using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Middleware
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
            context.Request.Host = new HostString("beta.domain.notwww");
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

        [Fact]
        public void ShouldSetRobotsTxtToLive()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.liveurl.haswww");
            context.Request.Path = "/robots.txt";
            var businessId = new BusinessId("businessid");
            _robotsTxtMiddleware.Invoke(context, businessId);

            context.Request.Path.ToString().Should().Be($"/robots-{businessId}-live.txt");
        }
    }
}
