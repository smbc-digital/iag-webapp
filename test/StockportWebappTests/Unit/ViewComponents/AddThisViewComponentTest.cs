using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.ViewComponents;
using Xunit;

namespace StockportWebappTests.Unit.ViewComponents
{
    public class AddThisViewComponentTest
    {
        [Fact]
        public void ShouldReturnTheAddThisShareId()
        {
            var businessId = "businessID";
            var sharedIdSetting = AppSetting.GetAppSetting("an id");
            var config = new Mock<IApplicationConfiguration>();
            config.Setup(o => o.GetAddThisShareId(businessId)).Returns(sharedIdSetting);

            var addThisViewComponent = new AddThisViewComponent(config.Object, new BusinessId(businessId));

            var result = AsyncTestHelper.Resolve(addThisViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<AppSetting>();

            var setting = result.ViewData.Model as AppSetting;
            setting.Should().Be(sharedIdSetting);
        }
    }
}
