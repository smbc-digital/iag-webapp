using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class VideoTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{VIDEO:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            return $"<div id=\"buto_{tagData}\"></div><script>(function(d,config){{var script=d.createElement(\"script\");script.setAttribute(\"async\",true);var data=JSON.stringify(config);script.src=\"//js.buto.tv/video/\"+data;var s=d.getElementsByTagName(\"script\")[0];s.parentNode.insertBefore(script,s)}})(document,{{\"object_id\":\"{tagData}\"}})</script>";
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