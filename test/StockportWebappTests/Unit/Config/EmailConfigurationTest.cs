namespace StockportWebappTests_Unit.Unit.Config;

public class EmailConfigurationBuilderTest
{
    [Fact]
    public void ShouldBuildEmailConfiguration()
    {
        const string businessId = "businessId";
        var hostSetting = AppSetting.GetAppSetting("host");
        var regionSetting = AppSetting.GetAppSetting("region");
        var emailFromSetting = AppSetting.GetAppSetting("emailFrom");
        const string awsAccessKeyId = "accessKey";
        const string awsSecretAccessKey = "secretKey";

        var amazonSESKeys = new AmazonSESKeys(awsAccessKeyId, awsSecretAccessKey);
        var appsettings = new Mock<IApplicationConfiguration>();
        appsettings.Setup(o => o.GetEmailHost(businessId)).Returns(hostSetting);
        appsettings.Setup(o => o.GetEmailRegion(businessId)).Returns(regionSetting);
        appsettings.Setup(o => o.GetEmailEmailFrom(businessId)).Returns(emailFromSetting);

        var emailConfigurationBuilder = new EmailConfigurationBuilder(amazonSESKeys, appsettings.Object);

        var emailConfig = emailConfigurationBuilder.Build(businessId);

        emailConfig.Host.Should().Be(hostSetting.ToString());
        emailConfig.Region.Should().Be(regionSetting.ToString());
        emailConfig.AwsAccessKeyId.Should().Be(awsAccessKeyId);
        emailConfig.AwsSecretAccessKey.Should().Be(awsSecretAccessKey);
        emailConfig.EmailFrom.Should().Be(emailFromSetting.ToString());
    }
}
