using System.Text;
using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
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
            outputHtml.Append("(function(d, config) {");
            outputHtml.Append("var script = d.createElement(\"script\");");
            outputHtml.Append("script.setAttribute(\"async\", true);");
            outputHtml.Append("var data = JSON.stringify(config);");
            outputHtml.Append("script.src = \"//js.buto.tv/video/\" + encodeURIComponent(data);");
            outputHtml.Append("var s = d.getElementsByTagName(\"script\")[0];");
            outputHtml.Append("s.parentNode.insertBefore(script, s)");
            outputHtml.Append($"}})(document, {{\"object_id\":\"{tag}\", \"width\": \"100%\", \"height\": \"100%\"}})");
            outputHtml.Append("</script>");

            //outputHtml.Append($"<div id=\"buto_{tag}\"></div>");
            //outputHtml.Append("<script>");
            //outputHtml.Append("var globalButoIds = globalButoIds || [];");
            //outputHtml.Append("(");
            //outputHtml.Append("function (d, config) {");
            //outputHtml.Append("var data = JSON.stringify(config);");
            //outputHtml.Append("globalButoIds.push(\"//js.buto.tv/video/\" + data);");
            //outputHtml.Append($"}}(document, {{ \"object_id\": \"{ tag}\" }})");
            //outputHtml.Append(")");
            //outputHtml.Append("</script>");

            response.Should().Be(outputHtml.ToString());
        }
    }
}
