using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class CarouselTagParserTest
    {
        private readonly CarouselTagParser _parser;

        public CarouselTagParserTest()
        {
            _parser = new CarouselTagParser();
        }

        [Fact]
        public void ShouldParseCarouselTagWithOneImage()
        {
            var tag = "![Frosty Twigs](Frosty_twigs_2.jpg)";
            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div></div>");
        }

        [Fact(Skip = "To Fix")]
        public void ShouldParseCarouselTagWithMoreThanImage()
        {
            var tag = "![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg)";
            var tag2 =
                "![UITEST:Image 1](//images.contentful.com/6cfgzlmcakf7/2FNKi2ZYcomigKGAqWiSyC/aec4f120903972fd6c842e84603c6a50/Flu_Jab.jpg), ![UITEST:Image 2](//images.contentful.com/6cfgzlmcakf7/3TsbpFmDewU4iuEAQU4IAo/afb6c155b46b9a0f196409ebb47cd4ff/Cllr_Foster_and_the_children_from_RECLAIM.jpg), ![UITEST:Image 3](//images.contentful.com/6cfgzlmcakf7/5NxhtngXYcEUUa6uOy6Ym6/a9263e313e538c45d04ce9a813880a2d/Art_gallery_image_2.jpg)";

            var response = _parser.Parse("{{CAROUSEL:" + tag + tag2 + "}}");
            response.Should().Be("<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div></div>");
        }

        [Fact]
        public void ShouldParseCarouselNoImages()
        {
            var tag = "";

            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'></div>");
        }

        [Fact(Skip = "To Fix")]
        public void ShouldParseCarouselTagWithMoreThanImageButNoCommaBeforeLastImage()
        {
            var tag = "![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg)![Hello](hello.jpg)";
            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div></div>");
        }

    }
}
