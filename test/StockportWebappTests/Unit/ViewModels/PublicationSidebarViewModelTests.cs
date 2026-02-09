using System.Text.Json;

namespace StockportWebappTests_Unit.Unit.ViewModels;

public class PublicationSidebarViewModelTests
{
    private static PublicationTemplate BuildPublicationTemplate(List<PublicationPage> pages) =>
        new()
        {
            Slug = "pub-slug",
            Title = "Publication",
            MetaDescription = "meta",
            HeaderImage = new MediaAsset(),
            PublicationPages = pages
        };

    [Fact]
    public void Constructor_Populates_Items_ForAllPages_And_OnlyActivePage_HasSections()
    {
        // Arrange
        PublicationSection s1 = new() { Slug = "s1", Title = "Section 1" };
        PublicationSection s2 = new() { Slug = "s2", Title = "Section 2" };

        PublicationPage page1 = new()
        {
            Slug = "page-1",
            Title = "Page 1",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection> { s1, s2 }
        };

        PublicationPage page2 = new()
        {
            Slug = "page-2",
            Title = "Page 2",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection>
            {
                new PublicationSection { Slug = "p2s1", Title = "P2 Section" }
            }
        };

        PublicationTemplate publication = BuildPublicationTemplate(new List<PublicationPage> { page1, page2 });

        // Act
        PublicationSidebarViewModel vm = new(publication, page1, s2);

        // Assert
        Assert.Equal(2, vm.Items.Count);

        PublicationSidebarPage first = vm.Items[0];
        Assert.Equal(page1.Title, first.Title);
        Assert.Equal(page1.Slug, first.Slug);
        Assert.True(first.IsActive);
        Assert.Equal(2, first.Sections.Count);
        Assert.Equal("Section 1", first.Sections[0].Title);
        Assert.Equal("s1", first.Sections[0].Slug);
        Assert.False(first.Sections[0].IsActive);
        Assert.Equal("Section 2", first.Sections[1].Title);
        Assert.Equal("s2", first.Sections[1].Slug);
        Assert.True(first.Sections[1].IsActive);

        PublicationSidebarPage second = vm.Items[1];
        Assert.Equal(page2.Title, second.Title);
        Assert.Equal(page2.Slug, second.Slug);
        Assert.False(second.IsActive);
        Assert.Empty(second.Sections);
    }

    [Fact]
    public void PublicationSidebarPage_IncludesSections_WhenIsActive_And_PageHasSections()
    {
        // Arrange
        PublicationSection secA = new() { Slug = "a", Title = "A" };
        PublicationSection secB = new() { Slug = "b", Title = "B" };

        PublicationPage page = new()
        {
            Slug = "pg",
            Title = "Pg",
            Body = new JsonElement(),
            PublicationSections = new List<PublicationSection> { secA, secB }
        };

        // Act
        PublicationSidebarPage pageVmActive = new(page, true, secB);

        // Assert
        Assert.Equal("Pg", pageVmActive.Title);
        Assert.Equal("pg", pageVmActive.Slug);
        Assert.True(pageVmActive.IsActive);
        Assert.Equal(2, pageVmActive.Sections.Count);
        Assert.Equal("b", pageVmActive.Sections[1].Slug);
        Assert.True(pageVmActive.Sections[1].IsActive);

        // Act
        PublicationSidebarPage pageVmNotActive = new(page, false, secB);

        // Assert
        Assert.False(pageVmNotActive.IsActive);
        Assert.Empty(pageVmNotActive.Sections);
    }

    [Fact]
    public void PublicationSidebarPage_DoesNotIncludeSections_WhenPageSectionsIsNull()
    {
        // Arrange
        PublicationPage page = new()
        {
            Slug = "pg-null",
            Title = "PgNull",
            Body = new JsonElement(),
            PublicationSections = null
        };

        // Act
        PublicationSidebarPage pageVm = new(page, true, null);

        // Assert
        Assert.True(pageVm.IsActive);
        Assert.Empty(pageVm.Sections);
    }

    [Fact]
    public void PublicationSidebarSection_Maps_Properties_Correctly()
    {
        // Arrange
        PublicationSection section = new() { Slug = "sec", Title = "Section Title" };

        // Act
        PublicationSidebarSection sectionVm = new(section, true);

        // Assert
        Assert.Equal("Section Title", sectionVm.Title);
        Assert.Equal("sec", sectionVm.Slug);
        Assert.True(sectionVm.IsActive);
    }
}