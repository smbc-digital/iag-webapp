using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class FilterTheme
{
    public string Title { get; set; }
    public IEnumerable<Filter> Filters { get; set; }
}