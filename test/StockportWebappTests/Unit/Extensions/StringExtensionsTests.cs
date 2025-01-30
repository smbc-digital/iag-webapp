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

    [Fact]
    public void ShouldRemoveEmojisFromString()
    {
        // Act
        string result = "😀🙏☀⛿test".StripEmojis();

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void ShouldRemoveEmojisInTheMiddleOfAString()
    {
        // Act
        string result = "😀🙏☀⛿te☀⛿s☀⛿t".StripEmojis();

        // Assert
        Assert.Equal("test", result);
    }
}