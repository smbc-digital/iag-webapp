using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests.Unit.Parsers
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

        [Fact]
        public void ShouldParseCarouselTagWithMoreThanImage()
        {
            var tag = "![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg)";
            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div></div>");
        }

        [Fact]
        public void ShouldParseCarouselNoImages()
        {
            var tag = "";
            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'></div>");
        }

        [Fact]
        public void ShouldParseCarouselTagWithMoreThanImageButNoCommaBeforeLastImage()
        {
            var tag = "![Frosty Twigs](Frosty_twigs_2.jpg),![Autumn Leaves](autumn_leaves.jpg)![Hello](hello.jpg)";
            var response = _parser.Parse("{{CAROUSEL:" + tag + "}}");
            response.Should().Be("<div class='carousel'><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(Frosty_twigs_2.jpg);\" title=\"Frosty Twigs\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Frosty Twigs</p></div></div><div class=\"carousel-image stockport-carousel\" style=\"background-image:url(autumn_leaves.jpg);\" title=\"Autumn Leaves\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">Autumn Leaves</p></div></div></div>");
        }

    }
}
