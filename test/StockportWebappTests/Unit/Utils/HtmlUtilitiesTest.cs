using AngleSharp.Parser.Html;
using FluentAssertions;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Utils
{
    public class HtmlUtilitiesTest
    {
        private readonly HtmlUtilities _htmlUtilities;
        private const string HtmlStart = "<html><head></head><body>";
        private const string HtmlEnd = "</body></html>";
        private const string ScriptHtmlStart = "<html><head>";
        private const string ScriptHtmlEnd = "</head><body></body></html>";

        public HtmlUtilitiesTest()
        {
            var htmlParser = new HtmlParser();
            _htmlUtilities = new HtmlUtilities(htmlParser);
        }

        [Fact]
        public void ItShouldConvertRelativeUrltoAbsolute()
        {
            var result = _htmlUtilities.ConvertRelativeUrltoAbsolute("<a href=\"/test/\">Test link</a>", "http://unittestlink.xy");

            result.Should().Be(string.Concat(HtmlStart, "<a href=\"http://unittestlink.xy/test/\">Test link</a>", HtmlEnd));
        }

        [Fact]
        public void ItShouldNotConvertAbsoluteLinks()
        {
            var result = _htmlUtilities.ConvertRelativeUrltoAbsolute("<a href=\"http://absolute.url/test/\">Test link</a>", "http://unittestlink.xy");

            result.Should().Be(string.Concat(HtmlStart, "<a href=\"http://absolute.url/test/\">Test link</a>", HtmlEnd));
        }

        [Fact]
        public void ItShouldConvertRelativeUrltoAbsoluteForSrc()
        {
            var result = _htmlUtilities.ConvertRelativeUrltoAbsolute("<img src=\"/img/test.jpg\">", "http://unittestlink.xy");

            result.Should().Be(string.Concat(HtmlStart, "<img src=\"http://unittestlink.xy/img/test.jpg\">", HtmlEnd));
        }

        [Fact]
        public void ItShouldNotConvertRelativeUrltoAbsoluteForSrcWithAbsoluteLinks()
        {
            var result = _htmlUtilities.ConvertRelativeUrltoAbsolute("<img src=\"http://absolute.url/img/test.jpg\">", "http://unittestlink.xy");

            result.Should().Be(string.Concat(HtmlStart, "<img src=\"http://absolute.url/img/test.jpg\">", HtmlEnd));
        }

        [Fact]
        public void ItShouldConvertRelativeUrltoAbsoluteForDataMainWithAbsoluteLinks()
        {
            var result = _htmlUtilities.ConvertRelativeUrltoAbsolute("<script data-main=\"/url/file.js\">", "http://unittestlink.xy");

            result.Should().Be(string.Concat(ScriptHtmlStart, "<script data-main=\"http://unittestlink.xy/url/file.js\"></script>", ScriptHtmlEnd));
        }
    }
}
