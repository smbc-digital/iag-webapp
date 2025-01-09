using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public interface ITagParserContainer
{
    string ParseAll(string content, string title = null, bool removeEmptyTags = true, IEnumerable<Alert> alerts = null, IEnumerable<Document> documents = null, IEnumerable<InlineQuote> inlineQuotes = null, IEnumerable<PrivacyNotice> privacyNotices = null, IEnumerable<Profile> profiles = null, bool redesigned = false);
}

public class TagParserContainer(IEnumerable<ISimpleTagParser> tagParsers,
                                IDynamicTagParser<Alert> alertsInlineTagParser,
                                IDynamicTagParser<Document> documentTagParser,
                                IDynamicTagParser<InlineQuote> inlineQuoteTagParser,
                                IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser,
                                IDynamicTagParser<Profile> profileTagParser) : ITagParserContainer
{
    private readonly IEnumerable<ISimpleTagParser> _tagParsers = tagParsers;
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser = alertsInlineTagParser;
    private readonly IDynamicTagParser<Document> _documentTagParser = documentTagParser;
    private readonly IDynamicTagParser<InlineQuote> _inlineQuoteTagParser = inlineQuoteTagParser;
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser = privacyNoticeTagParser;
    private readonly IDynamicTagParser<Profile> _profileTagParser = profileTagParser;
    private static Regex EmptyTagRegex => new("{{([�$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);

    public string ParseAll(string content,
                        string title = null,
                        bool removeEmptyTags = true,
                        IEnumerable<Alert> alerts = null,
                        IEnumerable<Document> documents = null,
                        IEnumerable<InlineQuote> inlineQuotes = null,
                        IEnumerable<PrivacyNotice> privacyNotices = null,
                        IEnumerable<Models.Profile> profiles = null,
                        bool redesigned = false)
    {
        string parsedContent = _tagParsers.Aggregate(content, (c, tagParser) => tagParser.Parse(c, title));
        parsedContent = _alertsInlineTagParser.Parse(parsedContent, alerts, redesigned);
        parsedContent = _documentTagParser.Parse(parsedContent, documents, redesigned);
        parsedContent = _inlineQuoteTagParser.Parse(parsedContent, inlineQuotes, redesigned);
        parsedContent = _privacyNoticeTagParser.Parse(parsedContent, privacyNotices, redesigned);
        parsedContent = _profileTagParser.Parse(parsedContent, profiles, redesigned);

        return removeEmptyTags
            ? RemoveEmptyTags(parsedContent)
            : parsedContent;
    }

    private static string RemoveEmptyTags(string content) =>
        EmptyTagRegex.Replace(content, string.Empty);
}