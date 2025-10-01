namespace StockportWebappTests_Unit.Unit.Middleware;

public class SecurityHeaderMiddlewareTest
{
    private readonly SecurityHeaderMiddleware _middleware;
    private readonly Mock<RequestDelegate> requestDelegate = new();

    public SecurityHeaderMiddlewareTest() =>
        _middleware = new(requestDelegate.Object);

    [Theory]
    [InlineData("int-iag.domain.com")]
    [InlineData("qa-iag.domain.com")]
    [InlineData("stage-iag.domain.com")]
    [InlineData("www.domain.com")]
    public void Invoke_ShouldReturnStrictTransportSecurityHeader_For_IntQaStageProdAddresses(string host)
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Host = new HostString(host);

        // Act
        _middleware.Invoke(context);

        // Assert
        Assert.NotNull(context.Response.Headers["Content-Security-Policy"].ToString());
    }

    [Fact]
    public void Invoke_ShouldNotReturnStrictTransportSecurityHeader_For_LocalAddresses()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Host = new HostString("stockportgov:5555");

        // Act
        _middleware.Invoke(context);

        // Assert
        Assert.NotNull(context.Response.Headers["Content-Security-Policy"].ToString());
    }
}