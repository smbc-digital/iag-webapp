namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedDirectoryEntry
    {
        public string Slug { get; }
        public string Name { get; }
        public string Description { get; }
        public string Teaser { get; }
        public string MetaDescription { get; }
        public IEnumerable<FilterTheme> Themes { get; }
        public IEnumerable<MinimalDirectory> Directories { get; }
        public IEnumerable<Alert> Alerts { get; }
        public IEnumerable<GroupBranding> Branding { get; }
        public MapPosition MapPosition { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string Website { get; }
        public string Twitter { get; }
        public string Facebook { get; }
        public string Address { get; }

        public ProcessedDirectoryEntry(string slug, string name, string description, string teaser,
            string metaDescription, IEnumerable<FilterTheme> themes, IEnumerable<MinimalDirectory> directories,
            IEnumerable<Alert> alerts, IEnumerable<GroupBranding> branding, MapPosition mapPosition, string phoneNumber,
            string email, string website, string twitter, string facebook, string address)
        {
            Slug = slug;
            Name = name;
            Description = description;
            Teaser = teaser;
            MetaDescription = metaDescription;
            Themes = themes;
            Directories = directories;
            Alerts = alerts;
            Branding = branding;
            MapPosition = mapPosition;
            PhoneNumber = phoneNumber;
            Email = email;
            Website = website;
            Twitter = twitter;
            Facebook = facebook;
            Address = address;
        }
    }
}