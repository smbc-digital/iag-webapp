using System.Linq;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class ButtonTagParser: ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{BUTTON:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);
        private const string ButtonClassStyle = "button button-primary button-outline button-partialrounded button-call-to-action";

        public string GenerateHtml(string tagData)
        {
            var commaSplitString = tagData.Split(new[] { ',' }, 2);
            var thereIsLinkText = commaSplitString.Count() == 2;
            var link = tagData;
            var title = tagData;
            if (thereIsLinkText)
            {
                link = commaSplitString[0].Trim();
                title = commaSplitString[1].Trim();
            }

            return $"<a class=\"{ButtonClassStyle}\" href=\"{link}\">{title}</a>";
        }

        public ButtonTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml,TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}