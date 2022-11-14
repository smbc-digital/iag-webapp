using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class FormBuilderTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new("{{FORM:(.*)}}", RegexOptions.Compiled);

        public string GenerateHtml(string tagData)
        {
            tagData.Replace("{{FORM:", string.Empty);
            tagData.Replace("}}", string.Empty);

            var ValidUrl = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

            var splitTagData = tagData.Split(";");
            if (!ValidUrl.IsMatch(splitTagData[0]))
                return null;

            var iFrameTitle = string.Empty;

            if (splitTagData.Length > 1)
                iFrameTitle = $"title=\"{splitTagData[1]}\"";

            return $"<iframe sandbox='allow-forms allow-scripts allow-top-navigation-by-user-activation allow-same-origin' {iFrameTitle} class='mapframe' allowfullscreen src='{splitTagData[0]}'></iframe>";
        }
        
        public FormBuilderTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}