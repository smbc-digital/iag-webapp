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
    public class SectionFactoryTest
    {
        private readonly SectionFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser;
        private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
        private const string Title = "title";
        private const string Slug = "slug";
        private const string Body = "The new content of the body";
        private readonly List<Profile> _profiles = new List<Profile>();
        private readonly List<Document> _documents = new List<Document>();
        private readonly Section _section;

        public SectionFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();
            _profileTagParser = new Mock<IDynamicTagParser<Profile>> ();
            _documentTagParser = new Mock<IDynamicTagParser<Document>>();

            _factory = new SectionFactory(_tagParserContainer.Object, _profileTagParser.Object, _markdownWrapper.Object, _documentTagParser.Object);

            _section = new Section(Title, Slug, Body, _profiles, _documents);

            _markdownWrapper.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
            _tagParserContainer.Setup(o => o.ParseAll(Body)).Returns(Body);
            _profileTagParser.Setup(o => o.Parse(Body, _section.Profiles)).Returns(Body);
            _documentTagParser.Setup(o => o.Parse(Body, _section.Documents)).Returns(Body);

        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedSection()
        {
            var result = _factory.Build(_section);

            result.Body.Should().Be(Body);
            result.Title.Should().Be(Title);
            result.Slug.Should().Be(Slug);
            result.Profiles.Should().BeEquivalentTo(_profiles);
        }

        [Fact]
        public void ShouldProcessBodyWithMarkdown()
        {
            _factory.Build(_section);

            _markdownWrapper.Verify(o => o.ConvertToHtml(Body), Times.Once);
        }

        [Fact]
        public void ShouldProcessBodyWithTagParsing()
        {
            _factory.Build(_section);

            _tagParserContainer.Verify(o => o.ParseAll(Body), Times.Once);
        }

        [Fact]
        public void ShouldProcessBodyWithProfileTagParsing()
        {
            _factory.Build(_section);

            _profileTagParser.Verify(o => o.Parse(Body, _section.Profiles), Times.Once);
        }
    }
}
