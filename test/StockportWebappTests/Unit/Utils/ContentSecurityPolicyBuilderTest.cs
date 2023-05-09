namespace StockportWebappTests_Unit.Unit.Utils;

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
        Assert.Contains("child-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainChildSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("child-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainFontSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("font-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainImageSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("img-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainStyleSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("style-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainScriptSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("script-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainConnectSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("connect-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainMediaSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("media-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainObjectSrcElement()
    {
        // Arrange
        var cspBuilder = new ContentSecurityPolicyBuilder();

        // Act
        var contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("object-src", contentSecurityPolicy);
    }
}
