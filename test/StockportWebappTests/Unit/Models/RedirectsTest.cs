namespace StockportWebappTests_Unit.Unit.Models;

public class RedirectsTest
{
    [Fact]
    public void ShouldCompareKeysWithCurrentCultureIgnoreCase()
    {
        // Arrange
        RedirectDictionary fromJsonRedirects = new()
        {
            {"from", "to"},
            {"from_again", "to_again"}
        };

        BusinessIdRedirectDictionary businessIdRedirects = new()
        {
            {"unittest", fromJsonRedirects}
        };

        // Act
        ShortUrlRedirects redirects = new(businessIdRedirects);

        // Assert
        Assert.Single(redirects.Redirects);
        Assert.Equal(StringComparer.CurrentCultureIgnoreCase, redirects.Redirects["unittest"].Comparer);
    }
}