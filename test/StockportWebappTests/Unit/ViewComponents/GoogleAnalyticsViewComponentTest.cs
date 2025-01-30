namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class GoogleAnalyticsViewComponentTest
{
    [Fact]
    public async Task ShouldReturnGoogleAnalyticsId()
    {
        // Arrange
        const string businessId = "businessID";
        AppSetting googleAnalyticsCode = AppSetting.GetAppSetting("a code");
        Mock<IApplicationConfiguration> config = new();
        config
            .Setup(conf => conf.GetGoogleAnalyticsCode(businessId))
            .Returns(googleAnalyticsCode);

        GoogleAnalyticsViewComponent googleAnalyticsViewComponent = new(config.Object, new BusinessId(businessId));

        // Act
        ViewViewComponentResult view = await googleAnalyticsViewComponent.InvokeAsync() as ViewViewComponentResult;

        // Assert
        config.Verify(o => o.GetGoogleAnalyticsCode(businessId), Times.Once);
        AppSetting model = view.ViewData.Model as AppSetting;
        Assert.Equal(googleAnalyticsCode, model);
    }
}