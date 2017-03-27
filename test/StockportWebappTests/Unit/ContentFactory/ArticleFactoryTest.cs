using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using Xunit;
using Helper = StockportWebappTests.TestHelper;
using Moq;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class ArticleFactoryTest
    {
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly ArticleFactory _articleFactory;
        private readonly Mock<ISectionFactory> _sectionFactory;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
        private readonly Mock<IDynamicTagParser<Alert>> _alertsInlineTagParser;
        private readonly List<Profile> _emptyProfiles = new List<Profile>();
        private readonly List<Document> _emptyDocuments = new List<Document>();
        private const string Title = "title";
        private const string Slug = "slug";
        private const string Body = "body";
        private const string Teaser = "teaser";
        private readonly Section _sectionOne;
        private readonly ProcessedSection _processedSectionOne;
        private readonly Section _sectionTwo;
        private readonly ProcessedSection _processedSectionTwo;
        private const string Icon = "icon";
        private const string BackgroundImage = "backgroundImage";
        private const string Image = "image";
        private readonly List<Crumb> _breadcrumbs;
        private readonly Article _article;
        private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser;
        private const bool _liveChatVisible = true;
        private readonly LiveChat _liveChat = new LiveChat("Title","text");
        private readonly List<Alert> _emptyAlertsInline = new List<Alert>();

        public ArticleFactoryTest()
        {
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _profileTagParser = new Mock<IDynamicTagParser<Profile>>();
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _sectionFactory = new Mock<ISectionFactory>();
            _documentTagParser= new Mock<IDynamicTagParser<Document>>();
            _alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
            _articleFactory = new ArticleFactory(_tagParserContainer.Object, _profileTagParser.Object, _sectionFactory.Object, _markdownWrapper.Object, _documentTagParser.Object, _alertsInlineTagParser.Object);

            _sectionOne = new Section(Helper.AnyString, "id-1", Helper.AnyString, _emptyProfiles, _emptyDocuments, _emptyAlertsInline);
            _processedSectionOne = new ProcessedSection(Helper.AnyString, "id-1", Helper.AnyString, _emptyProfiles, _emptyDocuments, _emptyAlertsInline);
            _sectionTwo = new Section(Helper.AnyString, "id-1", Helper.AnyString, _emptyProfiles, _emptyDocuments, _emptyAlertsInline);
            _processedSectionTwo = new ProcessedSection(Helper.AnyString, "id-1", Helper.AnyString, _emptyProfiles, _emptyDocuments, _emptyAlertsInline);
            var sections = new List<Section>() { _sectionOne, _sectionTwo };
            _breadcrumbs = new List<Crumb>();
            
             _article = new Article(Title, Slug, Body, Teaser, sections, Icon, BackgroundImage, Image, _breadcrumbs, _emptyProfiles, _emptyDocuments, _liveChatVisible, _liveChat, _emptyAlertsInline);

            _sectionFactory.Setup(o => o.Build(_sectionOne,_article.Title)).Returns(_processedSectionOne);
            _sectionFactory.Setup(o => o.Build(_sectionTwo,_article.Title)).Returns(_processedSectionTwo);

            _markdownWrapper.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
            _tagParserContainer.Setup(o => o.ParseAll(Body, It.IsAny<string>())).Returns(Body);
            _profileTagParser.Setup(o => o.Parse(Body, _emptyProfiles)).Returns(Body);
            _documentTagParser.Setup(o => o.Parse(Body, _emptyDocuments)).Returns(Body);
            _alertsInlineTagParser.Setup(o => o.Parse(Body, _emptyAlertsInline)).Returns(Body);
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedArticle()
        {
            var result = _articleFactory.Build(_article);

            result.Title.Should().Be(Title);
            result.NavigationLink.Should().Be("/" + Slug);
            result.Body.Should().Be(Body);
            result.Teaser.Should().Be(Teaser);
            result.Sections.Should().HaveCount(2);
            result.Sections.ToList()[0].Should().Be(_processedSectionOne);
            result.Sections.ToList()[1].Should().Be(_processedSectionTwo);
            result.Icon.Should().Be(Icon);
            result.BackgroundImage.Should().Be(BackgroundImage);
            result.Image.Should().Be(Image);
            result.Breadcrumbs.ToList().Should().BeEquivalentTo(_breadcrumbs);
            result.LiveChatVisible.Should().Be(true);
            result.LiveChat.Title.Should().Be("Title");
            result.LiveChat.Text.Should().Be("text");
        }

        [Fact]
        public void ShouldProcessAllSectionsInArticle()
        {
            _articleFactory.Build(_article);

            _sectionFactory.Verify(o => o.Build(_sectionOne,_article.Title), Times.Once);
            _sectionFactory.Verify(o => o.Build(_sectionTwo,_article.Title), Times.Once);
        }

        [Fact]
        public void ShouldProcessBodyWithMarkdown()
        {
            _articleFactory.Build(_article);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Body), Times.Once);
        }

        [Fact]
        public void ShouldProcessBodyWithStaticTagParsing()
        {
            _articleFactory.Build(_article);

            _tagParserContainer.Verify(o => o.ParseAll(Body, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldProcessBodyWithProfileTagParsing()
        {
            _articleFactory.Build(_article);

            _profileTagParser.Verify(o => o.Parse(Body, _article.Profiles), Times.Once);
        }

        [Fact]
        public void ShouldPassTitleToParserWhenBuilding()
        {
            _articleFactory.Build(_article);

            _tagParserContainer.Verify(o => o.ParseAll(Body, _article.Title), Times.Once);
        }
    }
}