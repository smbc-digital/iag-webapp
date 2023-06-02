namespace StockportWebapp.TagParsers;

public interface ISimpleTagParser
{
    string Parse(string body, string title = null);
}