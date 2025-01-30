namespace StockportWebapp.TagParsers;

public class CallToActionTagParser(IViewRender viewRenderer) : IDynamicTagParser<CallToActionBanner>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{Call-To-Action:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string body, IEnumerable<CallToActionBanner> callToActionBanners, bool redesigned = false)
    {
        MatchCollection matches = TagRegex.Matches(body);

        foreach (Match match in matches)
        {
            string tagTitle = match.Groups[1].Value;
            CallToActionBanner callToAction = callToActionBanners?.FirstOrDefault(cta => cta.Title.Equals(tagTitle, StringComparison.OrdinalIgnoreCase));

            if (callToAction is not null)
            {
                string renderedCallToAction = _viewRenderer.Render("CallToActionTagParser", callToAction);
                body = TagRegex.Replace(body, renderedCallToAction, 1);
            }
        }

        return RemoveEmptyTags(body);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
}