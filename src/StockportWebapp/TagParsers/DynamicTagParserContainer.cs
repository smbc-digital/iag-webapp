using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public interface IDynamicTagParserContainer
{
    string ParseAll(string content, string title = null, bool removeEmptytags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> quotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Models.Profile> profiles = null, IEnumerable<S3BucketSearch> s3BucketSearches = null);
}

public class DynamicTagParserContainer : IDynamicTagParserContainer
{
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
    private readonly IDynamicTagParser<Document> _documentTagParser;
    private readonly IDynamicTagParser<InlineQuote> _inlineQuoteTagParser;
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
    private readonly IDynamicTagParser<Profile> _profileTagParser;
    private readonly IDynamicTagParser<S3BucketSearch> _s3BucketSearchTagParser;

    private static Regex EmptyTagRegex => new Regex("{{([£$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);

    public DynamicTagParserContainer(IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<InlineQuote> inlineQuoteTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IDynamicTagParser<Profile> profileTagParser, IDynamicTagParser<S3BucketSearch> s3BucketSearchTagParser)
    {
        _alertsInlineTagParser = alertsInlineTagParser;
        _documentTagParser = documentTagParser;
        _inlineQuoteTagParser = inlineQuoteTagParser;
        _privacyNoticeTagParser = privacyNoticeTagParser;
        _profileTagParser = profileTagParser;
        _s3BucketSearchTagParser = s3BucketSearchTagParser;
    }

    public string ParseAll(string content, string title = null, bool removeEmptytags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> quotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Models.Profile> profiles = null, IEnumerable<S3BucketSearch> s3BucketSearches = null)
    {
        var parsedContent = content;
        parsedContent = _alertsInlineTagParser.Parse(parsedContent, alerts);
        parsedContent = _documentTagParser.Parse(parsedContent, documents);
        parsedContent = _inlineQuoteTagParser.Parse(parsedContent, quotes);
        parsedContent = _privacyNoticeTagParser.Parse(parsedContent, privacyNotices);
        parsedContent = _profileTagParser.Parse(parsedContent, profiles);
        parsedContent = _s3BucketSearchTagParser.Parse(parsedContent, s3BucketSearches);

        if (removeEmptytags)
            parsedContent = RemoveEmptyTags(parsedContent);

        return parsedContent;
    }

    private static string RemoveEmptyTags(string body) =>
        EmptyTagRegex.Replace(body, string.Empty);
}