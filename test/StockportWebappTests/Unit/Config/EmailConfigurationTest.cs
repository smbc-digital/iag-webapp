namespace StockportWebappTests_Unit.Unit.Config;

public class EmailConfigurationBuilderTest
{
    [Fact]
    public void ShouldBuildEmailConfiguration()
    {
        // Arrange
        AmazonSESKeys amazonSESKeys = new("accessKey", "secretKey");
        Mock<IApplicationConfiguration> appsettings = new();

        appsettings.Setup(setting => setting.GetEmailHost("businessId")).Returns(AppSetting.GetAppSetting("host"));
        appsettings.Setup(setting => setting.GetEmailRegion("businessId")).Returns(AppSetting.GetAppSetting("region"));
        appsettings.Setup(setting => setting.GetEmailEmailFrom("businessId")).Returns(AppSetting.GetAppSetting("emailFrom"));

        EmailConfigurationBuilder emailConfigurationBuilder = new(amazonSESKeys, appsettings.Object);

        // Act
        AmazonSesClientConfiguration emailConfig = emailConfigurationBuilder.Build("businessId");

        // Assert
        Assert.Equal(AppSetting.GetAppSetting("host").ToString(), emailConfig.Host);
        Assert.Equal(AppSetting.GetAppSetting("region").ToString(), emailConfig.Region);
        Assert.Equal("accessKey", emailConfig.AwsAccessKeyId);
        Assert.Equal("secretKey", emailConfig.AwsSecretAccessKey);
        Assert.Equal(AppSetting.GetAppSetting("emailFrom").ToString(), emailConfig.EmailFrom);
    }
}