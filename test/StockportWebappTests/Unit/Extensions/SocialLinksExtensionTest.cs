namespace StockportWebappTests_Unit.Unit.Extensions;

public class SocialLinksExtensionTest
{
    readonly SocialLinksExtension socialLinksExtension = new();

    [Fact]
    public void ShouldReturnFacebookDisplayUrlFromFullUrl()
    {
        // Act
        string result = socialLinksExtension.GetSubstring("http://www.facebook.com/zumba");

        // Assert
        Assert.Equal("/zumba", result);
    }

    [Fact]
    public void ShouldReturnTwitterDisplayUrlFromFullUrl()
    {
        // Act
        string result = socialLinksExtension.GetSubstring("http://www.twitter.com/zumba");

        // Assert
        Assert.Equal("@zumba", result);
    }
}