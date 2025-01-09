namespace StockportWebapp.TagParsers;

public class InlineQuoteTagParser(IViewRender viewRenderer) : IDynamicTagParser<InlineQuote>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{QUOTE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string body, IEnumerable<InlineQuote> dynamicContent, bool redesigned = false)
    {
        MatchCollection matches = TagRegex.Matches(body);

        foreach (Match match in matches)
        {
            string tagSlug = match.Groups[1].Value;
            InlineQuote inlineQuote = dynamicContent?.FirstOrDefault(_ => _.Slug.Equals(tagSlug));

            if (inlineQuote is not null)
            {
                string renderedInlineQuote = _viewRenderer.Render("InlineQuote", inlineQuote);
                body = TagRegex.Replace(body, renderedInlineQuote, 1);
            }
        }

        return RemoveEmptyTags(body);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
}