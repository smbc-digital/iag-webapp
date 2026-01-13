namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class PaginationTarget
{
    public string Title { get; }
    public string Url { get; }

    private PaginationTarget(string title, string url)
    {
        Title = title;
        Url = url;
    }

    public static PaginationTarget ForSection(
        string publicationTemplateSlug,
        PublicationPage page,
        PublicationSection section)
        => new(
            section.Title,
            $"/publications/{publicationTemplateSlug}/{page.Slug}/{section.Slug}");

    public static PaginationTarget ForPage(
        string publicationTemplateSlug,
        PublicationPage page,
        PublicationSection section)
        => section == null
            ? new PaginationTarget(
                page.Title,
                $"/publications/{publicationTemplateSlug}/{page.Slug}")
            : ForSection(publicationTemplateSlug, page, section);
}