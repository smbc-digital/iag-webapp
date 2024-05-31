namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class FeaturedTasks
{
    public readonly List<SubItem> SubItems;

    public FeaturedTasks(List<SubItem> subItems)
    {
        SubItems = subItems;
    }
}