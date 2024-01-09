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
        public IEnumerable<DirectoryEntry> Entries { get; set; }
        public IEnumerable<Directory> SubDirectories { get; set; } = new List<Directory>();
        public IEnumerable<DirectoryEntry> AllEntries => SubDirectories.Any() ? Entries?.Concat(SubDirectories.SelectMany(sub => sub.AllEntries)).Distinct() : Entries;
        public IEnumerable<FilterTheme> AllFilterThemes => AllEntries.Where(entry => entry.Themes is not null).SelectMany(entry => entry.Themes).OrderBy(theme => theme.Title);
        public string ToKml() => AllEntries.GetKmlForList();
    }
}