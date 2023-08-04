﻿namespace StockportWebappTests_Unit.Unit.Middleware;

public class SecurityHeaderMiddlewareTest
{

    private readonly SecurityHeaderMiddleware _middleware;

    public SecurityHeaderMiddlewareTest()
    {
        var requestDelegate = new Mock<RequestDelegate>();

        _middleware = new SecurityHeaderMiddleware(requestDelegate.Object);
    }

    [Fact(Skip = "Check if the new CSP works first")]
    public void ShouldReturnStrictTransportSecurityHeaderForWWW()
    {
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("www.domain.com");

        _middleware.Invoke(context);

        context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
        context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
    }

    [Fact(Skip = "Check if the new CSP works first")]
    public void ShouldReturnStrictTransportSecurityHeaderForInt()
    {
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("int-iag.domain.com");

        _middleware.Invoke(context);

        context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
        context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
    }

    [Fact(Skip = "Check if the new CSP works first")]
    public void ShouldReturnStrictTransportSecurityHeaderForQA()
    {
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("int-qa.domain.com");

        _middleware.Invoke(context);

        context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
        context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
    }

    [Fact(Skip = "Check if the new CSP works first")]
    public void ShouldReturnStrictTransportSecurityHeaderForStage()
    {
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("int-stage.domain.com");

        _middleware.Invoke(context);

        context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
        context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
    }

    [Fact(Skip = "Check if the new CSP works first")]
    public void ShouldNotReturnStrictTransportSecurityHeaderForLocalAddresses()
    {
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("stockportgov:5555");

        _middleware.Invoke(context);

        context.Response.Headers["Strict-Transport-Security"].ToString().Should().BeNullOrEmpty();
    }
}
