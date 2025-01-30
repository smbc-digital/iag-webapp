namespace StockportWebappTests_Unit.Unit.Utils;

public class TypeRouteTests
{  
    [Theory]
    [InlineData("article", "test", "/test")]
    [InlineData("topic", "test", "/topic/test")]
    [InlineData("start-page", "test", "/start/test")]
    [InlineData("news", "test", "/news")]
    [InlineData("events", "test", "/events")]
    [InlineData("eventHomepage", "test", "/events")]
    [InlineData("payment", "test", "/payment/test")]
    [InlineData("servicePayPayment", "test", "/service-pay-payment/test")]
    [InlineData("service-pay-payment", "test", "/service-pay-payment/test")]
    [InlineData("showcase", "test", "/showcase/test")]
    [InlineData("sia", "test", "/sia")]
    [InlineData("privacy-notices", "test", "/privacy-notices/test")]
    [InlineData("default", "test", "/test")]
    [InlineData("groups", "test", "/groups/test")]
    [InlineData("groups", "groups", "/groups/")]
    [InlineData("directory", "test", "/directories/test")]
    [InlineData("directory", "/directories/test", "/directories/test")]

    public void GetUrlFor_Returns_CorrectRoutes(string type, string slug, string expected)
    {
        // Act
        string route = TypeRoutes.GetUrlFor(type, slug);
        
        // Assert
        Assert.Equal(expected, route);
    }
}