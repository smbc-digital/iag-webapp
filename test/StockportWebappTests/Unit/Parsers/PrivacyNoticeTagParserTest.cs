using FluentAssertions;
using Moq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class PrivacyNoticeTagParserTest
    {
        private readonly Mock<IViewRender> _viewRenderer;
        private readonly PrivacyNoticeTagParser _privacyNoticeTagParser;
        private readonly Mock<ILogger<PrivacyNotice>> _mockLogger;

        public PrivacyNoticeTagParserTest()
        {
            _viewRenderer = new Mock<IViewRender>();
            _mockLogger = new Mock<ILogger<PrivacyNotice>>();
            _privacyNoticeTagParser = new PrivacyNoticeTagParser(_viewRenderer.Object, _mockLogger.Object);
        }

        [Fact]
        public void Parse_ShouldRenderIfPrivacyExists()
        {
            //arrange
            var content = "{{PrivacyNotice:title}}";
            var privacyNotice = new List<PrivacyNotice>()
            {
                new PrivacyNotice()
                {
                    Title = "title",
                    Slug = "slug"
                }
            };
            var renderResult = "";
            _viewRenderer.Setup(o => o.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(renderResult);
            //act
            _privacyNoticeTagParser.Parse(content, privacyNotice);
            //assert
            _viewRenderer.Verify(o => o.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Once);
        }

        [Fact]
        public void Parse_ShouldNotRenderIfPrivacyNoticeDoesNotExist()
        {
            //arrange
            var content = "{{PrivacyNotice:category}}";
            var privacyNotice = new List<PrivacyNotice>()
            {
                new PrivacyNotice()
                {
                    Title = "title",
                    Slug = "slug"
                }
            };
            var renderResult = "";
            _viewRenderer.Setup(o => o.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(renderResult);
            //act
            _privacyNoticeTagParser.Parse(content, privacyNotice);
            //assert
            _viewRenderer.Verify(o => o.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Never);
        }

        [Fact]
        public void Parse_ShouldReplaceContentIfPrivacyNoticeExists()
        {
            //arrange
            var content = "{{PrivacyNotice:title}}";
            var privacyNotice = new List<PrivacyNotice>()
            {
                new PrivacyNotice()
                {
                    Title = "title",
                    Slug = "slug"
                }
            };
            var renderResult = "<h1>title</h1>";
            _viewRenderer.Setup(o => o.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(renderResult);
            //act
            var result = _privacyNoticeTagParser.Parse(content, privacyNotice);
            //assert
            result.Should().Be("<h1>title</h1>");
        }
    }
}
