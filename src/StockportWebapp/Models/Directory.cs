using StockportWebapp.Comparers;
namespace StockportWebapp.Models;

public class Directory
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ContentfulId { get; set; } = string.Empty;
    public string Teaser { get; set; } = string.Empty;
    public string MetaDescription { get; set; }
    public string BackgroundImage { get; set; }
    public string Body { get; set; } = string.Empty;
    public CallToActionBanner CallToAction { get; init; }
    public IEnumerable<Alert> Alerts { get; set; }
    public IEnumerable<Alert> AlertsInline { get; set; }
    public IEnumerable<DirectoryEntry> Entries { get; set; } = new List<DirectoryEntry>();
    public IEnumerable<Directory> SubDirectories { get; set; } = new List<Directory>();
    public IEnumerable<SubItem> SubItems { get; set; } = new List<SubItem>();
    public EColourScheme ColourScheme { get; set; } = EColourScheme.Teal;
    public string SearchBranding { get; set; } = "Default";
    public string Icon { get; set; } = string.Empty;
    public EventCalendarBanner EventBanner { get; set; }
    public IEnumerable<SubItem> RelatedContent { get; set; }
    public IEnumerable<ExternalLink> ExternalLinks { get; set; } = new List<ExternalLink>();
    public IEnumerable<DirectoryEntry> PinnedEntries { get; set; } = new List<DirectoryEntry>();

    [JsonIgnore]
    private IEnumerable<DirectoryEntry> _cummulativeEntries = null;

    /// <summary>
    /// Gets a list of entries relevant to this directory including those in sub directories, but exclusing pinned entries
    /// </summary>
    [JsonIgnore]
    public IEnumerable<DirectoryEntry> CummulativeEntries
    {
        get
        {
            IEnumerable<DirectoryEntry> cummulativeEntries = SubDirectories is not null && SubDirectories.Any()
                                        ? Entries?
                                            .Concat(SubDirectories
                                                .Where(sub => sub is not null)
                                                .SelectMany(sub => sub.CummulativeEntries))
                                        : Entries;

            return cummulativeEntries
                    .Where(entry => entry is not null && !string.IsNullOrEmpty(entry.Slug))
                    .Distinct(new SlugComparer())
                    .Select(entry => (DirectoryEntry)entry);
        }         
    }

    /// <summary>
    /// Returns a list of entries excluding pinned entries
    /// </summary>
    [JsonIgnore]
    public IEnumerable<DirectoryEntry> RegularEntries 
        => CummulativeEntries.Where(entry => !PinnedEntries.Any(pinnedEntry => pinnedEntry.Slug.Equals(entry.Slug)));

    /// <summary>
    /// Returns a list of of all entries including pinned and unpinned
    /// </summary>
    [JsonIgnore]
    public IEnumerable<DirectoryEntry> AllEntries
        => RegularEntries.Concat(PinnedEntries);
}