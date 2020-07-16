using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class IFrameTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        private Regex TagRegex => new Regex("{{IFRAME:(.*)}}", RegexOptions.Compiled);

        private string GenerateHtml(string tagData)
        {
            tagData.Replace("{{IFRAME:", string.Empty);
            tagData.Replace("}}", string.Empty);

            var splitTagData = tagData.Split(';');

            if (splitTagData.Length < 4)
            {
                return string.Empty;
            }

            var url = splitTagData[0];
            var title = splitTagData[1];

            var validUrl = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

            return validUrl.IsMatch(url)
                ? $"<iframe class='mapframe' title='{title}' allowfullscreen src='{tagData}'></iframe>"
                : null;
        }

        public IFrameTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}