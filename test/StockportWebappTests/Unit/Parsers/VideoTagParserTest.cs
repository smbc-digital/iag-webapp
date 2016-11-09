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

            response.Should().Be($"<div id=\"buto_{tag}\"></div><script>(function(d,config){{var script=d.createElement(\"script\");script.setAttribute(\"async\",true);var data=JSON.stringify(config);script.src=\"//js.buto.tv/video/\"+data;var s=d.getElementsByTagName(\"script\")[0];s.parentNode.insertBefore(script,s)}})(document,{{\"object_id\":\"{tag}\"}})</script>");
        }
    }
}
