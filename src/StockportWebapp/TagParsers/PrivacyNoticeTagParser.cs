namespace StockportWebapp.TagParsers;

public class PrivacyNoticeTagParser : IDynamicTagParser<PrivacyNotice>
{
    private readonly IViewRender _viewRenderer;
    private readonly ILogger<PrivacyNotice> _logger;

    public PrivacyNoticeTagParser(IViewRender viewRenderer, ILogger<PrivacyNotice> logger)
    {
        _viewRenderer = viewRenderer;
        _logger = logger;
    }

    protected Regex TagRegex => new Regex("{{PrivacyNotice:(.*?)}}", RegexOptions.Compiled);

    public string Parse(string content, IEnumerable<PrivacyNotice> privacyNotices)
    {
        var matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            var privacyNoticeSlug = match.Groups[1].Value;

            privacyNotices = privacyNotices.Where(s => s.Title.Replace(" ", string.Empty) == privacyNoticeSlug).OrderBy(x => x.Category);

            if (privacyNotices.Any())
            {
                var privacyNoticeHtml = _viewRenderer.Render("PrivacyNotice", privacyNotices);
                content = TagRegex.Replace(content, privacyNoticeHtml, 1);
            }
        }
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content)
    {
        return TagRegex.Replace(content, string.Empty);
    }
}