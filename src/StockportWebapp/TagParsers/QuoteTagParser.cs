namespace StockportWebapp.TagParsers;

public class QuoteTagParser : IDynamicTagParser<InlineQuote>
{
    private readonly IViewRender _viewRenderer;

    public QuoteTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{ProfileQuote:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string body, IEnumerable<InlineQuote> dynamicContent)
    {
        var matches = TagRegex.Matches(body);

        foreach (Match match in matches)
        {
            var tagSlug = match.Groups[1].Value;
            var quote = dynamicContent?.FirstOrDefault(_ => _.Slug.Equals(tagSlug));

            if (quote is not null)
            {
                var renderedInlineQuote = _viewRenderer.Render("Quote", quote);
                body = TagRegex.Replace(body, renderedInlineQuote, 1);
            }
        }

        return RemoveEmptyTags(body);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
}