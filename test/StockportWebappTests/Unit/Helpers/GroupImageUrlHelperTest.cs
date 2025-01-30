namespace StockportWebappTests_Unit.Unit.Helpers;

public class GroupImageURLHelperTest
{
    [Fact]
    public void ShouldReturnFirstCategoryImageUrlIfGroupImageUrlIsEmptyButFirstCategoryImageUrlIsNot()
    {
        // Arrange
        GroupCategory groupCategory = new()
        {
            Icon = "icon",
            ImageUrl = "categoryImageUrl",
            Name = "name",
            Slug = "slug"
        };

        Group group = new GroupBuilder().Image(string.Empty).Categories(new List<GroupCategory> { { groupCategory } }).Build();

        // Act
        string outputImageUrl = GroupImageUrlHelper.GetImageUrl(group);

        // Assert
        Assert.Equal("categoryImageUrl", outputImageUrl);
    }

    [Fact]
    public void ShouldReturnEmptyStringIfThereAreNoGroupCategories()
    {
        // Arrange
        Group group = new GroupBuilder().Image(string.Empty).Build();

        // Act
        string outputImageUrl = GroupImageUrlHelper.GetImageUrl(group);

        // Assert
        Assert.Empty(outputImageUrl);
    }

    [Fact]
    public void ShouldReturnSecondCategoryImageUrlIfFirstCategoryImageUrlIsEmptyButSecondCategoryImageUrlIsNot()
    {
        // Arrange
        GroupCategory groupCategoryWithOutImageUrl = new()
        {
            Icon = "icon",
            ImageUrl = "",
            Name = "name",
            Slug = "slug"
        };

        GroupCategory groupCategoryWithImageUrl = new()
        {
            Icon = "icon",
            ImageUrl = "categoryImageUrl",
            Name = "name",
            Slug = "slug"
        };

        Group groupWithImage = new GroupBuilder().Image(string.Empty).Categories(new List<GroupCategory> { { groupCategoryWithOutImageUrl }, { groupCategoryWithImageUrl } }).Build();

        // Act
        string outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

        // Assert
        Assert.Equal("categoryImageUrl", outputImageUrl);
    }
}