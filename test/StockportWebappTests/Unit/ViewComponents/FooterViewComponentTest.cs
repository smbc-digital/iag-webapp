using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewComponents;
using Xunit;

namespace StockportWebappTests.Unit.ViewComponents
{
    public class FooterViewComponentTest
    {
        private Mock<IRepository> _repository;
        private FooterViewComponent _footerViewComponent;

        public FooterViewComponentTest()
        {
            _repository = new Mock<IRepository>();
            _footerViewComponent = new FooterViewComponent(_repository.Object, new FeatureToggles() { DynamicFooter = true });
        }

        [Fact]
        public void ShouldReturnFooterAsModelInView()
        {
            var footer = new Footer("Title", "Slug", "Copyright", new List<SubItem>(), new List<SocialMediaLink>());
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(200, footer));

            var result = AsyncTestHelper.Resolve(_footerViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<Footer>();
            var footerModel = result.ViewData.Model as Footer;
            footerModel.Should().Be(footer);
        }

        [Fact]
        public void ShouldNotReturnAFooterInViewIfViewNotFound()
        {
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

            var result = AsyncTestHelper.Resolve(_footerViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeNull();
        }

        [Fact]
        public void ShouldShowOldFooterIfDynamicFooterFeatureToggleIsOff()
        {
            var repository = new Mock<IRepository>();
            var footerViewComponent = new FooterViewComponent(repository.Object, new FeatureToggles() { DynamicFooter = false });

            var result = AsyncTestHelper.Resolve(footerViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewName.Should().Be("Old");
        }
    }
}
