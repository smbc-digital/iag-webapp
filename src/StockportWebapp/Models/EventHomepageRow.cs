namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EventHomepageRow
{
    public bool IsLatest { get; set; }
    public string Tag { get; set; }
    public bool MatchedByTag { get; set; }
    public IEnumerable<Event> Events { get; set; }
}