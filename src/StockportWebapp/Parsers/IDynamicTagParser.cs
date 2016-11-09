using System.Collections.Generic;

namespace StockportWebapp.Parsers
{
    public interface IDynamicTagParser<T>
    {
        string Parse(string body, IEnumerable<T> dynamicContent);
    }
}
