namespace StockportWebapp.ViewModels;

public class PublicationTemplateViewModel
{
    public readonly PublicationTemplate PublicationTemplate;
    public readonly PublicationSection DisplayedSection;
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
        if (currentSection is null && currentPage?.PublicationSections?.Any() is true)
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
        => GetPrevious() is not null;

    public PaginationTarget GetPrevious()
    {
        if (CurrentPage.PublicationSections?.Any() is true && SectionIndex > 0)
        {
            return PaginationTarget.ForSection(
                PublicationTemplateSlug,
                CurrentPage,
                CurrentPage.PublicationSections[SectionIndex - 1]);
        }

        if (PageIndex > 0)
        {
            PublicationPage prevPage = PublicationTemplate.PublicationPages[PageIndex - 1];

            PublicationSection prevSection = prevPage.PublicationSections?.LastOrDefault();

            return PaginationTarget.ForPage(PublicationTemplateSlug, prevPage, prevSection);
        }

        return null;
    }

    public PageHeaderViewModel PageHeader => new()
    {
        Title = PublicationTemplate.Title,
        Subtitle = PublicationTemplate.Subtitle,
        DatePublished = PublicationTemplate.DatePublished,
        UpdatedAt = PublicationTemplate.LastUpdated,
        HeaderImageUrl = PublicationTemplate.HeroImage?.Url,
        HeaderHighlight = null,
        HeaderHighlightType = null,
        DisplayLastUpdated = !PublicationTemplate.LastUpdated.Equals(DateTime.MaxValue),
        DisplayDatePublished = !PublicationTemplate.DatePublished.Equals(DateTime.MinValue),
        IsPublication = true
    };
}