namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedDirectoryEntry
    {
        public string Slug { get; }
        public string Title { get; }
        public string Body { get; }
        public string Teaser { get; }
        public string MetaDescription { get; }
        public IEnumerable<FilterTheme> Themes { get; }
        public IEnumerable<Directory> Directories { get; }

        public ProcessedDirectoryEntry(string slug, string title, string body, string teaser,
            string metaDescription, IEnumerable<FilterTheme> themes, IEnumerable<Directory> directories)
        {
            Slug = slug;
            Title = title;
            Body = body;
            Teaser = teaser;
            MetaDescription = metaDescription;
            Themes = themes;
            Directories = directories;
        }
    }
}