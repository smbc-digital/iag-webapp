namespace StockportWebappTests_Unit.Unit.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void ShouldRemoveHttpFromStartOfString()
    {
        // Act
        string result = "http://testing.com".StripHttpAndHttps();

        // Assert
        Assert.Equal("testing.com", result);
    }

    [Fact]
    public void ShouldRemoveHttpsFromStartOfString()
    {
        // Act
        string result = "https://testing.com".StripHttpAndHttps();

        // Assert
        Assert.Equal("testing.com", result);
    }

    [Fact]
    public void ShouldRemoveMoreThanOneHttporHttpssFromStartOfString()
    {
        // Act
        string result = "https://https://http://testing.com".StripHttpAndHttps();

        // Assert
        Assert.Equal("testing.com", result);
    }
}