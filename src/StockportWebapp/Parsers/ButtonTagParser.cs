using System.Linq;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class ButtonTagParser: ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{BUTTON:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);
        private const string ButtonClassStyle = "button button-outline button-partialrounded button-call-to-action";

        protected string GenerateHtml(string tagData)
        {
            var commaSplitString = tagData.Split(',');
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

        public string Parse(string body)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}