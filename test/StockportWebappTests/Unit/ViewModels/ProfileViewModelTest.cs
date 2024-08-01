namespace StockportWebappTests_Unit.Unit.ViewModels;

public class ProfileViewModelTest
{
    private readonly Profile _profile = new(){ Slug="test-slug", Title="test-title", Author="test-author" };
    private readonly ProfileViewModel _viewModel;
    private readonly List<SubItem> subItems = new(){ new("slug", "title", "teaser", "icon", "type", "image", null, "colour") };
    private readonly Topic parentTopic = new("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, new List<SubItem>(), new List<SubItem>(),
            new List<Crumb>(), null, true, "test-id", null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
    private readonly Topic parentTopicWithSubItems;
    
    public ProfileViewModelTest()
    {
        parentTopicWithSubItems = new("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, subItems, subItems,
            new List<Crumb>(), null, true, "test-id", null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);

        _viewModel = new ProfileViewModel(_profile);

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
        var viewModel = new ArticleViewModel(article);

        // Act & Assert
        Assert.True(viewModel.HasParentTopicWithSubItems());
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
        var viewModel = new ArticleViewModel(article);

        // Act & Assert
        Assert.True(viewModel.HasSecondarySubItems());
    }

    [Fact]
    public void SidebarSubItems_ShouldReturnTopicSubItemsListForSideBar()
    {
        // Arrange
        List<SubItem> featuredItems = new() 
        { 
            new(It.IsAny<string>(), "first-featureditem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal")
        };
        SubItem firstSubitem = new(It.IsAny<string>(), "first-subitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        List<SubItem> subItems = new() { firstSubitem };
        SubItem firstSecondaryitem = new(It.IsAny<string>(), "first-secondaryitem", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<SubItem>(), "teal");
        List<SubItem> secondaryItems = new() { firstSecondaryitem };

        Topic topic = new(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), featuredItems, subItems, secondaryItems, new List<Crumb>(), new List<Alert>(), false, It.IsAny<string>(), null, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, string.Empty);
      
        Profile profile = new( {It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<ProcessedSection>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), new DateTime(), new bool(), new List<GroupBranding>(), It.IsAny<string>(), new List<SubItem>());

        ProfileViewModel profileViewModel = new(article);

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
}