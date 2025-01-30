﻿namespace StockportWebappTests_Unit.Unit.Middleware;

public class RobotsMiddlewareTest
{
    private readonly RobotsMiddleware _robotsTxtMiddleware;
    private readonly IWebHostEnvironment _iWebHostingEnvironment;

    public RobotsMiddlewareTest()
    {
        Mock<RequestDelegate> requestDelegate = new();
        _robotsTxtMiddleware = new(requestDelegate.Object);

        Mock<IWebHostEnvironment> iWebHostingEnvironment = new();
        iWebHostingEnvironment
            .Setup(env => env.EnvironmentName)
            .Returns("prod");
        
        _iWebHostingEnvironment = iWebHostingEnvironment.Object;
    }

    [Fact]
    public void Should_Not_SetXRobotsTag_NoIndex()
    {
        // Arrange
        DefaultHttpContext context = new();
        BusinessId businessId = new(It.IsAny<string>());

        // Act
        _robotsTxtMiddleware.Invoke(context, businessId, _iWebHostingEnvironment);

        // Assert
        Assert.False(context.Response.Headers.ContainsKey("X-Robots-Tag"));
    }

    [Theory]
    [InlineData("local")]
    [InlineData("int")]
    [InlineData("qa")]
    [InlineData("stage")]
    public void Should_SetXRobotsTag_NoIndex(string environmentName)
    {
        // Arrange
        DefaultHttpContext context = new();
        BusinessId businessId = new(It.IsAny<string>());
        Mock<IWebHostEnvironment> iWebHostingEnvironment = new();
        iWebHostingEnvironment.Setup(env => env.EnvironmentName).Returns(environmentName);

        // Act
        _robotsTxtMiddleware.Invoke(context, businessId, iWebHostingEnvironment.Object);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("X-Robots-Tag"));
    }

    [Fact]
    public void ShouldSetRobotsTxtToBusinessIdSpecificIfPathIsRobots()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Path = "/robots.txt";
        context.Request.Host = new HostString("beta.domain.notwww");
        BusinessId businessId = new("businessid");
        
        // Act
        _robotsTxtMiddleware.Invoke(context, businessId, _iWebHostingEnvironment);

        // Assert
        Assert.Equal($"/robots-{businessId}.txt", context.Request.Path.ToString());
    }

    [Fact]
    public void ShouldNotSetRobotsTxtToBusinessIdSpecificIfPathIsNotRobots()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Path = "/notrobots";
        BusinessId businessId = new("businessid");

        // Act
        _robotsTxtMiddleware.Invoke(context, businessId, _iWebHostingEnvironment);

        // Assert
        Assert.Equal("/notrobots", context.Request.Path.ToString());
    }

    [Fact]
    public void ShouldSetRobotsTxtToLive()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Host = new HostString("www.liveurl.haswww");
        context.Request.Path = "/robots.txt";
        BusinessId businessId = new("businessid");

        // Act
        _robotsTxtMiddleware.Invoke(context, businessId, _iWebHostingEnvironment);

        // Assert
        Assert.Equal($"/robots-{businessId}-live.txt", context.Request.Path.ToString());
    }
}