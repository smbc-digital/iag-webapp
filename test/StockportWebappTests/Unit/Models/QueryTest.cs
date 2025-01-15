namespace StockportWebappTests_Unit.Unit.Models;

public class QueryTest
{
    [Fact]
    public void ShouldCreateFormattedQueryOnToString()
    {
        // Act
        Query query = new("name", "value");

        // Assert
        Assert.Equal("name=value", query.ToString());
    }

    [Fact]
    public void ShouldUrlencodeQueryStringValues()
    {
        // Act
        Query query = new("name", "#value");

        // Assert
        Assert.Equal("%23value", query.Value);
    }
}