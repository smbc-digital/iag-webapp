using System.Text;
using FluentAssertions;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class VideoTagParserTest
    {
        private readonly VideoTagParser _parser;
        private readonly Mock<FeatureToggles> _featureToggles = new Mock<FeatureToggles>();

        public VideoTagParserTest()
        {
            _parser = new VideoTagParser(_featureToggles.Object);
        }

        [Fact]
        public void ShouldParseButoVideoTags()
        {
            var tag = "VideoTag";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
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
            outputHtml.Append("</div>");

            response.Should().Be(outputHtml.ToString());
        }

        [Fact]
        public void ShouldParseTwentyThreeVideoTags()
        {
            _featureToggles.Object.TwentyThreeVideo = true;
            var tag = "VideoId;VideoToken;Test";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
            outputHtml.Append("<iframe src=");
            outputHtml.Append("\"https://y84kj.videomarketingplatform.co/v.ihtml/player.html?token=VideoToken&source=embed&");
            outputHtml.Append("photo%5fid=VideoId\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
            outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
            outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen title=\"Test\"\">");
            outputHtml.Append("</iframe></div>");

            response.Should().Be(outputHtml.ToString());
        }
    }
}
