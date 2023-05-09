namespace StockportWebappTests_Unit.Unit.Config;

public class AmazonSESClientConfigurationTest
{
    [Fact]
    public void ShouldCreateAnAmazonSESClientConfiguration()
    {
        var host = "a-host.com";
        var region = "region";
        var accessKey = "accessKey";
        var secretKey = "secretKey";
        var emailFrom = "email@email.com";
        var hostSetting = AppSetting.GetAppSetting(host);
        var regionSetting = AppSetting.GetAppSetting(region);
        var awsKeys = new AmazonSESKeys(accessKey, secretKey);
        var emailFromSetting = AppSetting.GetAppSetting(emailFrom);

        var amazonSesClientConfig = new AmazonSesClientConfiguration(hostSetting, regionSetting, emailFromSetting, awsKeys);

        amazonSesClientConfig.Host.Should().Be(host);
        amazonSesClientConfig.Endpoint.Should().Be($"https://{host}");
        amazonSesClientConfig.Region.Should().Be(region);
        amazonSesClientConfig.EmailFrom.Should().Be(emailFrom);
        amazonSesClientConfig.AwsAccessKeyId.Should().Be(accessKey);
        amazonSesClientConfig.AwsSecretAccessKey.Should().Be(secretKey);

        amazonSesClientConfig.IsValid().Should().BeTrue();
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
    public void ShouldReturnInvalidIfAPropertyIsNotValid(string host, string region, string accessKey, string secretKey, string emailFrom)
    {
        var hostSetting = AppSetting.GetAppSetting(host);
        var regionSetting = AppSetting.GetAppSetting(region);
        var awsKeys = new AmazonSESKeys(accessKey, secretKey);
        var emailFromSetting = AppSetting.GetAppSetting(emailFrom);

        var amazonSesClientConfig = new AmazonSesClientConfiguration(hostSetting, regionSetting, emailFromSetting, awsKeys);

        var isValid = amazonSesClientConfig.IsValid();

        isValid.Should().BeFalse();
    }
}
