using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public interface ITagParserContainer
{
    string ParseAll(string content, string title = null, bool removeEmptyTags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> quotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Models.Profile> profiles = null, IEnumerable<S3BucketSearch> s3BucketSearches = null);
}

public class TagParserContainer : ITagParserContainer
{
    private readonly IEnumerable<ISimpleTagParser> _tagParsers;
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
    private readonly IDynamicTagParser<Document> _documentTagParser;
    private readonly IDynamicTagParser<InlineQuote> _inlineQuoteTagParser;
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
    private readonly IDynamicTagParser<Profile> _profileTagParser;
    private readonly IDynamicTagParser<S3BucketSearch> _s3BucketSearchTagParser;
    private static Regex EmptyTagRegex => new Regex("{{([�$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);


    public TagParserContainer(IEnumerable<ISimpleTagParser> tagParsers, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<InlineQuote> inlineQuoteTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IDynamicTagParser<Profile> profileTagParser, IDynamicTagParser<S3BucketSearch> s3BucketSearchTagParser)
    {
        _tagParsers = tagParsers;
        _alertsInlineTagParser = alertsInlineTagParser;
        _documentTagParser = documentTagParser;
        _inlineQuoteTagParser = inlineQuoteTagParser;
        _privacyNoticeTagParser = privacyNoticeTagParser;
        _profileTagParser = profileTagParser;
        _s3BucketSearchTagParser = s3BucketSearchTagParser;
    } 

    public string ParseAll(string content, string title = null, bool removeEmptyTags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> quotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Models.Profile> profiles = null, IEnumerable<S3BucketSearch> s3BucketSearches = null)
    {
        var parsedContent = _tagParsers.Aggregate(content, (c, tagParser) => tagParser.Parse(c, title));
        parsedContent = _alertsInlineTagParser.Parse(parsedContent, alerts);
        parsedContent = _documentTagParser.Parse(parsedContent, documents);
        parsedContent = _inlineQuoteTagParser.Parse(parsedContent, quotes);
        parsedContent = _privacyNoticeTagParser.Parse(parsedContent, privacyNotices);
        parsedContent = _profileTagParser.Parse(parsedContent, profiles);
        parsedContent = _s3BucketSearchTagParser.Parse(parsedContent, s3BucketSearches);

        return removeEmptyTags ? RemoveEmptyTags(parsedContent) : parsedContent;
    }

    private static string RemoveEmptyTags(string content) =>
        EmptyTagRegex.Replace(content, string.Empty);

}