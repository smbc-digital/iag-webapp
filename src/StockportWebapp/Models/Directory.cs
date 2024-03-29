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
        public IEnumerable<DirectoryEntry> Entries { get; set; } = new List<DirectoryEntry>();
        public IEnumerable<Directory> SubDirectories { get; set; } = new List<Directory>();
        public string ColourScheme { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public EventCalendarBanner EventBanner { get; set; }
        public IEnumerable<SubItem> RelatedContent { get; set; }
        public IEnumerable<ExternalLink> ExternalLinks { get; set; } = new List<ExternalLink>();

        [JsonIgnore]
        private IEnumerable<DirectoryEntry> _allEntries = null;

        [JsonIgnore]
        public IEnumerable<DirectoryEntry> AllEntries
        {
            get
            {
                _allEntries ??= (SubDirectories is not null && SubDirectories.Any()
                                    ? Entries?
                                        .Concat(SubDirectories
                                            .Where(sub => sub is not null)
                                            .SelectMany(sub => sub.AllEntries))     
                                    : Entries)
                                        .Where(entry => entry is not null && !string.IsNullOrEmpty(entry.Slug))
                                        .Distinct(new DirectoryEntryComparer());

                
                return _allEntries;
            }         
        }

        public string ToKml() => AllEntries.GetKmlForList();
    }
}