namespace StockportWebapp.TagParsers;

public interface ISimpleTagParserContainer
{
    string ParseAll(string content, string title = null, bool removeEmptytags = true);
}

public class SimpleTagParserContainer : ISimpleTagParserContainer
{
    private readonly IEnumerable<ISimpleTagParser> _tagParsers;
    private static Regex EmptyTagRegex => new Regex("{{([�$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);

    public SimpleTagParserContainer(IEnumerable<ISimpleTagParser> tagParsers) => _tagParsers = tagParsers;

    public string ParseAll(string content, string title = null, bool removeEmptyTags = true)
    {
        var parsedContent = _tagParsers.Aggregate(content, (c, tagParser) => tagParser.Parse(c, title));
        return removeEmptyTags ? RemoveEmptyTags(parsedContent) : parsedContent;
    }

    private static string RemoveEmptyTags(string body) =>
        EmptyTagRegex.Replace(body, string.Empty);
}