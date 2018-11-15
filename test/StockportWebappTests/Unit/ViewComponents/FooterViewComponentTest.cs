using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewComponents;
using Xunit;

namespace StockportWebappTests.Unit.ViewComponents
{
    public class FooterViewComponentTest
    {
        private readonly Mock<IRepository> _repository;
        private readonly FooterViewComponent _footerViewComponent;
        private readonly Mock<ILogger<FooterViewComponent>> _logger;

        public FooterViewComponentTest()
        {
            _repository = new Mock<IRepository>();
            _logger = new Mock<ILogger<FooterViewComponent>>();
            _footerViewComponent = new FooterViewComponent(_repository.Object, _logger.Object);
        }

        [Fact]
        public async Task ShouldReturnFooterAsModelInView()
        {
            var footer = new Footer("Title", "Slug", "Copyright", new List<SubItem>(), new List<SocialMediaLink>());
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(200, footer));

            var result = await _footerViewComponent.InvokeAsync() as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<Footer>();
            var footerModel = result.ViewData.Model as Footer;
            footerModel.Should().Be(footer);

            LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
        }

        [Fact]
        public async Task ShouldNotReturnAFooterInViewIfViewNotFound()
        {
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

            var result = await _footerViewComponent.InvokeAsync() as ViewViewComponentResult;

            result.ViewData.Model.Should().BeNull();

            LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
        }
    }
}
