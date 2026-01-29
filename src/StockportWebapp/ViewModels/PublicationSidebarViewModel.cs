namespace StockportWebapp.ViewModels;

public class PublicationSidebarViewModel(PublicationTemplate publication,
                                        PublicationPage currentPage,
                                        PublicationSection currentSection)
{
    public IReadOnlyList<PublicationSidebarPage> Items { get; } = publication.PublicationPages
        .Select(page => new PublicationSidebarPage(
            page,
            page.Equals(currentPage),
            page.Equals(currentPage) ? currentSection : null
        ))
        .ToList();
}

public class PublicationSidebarPage(PublicationPage page,
                                    bool isActive,
                                    PublicationSection activeSection)
{
    public string Title { get; } = page.Title;
    public string Slug { get; } = page.Slug;
    public bool IsActive { get; } = isActive;

    public IReadOnlyList<PublicationSidebarSection> Sections { get; } = isActive && page.PublicationSections is not null
        ? page.PublicationSections
            .Select(section => new PublicationSidebarSection(
                section,
                section.Equals(activeSection)
            ))
            .ToList()
        : new List<PublicationSidebarSection>();
}

public class PublicationSidebarSection(PublicationSection section, bool isActive)
{
    public string Title { get; } = section.Title;
    public string Slug { get; } = section.Slug;
    public bool IsActive { get; } = isActive;
}
