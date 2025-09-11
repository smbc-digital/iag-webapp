using StockportWebapp.Comparers;

namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ShedEntryViewModel : ISlugComparable
{
    public ShedEntryViewModel() { }

    public ShedItem ShedItem { get; set; }

    public ShedEntryViewModel(ShedItem shedItem) =>
        ShedItem = shedItem;

    public string Slug { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public bool IsPinned { get; set; } = false;
    public int MapPinIndex { get; set; } = 0;
    public MapDetails MapDetails { get; set; }
}