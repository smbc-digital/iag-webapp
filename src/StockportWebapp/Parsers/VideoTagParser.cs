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
            outputHtml.Append("var globalButoIds = globalButoIds || [];");
            outputHtml.Append("console.log('test');");
            outputHtml.Append("(");
            outputHtml.Append("function (d, config) {");
            outputHtml.Append("var data = JSON.stringify(config);");
            outputHtml.Append("globalButoIds.push(\"//js.buto.tv/video/\" + data);");
            outputHtml.Append($"}}(document, {{ \"object_id\": \"{ tagData}\" }})");
            outputHtml.Append(")");
            outputHtml.Append("</script>");

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