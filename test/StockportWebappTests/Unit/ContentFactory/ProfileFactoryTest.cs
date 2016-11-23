using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class ProfileFactoryTest
    {
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer = new Mock<ISimpleTagParserContainer>();
        private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new Mock<MarkdownWrapper>();

        private readonly ProfileFactory _profileFactory;

        public ProfileFactoryTest()
        {
            _profileFactory = new ProfileFactory(_tagParserContainer.Object, _markdownWrapperMock.Object);
        }

        [Fact]
        public void ItBuildsAProfileWithProcessedBody()
        {
            var body = "body";
            _tagParserContainer.Setup(o => o.ParseAll(body, It.IsAny<string>())).Returns(body);
            _markdownWrapperMock.Setup(o => o.ConvertToHtml(body)).Returns(body);

            var crumb = new Crumb("title", "slug", "type");
            var slug = "slug";
            var teaser = "teaser";
            var subtitle = "subtitle";
            var title = "title";
            var icon = "icon";
            var type = "type";
            var image = "image";
            var backgroundImage = "backgroundImage";
            var breacrumbs = new List<Crumb> { crumb, crumb };

            var profile = new Profile(type, title, slug, subtitle, teaser, image, body, backgroundImage, icon, breacrumbs);

            var result = _profileFactory.Build(profile);

            result.Body.Should().Be(body);
            result.Slug.Should().Be(slug);
            result.Teaser.Should().Be(teaser);
            result.Subtitle.Should().Be(subtitle);
            result.Title.Should().Be(title);
            result.Icon.Should().Be(icon);
            result.Type.Should().Be(type);
            result.Image.Should().Be(image);
            result.BackgroundImage.Should().Be(backgroundImage);
            result.Breadcrumbs.Should().HaveCount(2);

            _tagParserContainer.Verify(o => o.ParseAll(body, title), Times.Once);
            _markdownWrapperMock.Verify(o => o.ConvertToHtml(body), Times.Once);
        }

       }
}