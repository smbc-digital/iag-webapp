namespace StockportWebapp.ViewModels;

public class PublicationSidebarViewModel
{
    public IReadOnlyList<PublicationSidebarPage> Items { get; }

    public PublicationSidebarViewModel(
        PublicationTemplate publication,
        PublicationPage currentPage,
        PublicationSection currentSection)
    {
        Items = publication.PublicationPages
            .Select(page => new PublicationSidebarPage(
                page,
                page == currentPage,
                page == currentPage ? currentSection : null
            ))
            .ToList();
    }

    private static PublicationSidebarItem BuildPageItem(
        PublicationPage page,
        string currentPageSlug,
        string? currentSectionSlug)
    {
        var pageItem = new PublicationSidebarItem
        {
            Title = page.Title,
            Slug = page.Slug,
            IsPage = true,
            IsActive = page.Slug == currentPageSlug
        };

        if (page.PublicationSections?.Any() == true)
        {
            pageItem.Children.AddRange(
                page.PublicationSections.Select(section =>
                    new PublicationSidebarItem
                    {
                        Title = section.Title,
                        Slug = section.Slug,
                        IsPage = false,
                        IsActive =
                            page.Slug == currentPageSlug &&
                            section.Slug == currentSectionSlug
                    }));
        }

        return pageItem;
    }
}

public class PublicationSidebarItem
{
    public string Title { get; init; }
    public string Slug { get; init; }

    /// <summary>
    /// True = PublicationPage
    /// False = PublicationSection
    /// </summary>
    public bool IsPage { get; init; }

    public bool IsActive { get; set; }

    public List<PublicationSidebarItem> Children { get; init; } = new();
}

public class PublicationSidebarPage
{
    public string Title { get; }
    public string Slug { get; }
    public bool IsActive { get; }

    public IReadOnlyList<PublicationSidebarSection> Sections { get; }

    public PublicationSidebarPage(
        PublicationPage page,
        bool isActive,
        PublicationSection activeSection)
    {
        Title = page.Title;
        Slug = page.Slug;
        IsActive = isActive;

        // 🔑 Key rule enforced here
        Sections = isActive && page.PublicationSections is not null
            ? page.PublicationSections
                .Select(section => new PublicationSidebarSection(
                    section,
                    section == activeSection
                ))
                .ToList()
            : new List<PublicationSidebarSection>();
    }
}

public class PublicationSidebarSection
{
    public string Title { get; }
    public string Slug { get; }
    public bool IsActive { get; }

    public PublicationSidebarSection(
        PublicationSection section,
        bool isActive)
    {
        Title = section.Title;
        Slug = section.Slug;
        IsActive = isActive;
    }
}
