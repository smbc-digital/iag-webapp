using System.Text;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class VideoTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{VIDEO:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            var outputHtml = new StringBuilder();

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

            //outputHtml.Append($"<div id=\"buto_{tagData}\"></div>");
            //outputHtml.Append("<script>");
            //outputHtml.Append("var globalButoIds = globalButoIds || [];");
            //outputHtml.Append("(");
            //outputHtml.Append("function (d, config) {");
            //outputHtml.Append("var data = JSON.stringify(config);");
            //outputHtml.Append("globalButoIds.push(\"//js.buto.tv/video/\" + data);");
            //outputHtml.Append($"}}(document, {{ \"object_id\": \"{tagData}\" }})");
            //outputHtml.Append(")");
            //outputHtml.Append("</script>");

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