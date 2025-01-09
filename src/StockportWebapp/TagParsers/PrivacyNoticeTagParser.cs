namespace StockportWebapp.TagParsers;

public class PrivacyNoticeTagParser(IViewRender viewRenderer) : IDynamicTagParser<PrivacyNotice>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{PrivacyNotice:(.*?)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<PrivacyNotice> privacyNotices, bool redesigned = false)
    {
        MatchCollection matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            string privacyNoticeSlug = match.Groups[1].Value;
            privacyNotices = privacyNotices?.Where(s => s.Title.Replace(" ", string.Empty) == privacyNoticeSlug).OrderBy(x => x.Category);

            if (privacyNotices.Any())
            {
                string privacyNoticeHtml = _viewRenderer.Render("PrivacyNotice", privacyNotices);
                content = TagRegex.Replace(content, privacyNoticeHtml, 1);
            }
        }
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
}