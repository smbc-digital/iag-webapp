using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.ContentFactory.Trivia;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using StockportWebappTests_Unit.Unit.TestBuilders;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class ShowcaseFactoryTest
    {
        private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
        private readonly Mock<ITriviaFactory> _triviaFactory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;

        public ShowcaseFactoryTest()
        {
            _tagParserContainer = new Mock<ISimpleTagParserContainer>();           
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _triviaFactory = new Mock<ITriviaFactory>();
        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedShowcase()
        {
            // Arrange
            var showcase = new ShowcaseBuilder()
                .Title("test title")
                .Slug("test_slug")
                .Teaser("test teaser")
                .MetaDescription("test metaDescription")
                .Subheading("test subheading")
                .HeroImageUrl("test-image-url.jpg")
                .Body("body")
                .Breadcrumbs(new List<Crumb> { new Crumb("test link", "test title", "test type") })
                .FeaturedItems(new List<SubItem>
                    {
                        new SubItem("slug","title", "icon", "teaser", "link", "image-url.jpg", new List<SubItem>())
                    })
                .Build();

            var _showcaseFactory = new ShowcaseFactory(_tagParserContainer.Object, _markdownWrapper.Object, _triviaFactory.Object);

            // Act
            var processedShowcase = _showcaseFactory.Build(showcase);

            // Assert   
            processedShowcase.Should().BeEquivalentTo(showcase);
        }
    }
}