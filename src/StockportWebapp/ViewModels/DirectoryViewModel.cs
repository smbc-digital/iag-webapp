using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public ProcessedDirectory Directory { get; set; }
    public ProcessedDirectoryEntry DirectoryEntry { get; set; }
    public ProcessedDirectory SubDirectory { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
}
