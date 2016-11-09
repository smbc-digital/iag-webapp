using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class HomepageFactoryTest
    {
        private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new Mock<MarkdownWrapper>();

        private readonly HomepageFactory _homepageFactory;

        public HomepageFactoryTest()
        {
            _homepageFactory = new HomepageFactory(_markdownWrapperMock.Object);
        }

        [Fact]
        public void ItBuildsAHomepageWithProcessedBody()
        {
            var freeText = "free text";
            _markdownWrapperMock.Setup(o => o.ConvertToHtml(freeText)).Returns(freeText);

            var backgroundImage = "background image";

            var homepage = new Homepage(Enumerable.Empty<string>(), string.Empty, string.Empty, new List<SubItem>(), new List<Topic>(), new List<Alert>(), new List<CarouselContent>(), backgroundImage, freeText);

            var result = _homepageFactory.Build(homepage);

            result.FreeText.Should().Be(freeText);
            result.BackgroundImage.Should().Be(backgroundImage);

            _markdownWrapperMock.Verify(o => o.ConvertToHtml(freeText), Times.Once);
        }
    }
}