namespace StockportWebappTests_Unit.Unit.Config;

public class AmazonSESClientConfigurationTest
{
    [Fact]
    public void ShouldCreateAnAmazonSESClientConfiguration()
    {
        // Arrange
        AmazonSESKeys awsKeys = new("accessKey", "secretKey");

        // Act
        AmazonSesClientConfiguration amazonSesClientConfig = new(AppSetting.GetAppSetting("a-host.com"),
                                                                AppSetting.GetAppSetting("region"),
                                                                AppSetting.GetAppSetting("email@email.com"),
                                                                awsKeys);

        // Assert
        Assert.Equal("a-host.com", amazonSesClientConfig.Host);
        Assert.Equal("https://a-host.com", amazonSesClientConfig.Endpoint);
        Assert.Equal("region", amazonSesClientConfig.Region);
        Assert.Equal("email@email.com", amazonSesClientConfig.EmailFrom);
        Assert.Equal("accessKey", amazonSesClientConfig.AwsAccessKeyId);
        Assert.Equal("secretKey", amazonSesClientConfig.AwsSecretAccessKey);
        Assert.True(amazonSesClientConfig.IsValid());
    }

    [Theory]
    [InlineData("", "region", "accessKey", "secretKey", "emailFrom")]
    [InlineData(null, "region", "accessKey", "secretKey", "emailFrom")]
    [InlineData("host", "", "accessKey", "secretKey", "emailFrom")]
    [InlineData("host", null, "accessKey", "secretKey", "emailFrom")]
    [InlineData("host", "region", "", "secretKey", "emailFrom")]
    [InlineData("host", "region", null, "secretKey", "emailFrom")]
    [InlineData("host", "region", "accessKey", "", "emailFrom")]
    [InlineData("host", "region", "accessKey", null, "emailFrom")]
    [InlineData("host", "region", "accessKey", "secretKey", "")]
    [InlineData("host", "region", "accessKey", "secretKey", null)]
    public void IsValid_ShouldReturnInvalidIfAPropertyIsNotValid(string host, string region, string accessKey, string secretKey, string emailFrom)
    {
        // Arrange
        AmazonSESKeys awsKeys = new(accessKey, secretKey);
        AmazonSesClientConfiguration amazonSesClientConfig = new(AppSetting.GetAppSetting(host),
                                                                AppSetting.GetAppSetting(region),
                                                                AppSetting.GetAppSetting(emailFrom),
                                                                awsKeys);
        // Act
        bool isValid = amazonSesClientConfig.IsValid();

        // Assert
        Assert.False(isValid);
    }
}
