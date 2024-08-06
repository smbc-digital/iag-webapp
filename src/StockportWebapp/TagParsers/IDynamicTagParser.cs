namespace StockportWebapp.TagParsers;

public interface IDynamicTagParser<T>
{
    string Parse(string content, IEnumerable<T> dynamicContent, bool redesigned = false);

    bool HasMatches(string content);
}
