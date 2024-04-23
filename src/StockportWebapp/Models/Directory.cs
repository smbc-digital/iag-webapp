using Amazon.Util.Internal;
using System.IO;

namespace StockportWebapp.Models
{
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

        private IEnumerable<DirectoryEntry> entries = new List<DirectoryEntry>();

        public IEnumerable<DirectoryEntry> GetEntries()
        {
            return entries;
        }

        public void SetEntries(IEnumerable<DirectoryEntry> value)
        {
            entries = value;
        }

        public IEnumerable<Directory> SubDirectories { get; set; } = new List<Directory>();
        public IEnumerable<SubItem> SubItems { get; set; } = new List<SubItem>();
        public string ColourScheme { get; set; } = string.Empty;
        public string SearchBranding { get; set; } = "Default";
        public string Icon { get; set; } = string.Empty;
        public EventCalendarBanner EventBanner { get; set; }
        public IEnumerable<SubItem> RelatedContent { get; set; }
        public IEnumerable<ExternalLink> ExternalLinks { get; set; } = new List<ExternalLink>();
        public IEnumerable<DirectoryEntry> PinnedEntries { get; set; } = new List<DirectoryEntry>();

        [JsonIgnore]
        private IEnumerable<DirectoryEntry> _entries = null;

        /// <summary>
        /// Gets a list of entries relevant to this directory including those in sub directories, but exclusing pinned entries
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DirectoryEntry> Entries
        {
            get
            {
                _entries ??= (SubDirectories is not null && SubDirectories.Any()
                                    ? GetEntries()?
                                        .Concat(SubDirectories
                                            .Where(sub => sub is not null)
                                            .SelectMany(sub => sub.Entries))     
                                    : GetEntries())
                                        .Where(entry => entry is not null && !string.IsNullOrEmpty(entry.Slug))
                                        .Distinct(new DirectoryEntryComparer());

                
                return _entries;
            }         
        }

        /// <summary>
        /// Returns a list of entries excluding pinned entries
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DirectoryEntry> RegularEntries 
            => Entries.Where(entry => !PinnedEntries.Any(pinnedEntry => pinnedEntry.Slug.Equals(entry.Slug)));

        /// <summary>
        /// Returns a list of of all entries including pinned and unpinned
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DirectoryEntry> AllEntries
            => RegularEntries.Concat(PinnedEntries);

        public string ToKml()
            => Entries.GetKmlForList();
    }
}