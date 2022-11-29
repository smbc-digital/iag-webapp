using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using StockportWebappTests_Unit.Builders;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class GroupFactoryTest
    {
        private readonly GroupFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
        private readonly Group _group;

        public GroupFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();
            _factory = new GroupFactory(_tagParserContainer.Object, _markdownWrapper.Object, _documentTagParser.Object);

            _group = new GroupBuilder().Build();

            _tagParserContainer.Setup(o => o.ParseAll(_group.Description, It.IsAny<string>(), It.IsAny<bool>())).Returns(_group.Description);
            _tagParserContainer.Setup(o => o.ParseAll(_group.AdditionalInformation, It.IsAny<string>(), It.IsAny<bool>())).Returns(_group.AdditionalInformation);
            _markdownWrapper.Setup(o => o.ConvertToHtml(_group.Description)).Returns(_group.Description);
            _markdownWrapper.Setup(o => o.ConvertToHtml(_group.AdditionalInformation)).Returns(_group.AdditionalInformation);
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedGroup()
        {
            var result = _factory.Build(_group);

            result.Name.Should().Be(_group.Name);
            result.MetaDescription.Should().Be(_group.MetaDescription);
            result.Description.Should().Be(_group.Description);
            result.Slug.Should().Be(_group.Slug);
            result.Address.Should().Be(_group.Address);
            result.Website.Should().Be(_group.Website);
            result.Email.Should().Be(_group.Email);
            result.PhoneNumber.Should().Be(_group.PhoneNumber);
            result.ImageUrl.Should().Be(_group.ImageUrl);
            result.MapDetails.MapPosition.Lat.Should().Be(_group.MapPosition.Lat);
            result.MapDetails.MapPosition.Lon.Should().Be(_group.MapPosition.Lon);
            result.ThumbnailImageUrl.Should().Be(_group.ThumbnailImageUrl);
            result.Twitter.Should().Be(_group.Twitter);
            result.Facebook.Should().Be(_group.Facebook);
            result.AdditionalInformation.Should().Be(_group.AdditionalInformation);
        }

        [Fact]
        public void ShouldProcessDescriptionWithMarkdown()
        {
            _factory.Build(_group);

            _markdownWrapper.Verify(o => o.ConvertToHtml(_group.Description), Times.Once);
        }

        [Fact]
        public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
        {
            _factory.Build(_group);

            _tagParserContainer.Verify(o => o.ParseAll(_group.Description, _group.Name, It.IsAny<bool>()), Times.Once);
        }
    }
}
