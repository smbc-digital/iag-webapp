using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class TopicFactoryTest
    {
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly TopicFactory _topicFactory;
        private readonly Topic _topic;
        private const string Title = "title";
        private const string Slug = "slug";
        private const string Summary = "summary";
        private const string Teaser = "teaser";
        private const string MetaDescription = "meta desctiption";
        private const string Icon = "icon";
        private const string Image = "Image";
        private const string BackgroundImage = "backgroundimage.jpg";
        private readonly List<Crumb> _breadcrumbs;
        private readonly List<SubItem> _subItems;
        private readonly List<SubItem> _secondaryItems;
        private readonly List<SubItem> _tertiaryItems;

        public TopicFactoryTest()
        {
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _topicFactory = new TopicFactory(_tagParserContainer.Object, _markdownWrapper.Object);
            _breadcrumbs = new List<Crumb>();

            _subItems = new List<SubItem>();
            _secondaryItems = new List<SubItem>();
            _tertiaryItems = new List<SubItem>();

            _topic = new Topic("name", Slug, Summary, Teaser, MetaDescription, Icon, BackgroundImage, Image, _subItems, _secondaryItems, _tertiaryItems, _breadcrumbs,
                new List<Alert>(), false, "emailAlertsTopic", new EventBanner("title", "teaser", "icon", "link"), "expanding Link Title",
                new List<ExpandingLinkBox>(), "primary Item Title", Title, true, new CarouselContent("Title", "Teaser", "Image", "url"),
                "event Category")
            {
                Video = new()
            };

            _markdownWrapper.Setup(o => o.ConvertToHtml(Summary)).Returns(Summary);
            _tagParserContainer.Setup(o => o.ParseAll(Summary, Title, It.IsAny<bool>())).Returns(Summary);
        }


        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedTopic()
        {
            var result = _topicFactory.Build(_topic);

            result.Title.Should().Be(Title);
            result.NavigationLink.Should().Be("/topic/" + Slug);
            result.Summary.Should().Be(Summary);
            result.Teaser.Should().Be(Teaser);
            result.MetaDescription.Should().Be(MetaDescription);
            result.Icon.Should().Be(Icon);
            result.Image.Should().Be(Image);
            result.BackgroundImage.Should().Be(BackgroundImage);
            result.Breadcrumbs.ToList().Should().BeEquivalentTo(_breadcrumbs);
            result.SubItems.ToList().Should().BeEquivalentTo(_subItems);
            result.SecondaryItems.ToList().Should().BeEquivalentTo(_secondaryItems);
            result.TertiaryItems.ToList().Should().BeEquivalentTo(_tertiaryItems);
        }


        [Fact]
        public void ShouldProcessSummaryWithMarkdown()
        {
            _topicFactory.Build(_topic);
            _markdownWrapper.Verify(o => o.ConvertToHtml(Summary), Times.Once);
        }

        [Fact]
        public void ShouldProcessSummaryWithStaticTagParsing()
        {
            _topicFactory.Build(_topic);

            _tagParserContainer.Verify(o => o.ParseAll(Summary, Title, It.IsAny<bool>()), Times.Once);
        }
    }
}
