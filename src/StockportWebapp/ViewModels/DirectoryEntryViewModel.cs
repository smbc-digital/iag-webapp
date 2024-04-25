using StockportWebapp.Comparers;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryEntryViewModel : ISlugComparable
{
    public DirectoryEntryViewModel() { }


    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry)
    {
        Slug = slug;
        DirectoryEntry = directoryEntry;
    }
    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, IEnumerable<Crumb> breadcrumbs) 
        : this(slug, directoryEntry)
        => Breadcrumbs = breadcrumbs;
    
    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, bool isPinned) 
        : this(slug, directoryEntry)    
        => IsPinned = isPinned;
    
    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, IEnumerable<Crumb> breadcrumbs, bool isPinned) 
        : this(slug, directoryEntry, breadcrumbs)
        => IsPinned = isPinned;    

    // Core page details

    public DirectoryEntry DirectoryEntry { get; set; }
    public string Slug { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public bool IsPinned { get; set; }
    public IEnumerable<Filter> HighlightedFilters => DirectoryEntry.Themes?
                                                        .SelectMany(_ => _.Filters.Where(_ => _.Highlight.Equals(true)))
                                                        .ToList();

    public bool DisplaySocials => !string.IsNullOrEmpty(DirectoryEntry.Facebook) 
                                    || !string.IsNullOrEmpty(DirectoryEntry.Twitter)
                                    || !string.IsNullOrEmpty(DirectoryEntry.Youtube)
                                    || !string.IsNullOrEmpty(DirectoryEntry.Instagram)
                                    || !string.IsNullOrEmpty(DirectoryEntry.LinkedIn);
    public bool HasPrimaryContact 
        => !string.IsNullOrEmpty(DirectoryEntry.PhoneNumber) ||  !string.IsNullOrEmpty(DirectoryEntry.Email);
    public bool DisplayContactUs => !string.IsNullOrEmpty(DirectoryEntry.Website) || HasPrimaryContact || DisplaySocials;
}