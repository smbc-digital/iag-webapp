using System.Collections.Generic;
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
    public class SemanticFooterViewComponentTest
    {
        private readonly Mock<IRepository> _repository;
        private readonly SemanticFooterViewComponent _semanticFooterViewComponent;
        private readonly Mock<ILogger<SemanticFooterViewComponent>> _logger;

        public SemanticFooterViewComponentTest()
        {
            _repository = new Mock<IRepository>();
            _logger = new Mock<ILogger<SemanticFooterViewComponent>>();
            _semanticFooterViewComponent = new SemanticFooterViewComponent(_repository.Object, _logger.Object);
        }

        [Fact]
        public void ShouldReturnFooterAsModelInView()
        {
            var footer = new Footer("Title", "Slug", "Copyright", new List<SubItem>(), new List<SocialMediaLink>());
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(200, footer));

            var result = AsyncTestHelper.Resolve(_semanticFooterViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<Footer>();
            var footerModel = result.ViewData.Model as Footer;
            footerModel.Should().Be(footer);

            LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
        }

        [Fact]
        public void ShouldNotReturnAFooterInViewIfViewNotFound()
        {
            _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

            var result = AsyncTestHelper.Resolve(_semanticFooterViewComponent.InvokeAsync()) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeNull();
            Assert.Equal("NoFooterFound", result.ViewName);
            LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
        }
    }
}
