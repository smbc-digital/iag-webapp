namespace StockportWebappTests_Unit.Unit.ViewModels;

public class PublicationTemplateViewModelTests
{
    private static PublicationTemplate BuildPublicationTemplate(
            string slug = "pub-slug",
            string title = "pub-title",
            string metaDescription = "pub-meta",
            DateTime? datePublished = null,
            DateTime? reviewDate = null,
            string headerImageUrl = null,
            List<PublicationPage>? pages = null) =>
        new()
        {
            Slug = slug,
            Title = title,
            MetaDescription = metaDescription,
            DatePublished = datePublished ?? DateTime.MinValue,
            ReviewDate = reviewDate ?? DateTime.MaxValue,
            HeaderImage = headerImageUrl is null ? null : new MediaAsset { Url = headerImageUrl },
            PublicationPages = pages ?? new List<PublicationPage>()
        };

    [Fact]
    public void MetaDescription_UsesPublicationMeta_WhenDisplayedSectionIsNull()
    {
        // Arrange
        PublicationTemplate publication = BuildPublicationTemplate(metaDescription: "publication meta");
        PublicationPage page = new() { Slug = "page-1", Title = "Page 1", Body = new JsonElement() };
        publication.PublicationPages = new List<PublicationPage> { page };

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, null);

        // Assert
        Assert.Equal("publication meta", viewModel.MetaDescription);
    }

    [Fact]
    public void Constructor_DefaultsCurrentSectionToFirst_WhenCurrentSectionIsNullAndPageHasSections()
    {
        // Arrange
        PublicationSection section1 = new()
        {
            Slug = "s1",
            Title = "Section 1"
        };

        PublicationSection section2 = new()
        {
            Slug = "s2",
            Title = "Section 2"
        };

        PublicationPage page = new()
        {
            Slug = "page-1",
            Title = "Page 1",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection> { section1, section2 }
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { page });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, null);

        // Assert
        Assert.NotNull(viewModel.CurrentSection);
        Assert.Equal(section1, viewModel.CurrentSection);
    }

    [Fact]
    public void HasNext_And_GetNext_ReturnsTarget_WhenMovingWithinSections()
    {
        // Arrange
        PublicationSection sectionA = new()
        {
            Slug = "a",
            Title = "A"
        };
        
        PublicationSection sectionB = new()
        {
            Slug = "b",
            Title = "B"
        };

        PublicationPage page = new()
        {
            Slug = "page-1",
            Title = "Page 1",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection> { sectionA, sectionB }
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { page });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, sectionA);

        // Assert
        Assert.True(viewModel.HasNext());
        Assert.NotNull(viewModel.GetNext());
    }

    [Fact]
    public void HasNext_And_GetNext_ReturnsTarget_WhenMovingToNextPage()
    {
        // Arrange
        PublicationPage firstPage = new()
        {
            Slug = "page-1",
            Title = "First Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationPage secondPage = new()
        {
            Slug = "page-2",
            Title = "Second Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { firstPage, secondPage });

        // Act
        PublicationTemplateViewModel vm = new(publication, firstPage, null);

        // Assert
        Assert.True(vm.HasNext());
        Assert.NotNull(vm.GetNext());
    }

    [Fact]
    public void HasNext_ReturnsFalse_WhenOnLastPageAndNoFurtherSections()
    {
        // Arrange
        PublicationPage page = new()
        {
            Slug = "only-page",
            Title = "Only Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { page });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, null);

        // Assert
        Assert.False(viewModel.HasNext());
        Assert.Null(viewModel.GetNext());
    }

    [Fact]
    public void HasPrevious_And_GetPrevious_ReturnsTarget_WhenMovingWithinSections()
    {
        // Arrange
        PublicationSection section1 = new()
        {
            Slug = "s1",
            Title = "S1"
        };

        PublicationSection section2 = new()
        {
            Slug = "s2",
            Title = "S2"
        };

        PublicationPage page = new()
        {
            Slug = "page-1",
            Title = "Page 1",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection> { section1, section2 }
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { page });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, section2);

        // Assert
        Assert.True(viewModel.HasPrevious());
        Assert.NotNull(viewModel.GetPrevious());
    }

    [Fact]
    public void HasPrevious_And_GetPrevious_ReturnsTarget_WhenMovingToPreviousPage()
    {
        // Arrange
        PublicationPage prevPage = new()
        {
            Slug = "page-1",
            Title = "Prev Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationPage currentPage = new()
        {
            Slug = "page-2",
            Title = "Current Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { prevPage, currentPage });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, currentPage, null);

        // Assert
        Assert.True(viewModel.HasPrevious());
        Assert.NotNull(viewModel.GetPrevious());
    }

    [Fact]
    public void HasPrevious_ReturnsFalse_WhenOnFirstPageAndNoPreviousSections()
    {
        // Arrange
        PublicationPage page = new()
        {
            Slug = "only-page",
            Title = "Only Page",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>()
        };

        PublicationTemplate publication = BuildPublicationTemplate(pages: new List<PublicationPage> { page });

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, null);

        // Assert
        Assert.False(viewModel.HasPrevious());
        Assert.Null(viewModel.GetPrevious());
    }

    [Fact]
    public void PageHeader_MapsFields_FromPublicationTemplate()
    {
        // Arrange
        PublicationTemplate publication = BuildPublicationTemplate(
            title: "Publication Title",
            slug: "slug",
            metaDescription: "meta",
            datePublished: new(2020, 1, 1),
            reviewDate: new(2021, 6, 1),
            headerImageUrl: "header-image.jpg"
        );

        PublicationPage page = new()
        {
            Slug = "page",
            Title = "Page",
            Body = new JsonElement()
        };
        publication.PublicationPages = new List<PublicationPage> { page };

        // Act
        PublicationTemplateViewModel viewModel = new(publication, page, null);
        PageHeaderViewModel header = viewModel.PageHeader;

        // Assert
        Assert.Equal("Publication Title", header.Title);
        Assert.Equal(new(2020, 1, 1), header.DatePublished);
        Assert.Equal(new(2021, 6, 1), header.UpdatedAt);
        Assert.Equal("header-image.jpg", header.HeaderImageUrl);
        Assert.True(header.DisplayLastUpdated);
        Assert.True(header.DisplayDatePublished);
        Assert.True(header.IsPublication);
    }
}