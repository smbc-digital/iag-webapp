using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.ContentFactory;

public class DirectoryFactory
{
    public DirectoryFactory() {}

    public virtual ProcessedDirectory Build(Directory directory) => new(directory.Title, directory.Slug,
                directory.ContentfulId, directory.Teaser, directory.MetaDescription, directory.BackgroundImage,
                directory.Body, directory.CallToAction, directory.Alerts, directory.Entries)
    {
        CallToAction = directory.CallToAction
    };

    public virtual ProcessedDirectoryEntry Build(DirectoryEntry directoryEntry) => new(
        directoryEntry.Slug, directoryEntry.Title, directoryEntry.Body, directoryEntry.Teaser, directoryEntry.MetaDescription, directoryEntry.Themes, directoryEntry.Directories
    );
}