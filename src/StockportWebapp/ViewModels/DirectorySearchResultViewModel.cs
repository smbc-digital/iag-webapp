namespace StockportWebapp.ViewModels
{
    public class DirectorySearchResultViewModel
    {
        public DirectoryEntryViewModel Entry { get; set; }
        public string DirectorySlug { get; set; }
        public string Slug => $"{DirectorySlug}/{Entry.Slug}";
    }
}