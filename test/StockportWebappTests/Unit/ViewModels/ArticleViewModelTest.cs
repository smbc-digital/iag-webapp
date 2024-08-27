namespace StockportWebappTests_Unit.Unit.ViewModels;

public class ArticleViewModelTest
{
    private readonly ProcessedSection _sectionOne;
    private readonly ProcessedSection _sectionTwo;
    private readonly ProcessedSection _sectionThree;
    private readonly ProcessedArticle _article;
    private readonly ArticleViewModel _viewModel;
    private readonly List<SubItem> subItems = new(){ new("slug", "title", "teaser", "icon", "type", "contentType", "image", string.Empty, "body text", null, string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty) };
    private readonly Topic parentTopic = new("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, new List<SubItem>(), new List<SubItem>(),
            new List<Crumb>(), null, true, "test-id", null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
    private readonly Topic parentTopicWithSubItems;
    
    public ArticleViewModelTest()
    {
        _sectionOne = BuildSection("test-slug");
        _sectionTwo = BuildSection("test-slug-section-two");
        _sectionThree = BuildSection("test-slug-section-three");
        _article = BuildArticle(string.Empty, new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree }, parentTopic);
        parentTopicWithSubItems = new("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, subItems, subItems,
            new List<Crumb>(), null, true, "test-id", null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);

        _viewModel = new(_article, _sectionOne.Slug);

    }

    [Fact]
    public void ArticleViewModel_SetsTheFirstSectionAsTheDisplayedSection_If_NoSectionSlugIsGiven()
    {
        // Arrange
        ProcessedArticle article = BuildArticle(string.Empty, new List<ProcessedSection> { _sectionOne, _sectionTwo }, parentTopic);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.Equal(_sectionOne, viewModel.DisplayedSection);
    }

    [Fact]
    public void ArticleViewModel_ShowsArticleSummary_If_DisplayedSectionIsFirstSection()
    {
        // Act & Assert
        Assert.True(_viewModel.ShouldShowArticleSummary);
    }

