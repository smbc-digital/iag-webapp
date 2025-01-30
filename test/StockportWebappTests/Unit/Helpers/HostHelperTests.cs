namespace StockportWebappTests_Unit.Unit.Helpers;

public class HostHelperTests
{
    [Fact]
    public void ShouldReturnHost()
    {
        // Arrange
        HostHelper hostHelper = new(new CurrentEnvironment("local"));
        DefaultHttpContext httpContext = new();
        httpContext.Request.Host = new HostString("host");
        httpContext.Request.Scheme = "http";
        HttpRequest request = httpContext.Request;

        // Act & Assert
        Assert.Equal("http://host", hostHelper.GetHost(request));
    }

    [Fact]
    public void ShouldReturnHttpsHostForNonLocal()
    {
        // Arrange
        HostHelper hostHelper = new(new CurrentEnvironment("nonlocal"));
        DefaultHttpContext httpContext = new();
        httpContext.Request.Host = new HostString("host");
        httpContext.Request.Scheme = "https";
        HttpRequest request = httpContext.Request;

        // Act & Assert
        Assert.Equal("https://host", hostHelper.GetHost(request));
    }

    [Fact]
    public void ShouldReturnHostAndQueryString()
    {
        // Arrange
        HostHelper hostHelper = new(new CurrentEnvironment("local"));
        DefaultHttpContext httpContext = new();
        httpContext.Request.Host = new HostString("host.com");
        httpContext.Request.Scheme = "http";
        httpContext.Request.QueryString = QueryString.Create(new List<KeyValuePair<string, string>>
        {
            new("test", "test")
        });

        // Act & Assert
        Assert.Equal("http://host.com?test=test", hostHelper.GetHostAndQueryString(httpContext.Request));
    }
}