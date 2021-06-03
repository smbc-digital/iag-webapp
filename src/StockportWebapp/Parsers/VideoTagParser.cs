using System.Text;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class VideoTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{VIDEO:([0-9aA-zZ]*;?[0-9aA-zZ]*)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            var videoData = tagData.Split(';');
            var outputHtml = new StringBuilder();

            outputHtml.Append("<div class=\"video-wrapper\">");
            outputHtml.Append("<iframe src=");
            outputHtml.Append($"\"https://video.stockport.gov.uk/v.ihtml/player.html?token={videoData[1]}&source=embed&");
            outputHtml.Append($"photo%5fid={videoData[0]}\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
            outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
            outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen\">");
            outputHtml.Append("</iframe></div>");

            return outputHtml.ToString();
        }

        public VideoTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}