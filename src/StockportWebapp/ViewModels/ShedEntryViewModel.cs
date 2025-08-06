using StockportWebapp.Comparers;

namespace StockportWebapp.ViewModels;

public class ShedEntryViewModel : ISlugComparable
{
    public ShedEntryViewModel() { }

    public ShedItem ShedItem { get; set; }

    public ShedEntryViewModel(ShedItem shedItem)
    {
        ShedItem = shedItem;
    }

    public string Slug { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public bool IsPinned { get; set; } = false;
    public int MapPinIndex { get; set; } = 0;
    public MapDetails MapDetails { get; set; }
    public string AddressWithoutTags =>
        Regex.Replace(ShedItem.Location, "<.*?>", string.Empty); 

    public string ParentSlug { get; set; }  

    public string FullyResolvedSlug => $"{ParentSlug}/{Slug}";
    
    private static string BuildDescriptionHtml(string name, string slug, string teaser) =>
        $@"<a href='/directories/entry/{slug}'><h1>{name}</h1><p>{teaser}</p></a>";
}