namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class GoogleAnalyticsViewComponentTest
{
    [Fact]
    public async Task ShouldReturnGoogleAnalyticsId()
    {
        // Arrange
        AppSetting googleAnalyticsCode = AppSetting.GetAppSetting("a code");
        Mock<IApplicationConfiguration> config = new();
        config
            .Setup(conf => conf.GetGoogleAnalyticsCode("businessID"))
            .Returns(googleAnalyticsCode);

        GoogleAnalyticsViewComponent googleAnalyticsViewComponent = new(config.Object, new BusinessId("businessID"));

        // Act
        ViewViewComponentResult view = await googleAnalyticsViewComponent.InvokeAsync() as ViewViewComponentResult;

        // Assert
        config.Verify(config => config.GetGoogleAnalyticsCode("businessID"), Times.Once);
        AppSetting model = view.ViewData.Model as AppSetting;
        Assert.Equal(googleAnalyticsCode, model);
    }
}