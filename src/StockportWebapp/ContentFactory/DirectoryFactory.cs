using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.ContentFactory;

public class DirectoryFactory
{
    public DirectoryFactory() {}

    public virtual ProcessedDirectory Build(Directory directory) => new(directory.Title, directory.Slug,
                directory.ContentfulId, directory.Teaser, directory.MetaDescription, directory.BackgroundImage,
                directory.Body, directory.CallToAction, directory.Alerts, directory.Entries, directory.SubDirectories)
    {
        CallToAction = directory.CallToAction
    };

    public virtual ProcessedDirectoryEntry Build(DirectoryEntry directoryEntry) => new(
        directoryEntry.Slug, directoryEntry.Name, directoryEntry.Description, directoryEntry.Teaser, directoryEntry.MetaDescription,
        directoryEntry.Themes, directoryEntry.Directories, directoryEntry.Alerts, directoryEntry.Branding, directoryEntry.MapPosition,
        directoryEntry.PhoneNumber, directoryEntry.Email, directoryEntry.Website, directoryEntry.Twitter, directoryEntry.Facebook, directoryEntry.Address
    );
}