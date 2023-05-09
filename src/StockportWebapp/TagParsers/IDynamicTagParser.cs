namespace StockportWebapp.TagParsers;

public interface IDynamicTagParser<T>
{
    string Parse(string body, IEnumerable<T> dynamicContent);
}
