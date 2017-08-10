using System.Text;
using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests.Unit.Parsers
{
    public class VideoTagParserTest
    {
        private readonly VideoTagParser _parser;

        public VideoTagParserTest()
        {
            _parser = new VideoTagParser();
        }

        [Fact]
        public void ShouldParseVideoTags()
        {
            var tag = "a-video-tag";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append($"<div id=\"buto_{tag}\"></div>");
            outputHtml.Append("<script>");
            outputHtml.Append("var globalButoIds = globalButoIds || [];");
            outputHtml.Append("console.log('test');");
            outputHtml.Append("(");
            outputHtml.Append("function (d, config) {");
            outputHtml.Append("var data = JSON.stringify(config);");
            outputHtml.Append("globalButoIds.push(\"//js.buto.tv/video/\" + data);");
            outputHtml.Append($"}}(document, {{ \"object_id\": \"{ tag}\" }})");
            outputHtml.Append(")");
            outputHtml.Append("</script>");

            response.Should().Be(outputHtml.ToString());
        }
    }
}
