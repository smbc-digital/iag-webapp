namespace StockportWebapp.ViewModels;

public class PublicationTemplateViewModel
{
    public readonly PublicationSection DisplayedSection;
    public readonly PublicationTemplate PublicationTemplate;
    public readonly PublicationPage CurrentPage;
    public readonly PublicationSection? CurrentSection;
    public PublicationSidebarViewModel Sidebar { get; }
    public string PublicationTemplateSlug { get; }
    public string MetaDescription =>
        string.IsNullOrEmpty(DisplayedSection?.MetaDescription)
            ? PublicationTemplate.MetaDescription
            : DisplayedSection.MetaDescription;


    public PublicationTemplateViewModel(
            PublicationTemplate publication,
            PublicationPage currentPage,
            PublicationSection? currentSection)
    {
        PublicationTemplate = publication;
        CurrentPage = currentPage;
        CurrentSection = currentSection;

        // Default to first section if page has sections
        if (currentSection == null && currentPage?.PublicationSections?.Any() == true)
            CurrentSection = currentPage.PublicationSections.First();
        else
            CurrentSection = currentSection;

        PublicationTemplateSlug = publication.Slug;

        Sidebar = new PublicationSidebarViewModel(
            publication,
            currentPage,
            currentSection
        );
    }

    private int PageIndex =>
        PublicationTemplate.PublicationPages.ToList().IndexOf(CurrentPage);

    private int SectionIndex =>
        CurrentSection == null
            ? -1
            : CurrentPage.PublicationSections.ToList().IndexOf(CurrentSection);

    public bool HasNext()
        => GetNext() != null;
    
    public PaginationTarget GetNext()
    {
        // Sections exist → move within sections
        if (CurrentPage.PublicationSections?.Any() == true)
        {
            if (SectionIndex < CurrentPage.PublicationSections.Count - 1)
            {
                return PaginationTarget.ForSection(
                    PublicationTemplateSlug,
                    CurrentPage,
                    CurrentPage.PublicationSections[SectionIndex + 1]);
            }
        }

        // Move to next page
        if (PageIndex < PublicationTemplate.PublicationPages.Count - 1)
        {
            PublicationPage nextPage =
                PublicationTemplate.PublicationPages[PageIndex + 1];

            PublicationSection nextSection =
                nextPage.PublicationSections?.FirstOrDefault();

            return PaginationTarget.ForPage(PublicationTemplateSlug, nextPage, nextSection);
        }

        return null;
    }

    public bool HasPrevious()
        => GetPrevious() != null;

    public PaginationTarget GetPrevious()
    {
        if (CurrentPage.PublicationSections?.Any() == true && SectionIndex > 0)
        {
            return PaginationTarget.ForSection(
                PublicationTemplateSlug,
                CurrentPage,
                CurrentPage.PublicationSections[SectionIndex - 1]);
        }

        if (PageIndex > 0)
        {
            PublicationPage prevPage =
                PublicationTemplate.PublicationPages[PageIndex - 1];

            PublicationSection prevSection =
                prevPage.PublicationSections?.LastOrDefault();

            return PaginationTarget.ForPage(PublicationTemplateSlug, prevPage, prevSection);
        }

        return null;
    }

    public PageHeaderViewModel PageHeader => new()
    {
        Title = PublicationTemplate.Title,
        Subtitle = PublicationTemplate.Subtitle,
        UpdatedAt = new DateTime(2024, 7, 11), // PublicationTemplate.UpdatedAt,
        HeaderImageUrl = PublicationTemplate.HeroImage?.Url,
        HeaderHighlight = null,
        HeaderHighlightType = null
    };

    private IEnumerable<PublicationSection> AllSections() =>
        PublicationTemplate.PublicationPages
            .SelectMany(page => page.PublicationSections);

    private PublicationSection GetSectionOrThrow(string sectionSlug) =>
        AllSections().FirstOrDefault(_ => _.Slug.Equals(sectionSlug))
            ?? throw new SectionDoesNotExistException($"Section with slug: {sectionSlug} does not exist");

    private static PublicationSection FirstOrNull(IEnumerable<PublicationSection> sections) =>
        sections.FirstOrDefault();

    private int IndexForDisplayedSection()
    {
        var sections = AllSections().ToList();
        for (int i = 0; i < sections.Count; i++)
        {
            if (sections[i].Slug.Equals(DisplayedSection.Slug))
            {
                return i;
            }
        }

        return 0;
    }

    public PublicationSection NextSection()
    {
        var sections = AllSections().ToList();
        int nextSectionIndex = IndexForDisplayedSection() + 1;
        return nextSectionIndex < sections.Count
            ? sections.ElementAt(nextSectionIndex)
            : null;
    }

    public PublicationSection PreviousSection()
    {
        var sections = AllSections().ToList();
        int previousSectionIndex = IndexForDisplayedSection() - 1;
        return previousSectionIndex >= 0
            ? sections.ElementAt(previousSectionIndex)
            : null;
    }

    public bool ShouldShowNextSectionButton() =>
        NextSection() is not null;

    public bool ShouldShowPreviousSectionButton() =>
        PreviousSection() is not null;
}