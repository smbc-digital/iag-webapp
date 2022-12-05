using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class GroupHomepageFactoryTest
    {
        private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new Mock<MarkdownWrapper>();
        private readonly Mock<ISimpleTagParserContainer> _simpleTagParserContainerMock = new Mock<ISimpleTagParserContainer>();
        private readonly GroupHomepageFactory _groupHomepageFactory;
        private const string Title = "title";
        private string Body = "body";
        private GroupHomepage _groupHomepage;


        public GroupHomepageFactoryTest()
        {
            _groupHomepageFactory = new GroupHomepageFactory(_simpleTagParserContainerMock.Object, _markdownWrapperMock.Object);


            _groupHomepage = new GroupHomepage
            {
                Title = Title,
                BackgroundImage = "background image",
                FeaturedGroupsHeading = string.Empty,
                FeaturedGroups = new List<Group>(),
                FeaturedGroupsCategory = new GroupCategory(),
                FeaturedGroupsSubCategory = new GroupSubCategory(),
                Alerts = new List<Alert>(),
                Body = "body",
                SecondaryBody = "secondary body",
                EventBanner = new EventBanner("title", "teaser", "icon", "link")
            };

            _markdownWrapperMock.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
            _simpleTagParserContainerMock.Setup(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>())).Returns(Body);
        }

        [Fact]
        public void ItBuildsAGroupsHomepageWithProcessedBody()
        {
            var result = _groupHomepageFactory.Build(_groupHomepage);

            result.Title.Should().Be(_groupHomepage.Title);
            result.BackgroundImage.Should().Be(_groupHomepage.BackgroundImage);
            result.Body.Should().Be(_groupHomepage.Body);
        }

        [Fact]
        public void ShouldParseAllOfBody()
        {
            var result = _groupHomepageFactory.Build(_groupHomepage);

            _simpleTagParserContainerMock.Verify(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            _markdownWrapperMock.Verify(o => o.ConvertToHtml(Body), Times.Once);
        }

    }
}