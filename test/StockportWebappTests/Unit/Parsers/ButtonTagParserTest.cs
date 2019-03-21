using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class ButtonTagParserTest
    {
        private readonly ButtonTagParser _buttonParser;

        public ButtonTagParserTest()
        {
            _buttonParser = new ButtonTagParser();
        }

        [Fact]
        public void ItParsesTheButtonTagAndReplacesItWithHtmlButtonWithTitleIfTitleIsGiven()
        {
            const string body = "{{BUTTON: http://www.example.com, Click here!}}";
            var expectedHtmlData = HtmlButton("http://www.example.com", "Click here!");

            var result = _buttonParser.Parse(body);

            result.Should().Be(expectedHtmlData);
        }

        [Fact]
        public void ItParsesTheButtonTagWithRelativeLinks()
        {
            const string body = "{{BUTTON: /relative_link/to_the_website/, Click here!}}";
            var expectedHtmlData = HtmlButton("/relative_link/to_the_website/", "Click here!");

            var result = _buttonParser.Parse(body);

            result.Should().Be(expectedHtmlData);
        }

        [Fact]
        public void ItParsesTheButtonTagAndReplacesItWithHtmlButtonWithLinkAsTitleIfNoTitleIsGiven()
        {
            const string body = "{{BUTTON: http://www.no-title-here.com}}";
            var expectedHtmlData = HtmlButton("http://www.no-title-here.com");

            var result = _buttonParser.Parse(body);

            result.Should().Be(expectedHtmlData);
        }

        [Fact]
        public void ItParsesAllButtonTagsAndReplacesThemWithHtmlButtons()
        {
            const string body = "{{BUTTON: http://www.example1.com, Click}} body text {{BUTTON: http://www.example2.com, Here}}";
            var expectedHtmlData = $"{HtmlButton("http://www.example1.com", "Click")} body text {HtmlButton("http://www.example2.com", "Here")}";

            var result = _buttonParser.Parse(body);
             
            result.Should().Be(expectedHtmlData);
        }

        private static string HtmlButton(string link)
        {
            return HtmlButton(link, link);
        }

        private static string HtmlButton(string link, string title)
        {
            const string buttonClassStyle = "button button-primary button-outline button-partialrounded button-call-to-action";
            return $"<a class=\"{buttonClassStyle}\" href=\"{link}\">{title}</a>";
        }
    }
}
