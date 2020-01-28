using System.Text;
using System.Text.RegularExpressions;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Parsers
{
    public class VideoTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        private readonly FeatureToggles _featureToggles; 
        protected Regex TagRegex => new Regex("{{VIDEO:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            var outputHtml = new StringBuilder();

            if (_featureToggles.TwentyThreeVideo)
            {
                outputHtml.Append("<div class=\"video-wrapper\">");
                outputHtml.Append("<iframe src=");
                outputHtml.Append("\"https://y84kj.videomarketingplatform.co/v.ihtml/player.html?source=embed&");
                outputHtml.Append($"photo%5fid={tagData}\" style=\"width:100%; height:100%; position:absolute; top:0; left:0;\" ");
                outputHtml.Append("frameborder=\"0\" border=\"0\" scrolling=\"no\" allowfullscreen=\"1\" mozallowfullscreen=\"1\" ");
                outputHtml.Append("webkitallowfullscreen=\"1\" allow=\"autoplay; fullscreen\">");
                outputHtml.Append("</iframe></div>");
            }
            else
            {
                outputHtml.Append("<div class=\"video-wrapper\">");
                outputHtml.Append($"<div id=\"buto_{tagData}\"></div>");
                outputHtml.Append("<script>");
                outputHtml.Append("(function(d, config) {");
                outputHtml.Append("var script = d.createElement(\"script\");");
                outputHtml.Append("script.setAttribute(\"async\", true);");
                outputHtml.Append("var data = JSON.stringify(config);");
                outputHtml.Append("script.src = \"//js.buto.tv/video/\" + encodeURIComponent(data);");
                outputHtml.Append("var s = d.getElementsByTagName(\"script\")[0];");
                outputHtml.Append("s.parentNode.insertBefore(script, s)");
                outputHtml.Append($"}})(document, {{\"object_id\":\"{tagData}\", \"width\": \"100%\", \"height\": \"100%\"}})");
                outputHtml.Append("</script>");
                outputHtml.Append("</div>");
            }

            return outputHtml.ToString();
        }

        public VideoTagParser(FeatureToggles featureToggles)
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
            _featureToggles = featureToggles;
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}