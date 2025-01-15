namespace StockportWebappTests_Unit.Unit.Config;

public class AmazonSESKeysTest
{
    [Theory]
    [InlineData("", "secretKey")]
    [InlineData(null, "secretKey")]
    [InlineData("accessKey", "")]
    [InlineData("accessKey", null)]
    [InlineData("", "")]
    public void ShouldBeNotValidIfAnyParameterIsNullOrEmpty(string accessKey, string secretKey)
    {
        // Arrange
        AmazonSESKeys keys = new(accessKey, secretKey);

        // Act & Assert
        Assert.False(keys.IsValid());
    }

    [Fact]
    public void ShouldBeValidIfParametersAreSet()
    {
        // Arrange
        AmazonSESKeys keys = new("accessKey", "secretKey");
        
        // Act & Assert
        Assert.True(keys.IsValid());
    }
}
