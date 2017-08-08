using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class IShareTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{ISHARE:(.*)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            tagData = tagData.Replace("{{ISHARE:", "");
            tagData = tagData.Replace("}}", "");

            var splitTagData = tagData.Split(';');

            string mapSource = splitTagData[0];
            string panelOne = splitTagData[1];
            string panelTwo = splitTagData[2];
            string layers = splitTagData[3];

            var html = $"<iframe class=\"mapframe\" src=\"/map?layers={layers}&source={mapSource}&panels={panelOne},{panelTwo}\"></iframe>";

            return html;

        }

        public IShareTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}