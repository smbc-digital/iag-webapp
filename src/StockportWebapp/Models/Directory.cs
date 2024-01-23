using StockportWebapp.Extensions;

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
        
        [JsonIgnore]
        public IEnumerable<DirectoryEntry> AllEntries =>  SubDirectories is not null
                                                            && SubDirectories.Any()  
                                                                ? Entries?
                                                                    .Concat(SubDirectories.SelectMany(sub => sub.AllEntries))
                                                                    .Distinct()
                                                                : Entries;
        [JsonIgnore]
        public IEnumerable<FilterTheme> AllFilterThemes => AllEntries is not null  
                                                            && AllEntries.Any() 
                                                                ? AllEntries.Where(entry => entry.Themes is not null).SelectMany(entry => entry.Themes).OrderBy(theme => theme.Title)
                                                                : new List<FilterTheme>();

        public string ToKml() => AllEntries.GetKmlForList();
    }
}