namespace StockportWebapp.TagParsers;

public interface IDynamicTagParser<T>
{
    string Parse(string content, IEnumerable<T> dynamicContent);

    bool HasMatches(string content);
}
