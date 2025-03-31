namespace StockportWebappTests_Unit.Unit.Utils;

public class ContentSecurityPolicyBuilderTest
{
    [Fact]
    public void CSPWillContainDefaultSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("child-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainChildSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("child-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainFontSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("font-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainImageSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("img-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainStyleSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("style-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainScriptSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("script-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainConnectSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("connect-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainMediaSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("media-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainObjectSrcElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("object-src", contentSecurityPolicy);
    }

    [Fact]
    public void CSPWillContainFormActionElement()
    {
        // Arrange
        ContentSecurityPolicyBuilder cspBuilder = new();

        // Act
        string contentSecurityPolicy = cspBuilder.BuildPolicy();

        // Assert
        Assert.Contains("form-action", contentSecurityPolicy);
    }
}