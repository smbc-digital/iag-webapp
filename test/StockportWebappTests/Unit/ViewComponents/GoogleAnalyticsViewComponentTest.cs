using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.ViewComponents;
using System.Threading.Tasks;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ViewComponents
{
    public class GoogleAnalyticsViewComponentTest
    {
        [Fact]
        public async Task ShouldReturnGoogleAnalyticsId()
        {
            const string businessId = "businessID";
            var googleAnalyticsCode = AppSetting.GetAppSetting("a code");

            var config = new Mock<IApplicationConfiguration>();
            config.Setup(o => o.GetGoogleAnalyticsCode(businessId)).Returns(googleAnalyticsCode);

            var googleAnalyticsViewComponent = new GoogleAnalyticsViewComponent(config.Object, new BusinessId(businessId));

            var view = await googleAnalyticsViewComponent.InvokeAsync() as ViewViewComponentResult;

            config.Verify(o => o.GetGoogleAnalyticsCode(businessId), Times.Once);
            var model = view.ViewData.Model as AppSetting;
            model.Should().Be(googleAnalyticsCode);
        }
    }
}
