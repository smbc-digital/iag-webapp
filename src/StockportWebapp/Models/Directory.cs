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

        // TODO This is commented out - it's experiment to try and impose a heirachy in both breacrumb and URL structure - but not sure whether this really necessary 
        //public ICollection<string> RouteValues { get; set; } = new List<string>();

        //public void AddToRouteValuesIfNotNullOrEmpty(string value, bool isCurrent = false)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        var subDirectories = SubDirectories.ToList();
        //        subDirectories.ForEach(subDirectory => subDirectory.AddToRouteValuesIfNotNullOrEmpty(value));
        //        SubDirectories = subDirectories;

        //        if(!isCurrent)
        //            RouteValues.Add(value);
        //    }
        //}

        //public RouteValueDictionary ParentRouteValues
        //{
        //    get
        //    {
        //        var rvd = new RouteValueDictionary();
        //        rvd.Add("slug", Slug);
        //        for (var i = 0; i < RouteValues.Count; i++)
        //        {
        //            rvd.Add($"parentSlug{i+1}", RouteValues.ToArray()[i]);
        //        }

        //        return rvd;
        //    }
        //}
    }
}