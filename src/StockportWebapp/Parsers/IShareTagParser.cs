using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class IShareTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        private Regex TagRegex => new Regex("{{ISHARE:(.*)}}", RegexOptions.Compiled);

        private string GenerateHtml(string tagData)
        {
            tagData.Replace("{{ISHARE:", string.Empty);
            tagData.Replace("}}", string.Empty);

            var splitTagData = tagData.Split(';');

            if (splitTagData.Length < 4)
            {
                return string.Empty;
            }

            var mapSource = splitTagData[0];
            var panelOne = splitTagData[1];
            var panelTwo = splitTagData[2];
            var layers = splitTagData[3];
            var title = splitTagData[4];

            var html = $"<iframe class='mapframe' title='{title}' src='/map?layers={layers}&source={mapSource}&panels={panelOne},{panelTwo}'></iframe>";

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