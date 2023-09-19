namespace StockportWebappTests_Unit.Unit.Middleware;

public class SecurityHeaderMiddlewareTest
{

    private readonly SecurityHeaderMiddleware _middleware;

    public SecurityHeaderMiddlewareTest()
    {
        var requestDelegate = new Mock<RequestDelegate>();

        _middleware = new SecurityHeaderMiddleware(requestDelegate.Object);
    }

    [Theory]
    [InlineData("int-iag.domain.com")]
    [InlineData("qa-iag.domain.com")]
    [InlineData("stage-iag.domain.com")]
    [InlineData("www.domain.com")]
    public void Invoke_ShouldReturnStrictTransportSecurityHeader_For_IntQaStageProdAddresses(string host)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString(host);

        // Act
        _middleware.Invoke(context);

        // Assert
        context.Response.Headers["Content-Security-Policy"].ToString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Invoke_ShouldNotReturnStrictTransportSecurityHeader_For_LocalAddresses()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("stockportgov:5555");

        // Act
        _middleware.Invoke(context);

        // Assert
        context.Response.Headers["Content-Security-Policy"].ToString().Should().BeNullOrEmpty();
    }
}