    [Fact]
    public void ArticleViewModel_DoesNotShowArticleSummary_If_DisplayedSectionIsNotTheFirstSection()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.False(viewModel.ShouldShowArticleSummary);
    }

    [Fact]
    public void ArticleViewModel_ReturnsTheDisplayedSectionIndex()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.Equal(2, viewModel.DisplayedSectionIndex);
    }

    [Fact]
    public void NextSection_ReturnsTheNextSection_If_TheDisplayedSectionIsNotTheLast()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.Equal(_sectionThree, viewModel.NextSection());
    }

    [Fact]
    public void NextSection_ReturnsNullForNextSection_If_DisplayedSectionIsTheLast()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionThree.Slug);

        // Act & Assert
        Assert.Null(viewModel.NextSection());
    }

    [Fact]
    public void ShouldShowNextSectionButton_ReturnsNextSection_If_DisplayedSectionIsNotTheLast()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

        // Act & Assert
        Assert.True(viewModel.ShouldShowNextSectionButton());
    }

    [Fact]
    public void ShouldShowNextSectionButton_DoesNotShowNextSectionButton_If_DisplayedSectionIsTheLast()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionThree.Slug);

        // Act & Assert
        Assert.False(viewModel.ShouldShowNextSectionButton());
    }

    [Fact]
    public void ReturnsThePreviousSectionIfTheDisplayedSectionIsNotTheFirst()
    {
        // Arrange
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

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
        ArticleViewModel viewModel = new(_article, _sectionTwo.Slug);

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
    public void HasParentTopicWithSubItems_ReturnsFalse_If_ParentTopicIsNull()
    {
        // Act & Assert
        Assert.False(_viewModel.HasParentTopicWithSubItems());
    }

    [Fact]
    public void HasParentTopicWithSubItems_ReturnsTrue_If_SubItemsHasItems()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne }, parentTopicWithSubItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.True(viewModel.HasParentTopicWithSubItems());
    }

    [Fact]
    public void HasRelatedContentWithSubItems_ReturnsFalse_If_RelatedContentIsNull()
    {
        // Act & Assert
        Assert.False(_viewModel.HasRelatedContentWithSubItems());
    }

    [Fact]
    public void HasRelatedContentWithSubItems_ReturnsTrue_If_RelatedContentHasItems()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne }, parentTopicWithSubItems, subItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.True(viewModel.HasRelatedContentWithSubItems());
    }

    [Fact]
    public void HasSecondarySubItems_ReturnsFalse_If_SecondaryItemsEmpty()
    {
        // Act & Assert
        Assert.False(_viewModel.HasSecondarySubItems());
    }

    [Fact]
    public void HasSecondarySubItems_ReturnsTrue_If_SecondaryItemsHasItems()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne }, parentTopicWithSubItems, subItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.True(viewModel.HasSecondarySubItems());
    }
    
    [Fact]
    public void ArticleWithSection_ReturnsFalse_If_SectionIsNull()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { null }, parentTopicWithSubItems, subItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.False(viewModel.ArticleWithSection);
    }

    [Fact]
    public void ArticleWithSection_ReturnsFalse_If_DisplayedSectionIsNull()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", null, parentTopicWithSubItems, subItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.False(viewModel.ArticleWithSection);
    }

    [Fact]
    public void ArticleWithSection_ReturnsTrue_If_SectionHasItems_And_DisplayedSectionIsNotNull()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree }, parentTopicWithSubItems, subItems);
        ArticleViewModel viewModel = new(article);

        // Act & Assert
        Assert.True(viewModel.ArticleWithSection);
    }

    [Fact]
    public void ArticleWithImage_ReturnsTrue_If_ImageIsNull()
    {
        // Act & Assert
        Assert.True(_viewModel.ArticleWithImage);
    }

    [Fact]
    public void ArticleWithImage_ReturnsFalse_If_ImageHasValue()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree }, parentTopic, null, "image");
        ArticleViewModel viewModel = new(article, _sectionOne.Slug);

        // Act & Assert
        Assert.False(viewModel.ArticleWithImage);
    }

    [Fact]
    public void ArticleViewModel_ShouldSetOgTitleUsingTitleAndDisplayedSection()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("Article title", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree }, parentTopic);

        // Act
        ArticleViewModel viewModel = new(article, _sectionOne.Slug);

        // Assert
        Assert.Equal($"{viewModel.Article.Title} - {viewModel.DisplayedSection.Title}", viewModel.OgTitleMetaData);
    }

    [Fact]
    public void ArticleViewModel_ShowsArticleWhenThereAreNoSections()
    {
        // Arrange
        ProcessedArticle article = BuildArticle("article-slug", new List<ProcessedSection> { }, parentTopic);

        // Act
        ArticleViewModel viewModel = new(article);
        
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
        ProcessedArticle article = BuildArticle("Article Title", new List<ProcessedSection> { _sectionOne, _sectionTwo }, parentTopic);

        // Act & Assert
        SectionDoesNotExistException exception = Assert.Throws<SectionDoesNotExistException>(() => new ArticleViewModel(article, "non-existent-section-slug"));
        Assert.Equal("Section with slug: non-existent-section-slug does not exist", exception.Message);
    }

    [Fact]
    public void SidebarSubItems_ShouldReturnTopicSubItemsListForSideBar()
    {
        // Arrange
        List<SubItem> featuredItems = new() 
        { 
            new(It.IsAny<string>(), "first-featureditem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty)
        };
        SubItem firstSubitem = new(It.IsAny<string>(), "first-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        List<SubItem> subItems = new() { firstSubitem };
        SubItem firstSecondaryitem = new(It.IsAny<string>(), "first-secondaryitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        List<SubItem> secondaryItems = new() { firstSecondaryitem };

        Topic topic = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), featuredItems, subItems, secondaryItems, new List<Crumb>(), new List<Alert>(), false, It.IsAny<string>(), null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
      
        ProcessedArticle article = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<ProcessedSection>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), It.IsAny<string>(), new List<SubItem>());

        ArticleViewModel articleViewModel = new(article);

        // Act
        IEnumerable<SubItem> sidebarSubItems = articleViewModel.SidebarSubItems(out bool showMoreButton);

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
        SubItem firstSubItem = new(It.IsAny<string>(), "first-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem secondSubItem = new(It.IsAny<string>(), "second-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem thirdSubItem = new(It.IsAny<string>(), "third-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem fourthSubItem = new(It.IsAny<string>(), "fourth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem fifthSubItem = new(It.IsAny<string>(), "fifth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem sixthSubItem = new(It.IsAny<string>(), "sixth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem seventhSubItem = new(It.IsAny<string>(), "seventh-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem eightSubItem = new(It.IsAny<string>(), "eigth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem ninthSubItem = new(It.IsAny<string>(), "ninth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);
        SubItem tenthSubItem = new(It.IsAny<string>(), "tenth-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty);

        List<SubItem> subItems = new() { firstSubItem, secondSubItem, thirdSubItem, fourthSubItem };
        List<SubItem> secondaryItems = new() { fifthSubItem, sixthSubItem, seventhSubItem, eightSubItem };
        List<SubItem> featuredItems = new() { ninthSubItem, tenthSubItem };

        Topic topic = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            featuredItems, subItems, secondaryItems, new List<Crumb>(), new List<Alert>(), false, It.IsAny<string>(), null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
        ProcessedArticle article = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<ProcessedSection>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), It.IsAny<string>(), new List<SubItem>());

        ArticleViewModel articleViewModel = new(article);

        // Act
        IEnumerable<SubItem> sidebarSubItems = articleViewModel.SidebarSubItems(out bool showMoreButton);

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
        ProcessedSection section = new(string.Empty, "test-slug", sectionMeta, string.Empty, null, null, null, null, string.Empty, new DateTime());
        ProcessedArticle article = new(string.Empty, string.Empty, string.Empty, string.Empty, articleMeta, 
            new List<ProcessedSection> { section }, string.Empty, It.IsAny<string>(), string.Empty, null, null, null, null, null, new DateTime(), new bool(), new List<GroupBranding>(), string.Empty, new List<SubItem>()
        );

        // Act
        ArticleViewModel model = new(article, "test-slug");

        // Assert
        Assert.Equal(expectedMeta, model.MetaDescription);
    }

    private static ProcessedArticle BuildArticle(string slug, List<ProcessedSection> sections, Topic topic, List<SubItem> relatedContent=null, string image="") 
        => new(It.IsAny<string>(), slug, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), sections,
            It.IsAny<string>(), It.IsAny<string>(), image, It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), It.IsAny<string>(), relatedContent);

    private static ProcessedSection BuildSection(string slug) => 
        new("title", slug, It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
}