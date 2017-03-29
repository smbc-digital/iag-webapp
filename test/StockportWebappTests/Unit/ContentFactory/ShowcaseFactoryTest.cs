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
using StockportWebappTests.Unit.TestBuilders;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class ShowcaseFactoryTest
    {


        public ShowcaseFactoryTest()
        {

        }

        [Fact]
        public void ShouldSetTheCorrespondingFieldsForAProcessedShowcase()
        {
            // Arrange
            var showcase = new ShowcaseBuilder()
                .Title("test title")
                .Slug("test_slug")
                .Teaser("test teaser")
                .Subheading("test subheading")
                .HeroImageUrl("test-image-url.jpg")
                .Breadcrumbs(new List<Crumb> { new Crumb("test link", "test title", "test type") })
                .FeaturedItems(new List<SubItem>
        {
            new SubItem("slug","title", "icon", "teaser", "link", "image-url.jpg", new List<SubItem>())
        })
                .Build();

            var _showcaseFactory = new ShowcaseFactory();

            // Act
            var processedShowcase = _showcaseFactory.Build(showcase);

            // Assert   
            processedShowcase.ShouldBeEquivalentTo(showcase);
        }
    }
}