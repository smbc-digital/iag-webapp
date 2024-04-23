using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryEntryViewModel
{
    public DirectoryEntryViewModel() { }

    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, IEnumerable<Crumb> breadcrumbs) : this(slug, directoryEntry)
    {
        Breadcrumbs = breadcrumbs;
    }
    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry)
    {
        Slug = slug;
        Name = directoryEntry.Name;
        MetaDescription = directoryEntry.MetaDescription;
        Description = directoryEntry.Description;
        PhoneNumber = directoryEntry.PhoneNumber;
        Email = directoryEntry.Email;
        Website = directoryEntry.Website;
        Twitter = directoryEntry.Twitter;
        Facebook = directoryEntry.Facebook;
        Youtube = directoryEntry.Youtube;
        Instagram = directoryEntry.Instagram;
        LinkedIn = directoryEntry.LinkedIn;
        Address = directoryEntry.Address;
        Image = directoryEntry.Image;
        Branding = directoryEntry.Branding;
    }

    // Core page details
    public string Slug { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public string Description { get; set; }
    public string Teaser { get; set; }
    public string Image { get; set; }
    public string MetaDescription { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Twitter { get; set; }
    public string Facebook { get; set; }
    public string Youtube { get; set; }
    public string Instagram { get; set; }
    public string LinkedIn { get; set; }
    public string Address { get; set; }
    public List<GroupBranding> Branding { get; set; }
    public IEnumerable<FilterTheme> Themes { get; set; } = new List<FilterTheme>();
    public IEnumerable<Filter> HighlightedFilters => Themes?
                                                        .SelectMany(_ => _.Filters.Where(_ => _.Highlight.Equals(true)))
                                                        .ToList();

    public bool DisplaySocials => !string.IsNullOrEmpty(Facebook) 
                                    || !string.IsNullOrEmpty(Twitter)
                                    || !string.IsNullOrEmpty(Youtube)
                                    || !string.IsNullOrEmpty(Instagram)
                                    || !string.IsNullOrEmpty(LinkedIn);
    public bool HasPrimaryContact 
        => !string.IsNullOrEmpty(PhoneNumber) ||  !string.IsNullOrEmpty(Email);
    public bool DisplayContactUs => !string.IsNullOrEmpty(Website) || HasPrimaryContact || DisplaySocials;

}