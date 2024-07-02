using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public interface ITagParserContainer
{
    string ParseAll(string content, string title = null, bool removeEmptyTags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> inlineQuotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Profile> profiles = null);
}

public class TagParserContainer : ITagParserContainer
{
    private readonly IEnumerable<ISimpleTagParser> _tagParsers;
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
    private readonly IDynamicTagParser<Document> _documentTagParser;
    private readonly IDynamicTagParser<InlineQuote> _inlineQuoteTagParser;
    private readonly IDynamicTagParser<InlineQuote> _quoteTagParser;
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
    private readonly IDynamicTagParser<Profile> _profileTagParser;
    private static Regex EmptyTagRegex => new("{{([�$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);


    public TagParserContainer(IEnumerable<ISimpleTagParser> tagParsers, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<InlineQuote> inlineQuoteTagParser,
    IDynamicTagParser<InlineQuote> quoteTagParser,  IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IDynamicTagParser<Profile> profileTagParser)
    {
        _tagParsers = tagParsers;
        _alertsInlineTagParser = alertsInlineTagParser;
        _documentTagParser = documentTagParser;
        _inlineQuoteTagParser = inlineQuoteTagParser;
        _quoteTagParser = quoteTagParser;
        _privacyNoticeTagParser = privacyNoticeTagParser;
        _profileTagParser = profileTagParser;
    } 

    public string ParseAll(string content, string title = null, bool removeEmptyTags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> inlineQuotes = null,
    IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Models.Profile> profiles = null)
    {
        string parsedContent = _tagParsers.Aggregate(content, (c, tagParser) => tagParser.Parse(c, title));
        parsedContent = _alertsInlineTagParser.Parse(parsedContent, alerts);
        parsedContent = _documentTagParser.Parse(parsedContent, documents);
        parsedContent = _inlineQuoteTagParser.Parse(parsedContent, inlineQuotes);
        parsedContent = _quoteTagParser.Parse(parsedContent, inlineQuotes);
        parsedContent = _privacyNoticeTagParser.Parse(parsedContent, privacyNotices);
        parsedContent = _profileTagParser.Parse(parsedContent, profiles);

        return removeEmptyTags ? RemoveEmptyTags(parsedContent) : parsedContent;
    }

    private static string RemoveEmptyTags(string content) =>
        EmptyTagRegex.Replace(content, string.Empty);
}