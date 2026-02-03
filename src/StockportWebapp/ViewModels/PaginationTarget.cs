namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class PaginationTarget
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string PageTitle { get; set; }
    public string? SectionTitle { get; set; }
    public bool IsSection => !string.IsNullOrEmpty(SectionTitle);
    public bool SamePageAsCurrent { get; set; }

    private PaginationTarget(string pageTitle, string? sectionTitle, string url, bool samePageAsCurrent)
    {
        PageTitle = pageTitle;
        SectionTitle = sectionTitle;
        Url = url;
        SamePageAsCurrent = samePageAsCurrent;
    }

    public static PaginationTarget ForSection(string publicationTemplateSlug, PublicationPage page, PublicationSection section, bool samePageAsCurrent) =>
        new(page.Title, section.Title, $"/publications/{publicationTemplateSlug}/{page.Slug}/{section.Slug}", samePageAsCurrent);

    public static PaginationTarget ForPage(string publicationTemplateSlug, PublicationPage page, PublicationSection section) =>
        section is null
        ? new PaginationTarget(
            page.Title,
            null,
            $"/publications/{publicationTemplateSlug}/{page.Slug}",
            false)
        : ForSection(publicationTemplateSlug, page, section, false);
}