namespace StockportWebapp.ContentFactory;

public class DirectoryEntryFactory
{
    public DirectoryEntryFactory() {}

    public virtual ProcessedDirectoryEntry Build(DirectoryEntry directoryEntry) => new(directoryEntry.Slug, directoryEntry.Title, directoryEntry.Body,
            directoryEntry.Teaser, directoryEntry.MetaDescription, directoryEntry.Themes, directoryEntry.Directories)
    { };
}