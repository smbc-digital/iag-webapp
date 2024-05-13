namespace StockportWebapp.TagParsers;

public class InlineQuoteTagParser : IDynamicTagParser<InlineQuote>
{
    private readonly IViewRender _viewRenderer;

    public InlineQuoteTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new Regex("{{QUOTE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);


    public string Parse(string body, IEnumerable<InlineQuote> dynamicContent)
    {
        var matches = TagRegex.Matches(body);

        foreach (Match match in matches)
        {
            var tagSlug = match.Groups[1].Value;
            var inlineQuote = dynamicContent?.FirstOrDefault(_ => _.Slug == tagSlug);

            if (inlineQuote != null)
            {
                var renderedInlineQuote = _viewRenderer.Render("InlineQuote", inlineQuote);
                body = TagRegex.Replace(body, renderedInlineQuote, 1);
            }
        }

        return RemoveEmptyTags(body);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
}
