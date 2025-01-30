namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class FeaturedTasks(List<SubItem> subItems)
{
    public readonly List<SubItem> SubItems = subItems;
}