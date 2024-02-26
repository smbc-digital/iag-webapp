using Amazon.S3.Model.Internal.MarshallTransformations;

namespace StockportWebapp.ViewModels
{
    public class DirectorySearchResultViewModel
    {
        public DirectoryEntry Entry { get; set; }
        public string DirectorySlug { get; set; }

        public string Slug => $"{DirectorySlug}/{Entry.Slug}";
    }
}
