using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.ContentFactory;

public class DirectoryFactory
{
    public DirectoryFactory() {}

    public virtual ProcessedDirectory Build(Directory directory) => new(directory.Title, directory.Slug,
                directory.ContentfulId, directory.Teaser, directory.MetaDescription, directory.BackgroundImage,
<<<<<<< HEAD
                directory.Body, directory.CallToAction, directory.Alerts, directory.Entries, directory.SubDirectories)
=======
                directory.Body, directory.CallToAction, directory.Alerts, directory.Entries, directory.AllEntries, directory.AllFilterThemes)
>>>>>>> 86225556 (minor: basic start to styling results pages)
    {
        CallToAction = directory.CallToAction,
        SubDirectories = directory.SubDirectories?.Select(directory => Build(directory))
    };

    public virtual ProcessedDirectoryEntry Build(DirectoryEntry directoryEntry) => new(
<<<<<<< HEAD
        directoryEntry.Slug, directoryEntry.Name, directoryEntry.Description, directoryEntry.Teaser, directoryEntry.MetaDescription,
        directoryEntry.Themes, directoryEntry.Directories, directoryEntry.Alerts, directoryEntry.Branding, directoryEntry.MapPosition,
        directoryEntry.PhoneNumber, directoryEntry.Email, directoryEntry.Website, directoryEntry.Twitter, directoryEntry.Facebook, directoryEntry.Address
=======
        directoryEntry.Slug, directoryEntry.Name, directoryEntry.Body, directoryEntry.Teaser, directoryEntry.MetaDescription, directoryEntry.Themes, directoryEntry.Directories
>>>>>>> 86225556 (minor: basic start to styling results pages)
    );
}