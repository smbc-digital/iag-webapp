using System.Text;
using FluentAssertions;
using StockportWebapp.Parsers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class VideoTagParserTests
    {
        private readonly VideoTagParser _parser;

        public VideoTagParserTests()
        {
            _parser = new VideoTagParser();
        }

        [Fact]
        public void ShouldParseTwentyThreeVideoTags()
        {
            var tag = "VideoId;VideoToken";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
            outputHtml.Append("<iframe  src=");
            outputHtml.Append("\"https://video.stockport.gov.uk/v.ihtml/player.html?token=VideoToken&source=embed&");
            outputHtml.Append("photo%5fid=VideoId\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
            outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
            outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen\">");
            outputHtml.Append("</iframe></div>");

            response.Should().Be(outputHtml.ToString());
        }

        [Fact]
        public void ShouldParseTwentyThreeVideoTags_With_Optional_IFrameTitle()
        {
            var tag = "VideoId;VideoToken;iframe title";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
            outputHtml.Append("<iframe title=\"iframe title\" src=");
            outputHtml.Append("\"https://video.stockport.gov.uk/v.ihtml/player.html?token=VideoToken&source=embed&");
            outputHtml.Append("photo%5fid=VideoId\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
            outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
            outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen\">");
            outputHtml.Append("</iframe></div>");

            response.Should().Be(outputHtml.ToString());
        }

        [Fact]
        public void ShouldParseTwentyThreeVideoTags_With_Optional_IFrameTitle_WhichHas_Different_Characters()
        {
            var tag = "VideoId;VideoToken;iframe Title's # this+";
            var response = _parser.Parse("{{VIDEO:" + tag + "}}");

            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
            outputHtml.Append($"<iframe title=\"iframe Title's # this+\" src=");
            outputHtml.Append("\"https://video.stockport.gov.uk/v.ihtml/player.html?token=VideoToken&source=embed&");
            outputHtml.Append("photo%5fid=VideoId\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
            outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
            outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen\">");
            outputHtml.Append("</iframe></div>");

            response.Should().Be(outputHtml.ToString());
        }
    }
}
