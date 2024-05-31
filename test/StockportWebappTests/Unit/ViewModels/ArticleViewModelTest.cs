namespace StockportWebappTests_Unit.Unit.ViewModels;

public class ArticleViewModelTest
{
    private readonly ProcessedSection _sectionOne;
    private readonly ProcessedSection _sectionTwo;
    private readonly ProcessedSection _sectionThree;
    private readonly ProcessedArticle _article;
    private readonly ArticleViewModel _viewModel;

    public ArticleViewModelTest()
    {
        _sectionOne = BuildSection("test-slug");
        _sectionTwo = BuildSection("test-slug-section-two");
        _sectionThree = BuildSection("test-slug-section-three");
        _article = BuildArticle(string.Empty, new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });
        _viewModel = new ArticleViewModel(_article, _sectionOne.Slug);

    }

    [Fact]
    public void ArticleViewModel_SetsTheFirstSectionAsTheDisplayedSection_If_NoSectionSlugIsGiven()
    {
        // Arrange
        var article = BuildArticle(string.Empty, new List<ProcessedSection> { _sectionOne, _sectionTwo });
        
        // Act
        var viewModel = new ArticleViewModel(article);

        // Assert
        Assert.Equal(_sectionOne, viewModel.DisplayedSection);
    }

    [Fact]
    public void ArticleViewModel_ShowsArticleSummary_If_DisplayedSectionIsFirstSection()
    {
        // Assert
        Assert.True(_viewModel.ShouldShowArticleSummary);
    }

    [Fact]
    public void ArticleViewModel_DoesNotShowArticleSummary_If_DisplayedSectionIsNotTheFirstSection()
    {
        // Act
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Assert
        Assert.False(viewModel.ShouldShowArticleSummary);
    }

    [Fact]
    public void ArticleViewModel_ReturnsTheDisplayedSectionIndex()
    {
        // Act
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Assert
        Assert.Equal(2, viewModel.DisplayedSectionIndex);
    }

    [Fact]
    public void NextSection_ReturnsTheNextSection_If_TheDisplayedSectionIsNotTheLast()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.Equal(_sectionThree, viewModel.NextSection());
    }

    [Fact]
    public void NextSection_ReturnsNullForNextSection_If_DisplayedSectionIsTheLast()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionThree.Slug);

        // Act & Assert
        Assert.Null(viewModel.NextSection());
    }

    [Fact]
    public void ShouldShowNextSectionButton_ReturnsNextSection_If_DisplayedSectionIsNotTheLast()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.True(viewModel.ShouldShowNextSectionButton());
    }

    [Fact]
    public void ShouldShowNextSectionButton_DoesNotShowNextSectionButton_If_DisplayedSectionIsTheLast()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionThree.Slug);

        // Act & Assert
        Assert.False(viewModel.ShouldShowNextSectionButton());
    }

    [Fact]
    public void ReturnsThePreviousSectionIfTheDisplayedSectionIsNotTheFirst()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.Equal(_sectionOne, viewModel.PreviousSection());
    }

    [Fact]
    public void PreviousSection_ReturnsNull_If_DisplayedSectionIsTheFirst()
    {
        // Act & Assert
        Assert.Null(_viewModel.PreviousSection());
    }

    [Fact]
    public void ShouldShowPreviousSectionButton_ReturnsPreviousSectionButton_If_DisplayedSectionIsNotTheFirst()
    {
        // Arrange
        var viewModel = new ArticleViewModel(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.True(viewModel.ShouldShowPreviousSectionButton());
    }

    [Fact]
    public void ShouldShowPreviousSectionButton_DoesNotShowPreviousSectionButton_If_DisplayedSectionIsTheFirst()
    {
        // Act & Assert
        Assert.False(_viewModel.ShouldShowPreviousSectionButton());
    }

    [Fact]
    public void ArticleViewModel_ShouldSetOgTitleUsingTitleAndDisplayedSection()
    {
        // Arrange
        var article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

        // Act
        var viewModel = new ArticleViewModel(article, _sectionOne.Slug);

        // Assert
        Assert.Equal($"{viewModel.Article.Title} - {viewModel.DisplayedSection.Title}", viewModel.OgTitleMetaData);
    }

    [Fact]
    public void ArticleViewModel_ShowsArticleWhenThereAreNoSections()
    {
        // Arrange
        var article = BuildArticle("article-slug", new List<ProcessedSection> { });

        // Act
        var viewModel = new ArticleViewModel(article);
        
        // Assert
        Assert.Empty(viewModel.Article.Sections);
        Assert.Null(viewModel.DisplayedSection);
        Assert.True(viewModel.ShouldShowArticleSummary);
        Assert.Equal(viewModel.Article.Title, viewModel.OgTitleMetaData);
    }

    [Fact]
    public void ArticleViewModel_ThrowsException_IfSectionSlugNotFound()
    {
        // Arrange
        var article = BuildArticle("Article Title", new List<ProcessedSection> { _sectionOne, _sectionTwo });

        // Act & Assert
        var exception = Assert.Throws<SectionDoesNotExistException>(() => new ArticleViewModel(article, "non-existent-section-slug"));
        Assert.Equal("Section with slug: non-existent-section-slug does not exist", exception.Message);
    }

    [Fact]
    public void SidebarSubItems_ShouldReturnTopicSubItemsListForSideBar()
    {
        // Arrange
        List<SubItem> featuredItems = new() { 
            new(It.IsAny<string>(), "first-featureditem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal")
        };
        SubItem firstSubitem = new(It.IsAny<string>(), "first-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        List<SubItem> subItems = new() { firstSubitem };
        SubItem firstSecondaryitem = new(It.IsAny<string>(), "first-secondaryitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        List<SubItem> secondaryItems = new() { firstSecondaryitem };

        Topic topic = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), featuredItems, subItems, secondaryItems, new List<Crumb>(), new List<Alert>(), false, It.IsAny<string>(), null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
        ProcessedArticle article = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<ProcessedSection>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool());

        ArticleViewModel articleViewModel = new(article);

        // Act
        var sidebarSubItems = articleViewModel.SidebarSubItems(out bool showMoreButton);

        // Assert
        Assert.Equal(2, sidebarSubItems.Count());
        Assert.Equal(firstSubitem, sidebarSubItems.ToList().First());
        Assert.Equal(firstSecondaryitem, sidebarSubItems.ToList()[1]);
        Assert.False(showMoreButton);
    }

    [Fact]
    public void SidebarSubItems_ShouldReturnSixTopicsSubItemsForSideBar()
    {
        // Arrange
        SubItem firstSubItem = new(It.IsAny<string>(), "first-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem secondSubItem = new(It.IsAny<string>(), "second-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem thirdSubItem = new(It.IsAny<string>(), "third-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem fourthSubItem = new(It.IsAny<string>(), "fourth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem fifthSubItem = new(It.IsAny<string>(), "fifth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem sixthSubItem = new(It.IsAny<string>(), "sixth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem seventhSubItem = new(It.IsAny<string>(), "seventh-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem eightSubItem = new(It.IsAny<string>(), "eigth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem ninthSubItem = new(It.IsAny<string>(), "ninth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        SubItem tenthSubItem = new(It.IsAny<string>(), "tenth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");

        List<SubItem> subItems = new() { firstSubItem, secondSubItem, thirdSubItem, fourthSubItem };
        List<SubItem> secondaryItems = new() { fifthSubItem, sixthSubItem, seventhSubItem, eightSubItem };
        List<SubItem> featuredItems = new() { ninthSubItem, tenthSubItem };

        Topic topic = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            featuredItems, subItems, secondaryItems, new List<Crumb>(), new List<Alert>(), false, It.IsAny<string>(), null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
        ProcessedArticle article = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<ProcessedSection>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool());

        ArticleViewModel articleViewModel = new(article);

        // Act
        var sidebarSubItems = articleViewModel.SidebarSubItems(out bool showMoreButton);

        // Assert
        Assert.Equal(6, sidebarSubItems.Count());
        Assert.Equal(firstSubItem, sidebarSubItems.ToList().First());
        Assert.Equal(secondSubItem, sidebarSubItems.ToList()[1]);
        Assert.Equal(thirdSubItem, sidebarSubItems.ToList()[2]);
        Assert.Equal(fourthSubItem, sidebarSubItems.ToList()[3]);
        Assert.Equal(fifthSubItem, sidebarSubItems.ToList()[4]);
        Assert.Equal(sixthSubItem, sidebarSubItems.ToList()[5]);
        Assert.True(showMoreButton);
    }

    [Theory]
    [InlineData("test section meta", "test article meta", "test section meta")]
    [InlineData("test section meta", null, "test section meta")]
    [InlineData("", "test article meta", "test article meta")]
    [InlineData(null, "test article meta", "test article meta")]
    public void ArticleViewModel_ShouldSetMetaDescription(string sectionMeta, string articleMeta, string expectedMeta)
    {
        // Arrange
        ProcessedSection section = new(string.Empty, "test-slug", sectionMeta, string.Empty, null, null, null);
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, articleMeta, 
            new List<ProcessedSection> { section }, string.Empty, string.Empty, string.Empty, null, null, null, null, null, new DateTime(), new bool()
        );

        // Act
        ArticleViewModel model = new(article, "test-slug");

        // Assert
        Assert.Equal(expectedMeta, model.MetaDescription);
    }

    private static ProcessedArticle BuildArticle(string slug, List<ProcessedSection> sections)
    {
        Topic parentTopic = new("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, null, null,
            new List<Crumb>(), null, true, "test-id", null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);

        return new(It.IsAny<string>(), slug, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), sections,
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), parentTopic, new List<Alert>(), new DateTime(), new bool());
    }

    private static ProcessedSection BuildSection(string slug) => 
        new("title", slug, It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>());
}