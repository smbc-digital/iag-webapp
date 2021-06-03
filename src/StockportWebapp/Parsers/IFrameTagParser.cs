using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class IFrameTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{IFRAME:(.*)}}", RegexOptions.Compiled);

        public string GenerateHtml(string tagData)
        {
            tagData.Replace("{{IFRAME:", string.Empty);
            tagData.Replace("}}", string.Empty);

            var ValidUrl = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

            if (!ValidUrl.IsMatch(tagData))
                return null;

            return $"<iframe class='mapframe' allowfullscreen src='{tagData}'></iframe>";
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