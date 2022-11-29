using System.Text.RegularExpressions;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Parsers
{
    public class InlineQuoteTagParser : IDynamicTagParser<InlineQuote>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<InlineQuoteTagParser> _logger;
        private readonly Regex _tagRegex = new Regex("{{QUOTE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        public InlineQuoteTagParser(IViewRender viewRenderer, ILogger<InlineQuoteTagParser> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
        }

        public string Parse(string body, IEnumerable<InlineQuote> dynamicContent)
        {
            var matches = _tagRegex.Matches(body);

            foreach (Match match in matches)
            {
                var tagSlug = match.Groups[1].Value;
                var inlineQuote = dynamicContent.FirstOrDefault(_ => _.Slug == tagSlug);

                if (inlineQuote != null)
                {
                    var renderedInlineQuote = _viewRenderer.Render("InlineQuote", inlineQuote);
                    body = _tagRegex.Replace(body, renderedInlineQuote, 1);
                }
            }

            return RemoveEmptyTags(body);
        }

        private string RemoveEmptyTags(string content)
        {
            return _tagRegex.Replace(content, string.Empty);
        }
    }
}
