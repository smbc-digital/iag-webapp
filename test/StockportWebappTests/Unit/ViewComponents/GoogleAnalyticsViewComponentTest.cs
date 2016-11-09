using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.ViewComponents;
using Xunit;

namespace StockportWebappTests.Unit.ViewComponents
{
    public class GoogleAnalyticsViewComponentTest
    {
        [Fact]
        public void ShouldReturnGoogleAnalyticsId()
        {
            const string businessId = "businessID";
            var googleAnalyticsCode = AppSetting.GetAppSetting("a code");

            var config = new Mock<IApplicationConfiguration>();
            config.Setup(o => o.GetGoogleAnalyticsCode(businessId)).Returns(googleAnalyticsCode);

            var googleAnalyticsViewComponent = new GoogleAnalyticsViewComponent(config.Object, new BusinessId(businessId));

            var view = AsyncTestHelper.Resolve(googleAnalyticsViewComponent.InvokeAsync()) as ViewViewComponentResult;

            config.Verify(o => o.GetGoogleAnalyticsCode(businessId), Times.Once);
            var model = view.ViewData.Model as AppSetting;
            model.Should().Be(googleAnalyticsCode);
        }
    }
}
