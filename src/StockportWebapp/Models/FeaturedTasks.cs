namespace StockportWebapp.Models;

public class FeaturedTasks
{
    public readonly List<SubItem> SubItems;

    public FeaturedTasks(List<SubItem> subItems)
    {
        SubItems = subItems;
    }
}

public class NullFeaturedTasks : FeaturedTasks
{
    public NullFeaturedTasks() : base(new List<SubItem>())
    {
    }
}
