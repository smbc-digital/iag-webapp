using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebappTests_Unit.Builders;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Helpers
{
    public class GroupImageURLHelperTest
    {
        [Fact]
        public void ShouldReturnFirstCategoryImageUrlIfGroupImageUrlIsEmptyButFirstCategoryImageUrlIsNot()
        {
            // Arrange
            var groupCategory = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "categoryImageUrl",
                Name = "name",
                Slug = "slug"
            };

            var group = new GroupBuilder().Image(string.Empty).Categories(new List<GroupCategory> { { groupCategory } }).Build();

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(group);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

        [Fact]
        public void ShouldReturnEmptyStringIfThereAreNoGroupCategories()
        {
            // Arrange
            var group = new GroupBuilder().Image(string.Empty).Build();

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(group);

            // Assert
            outputImageUrl.Should().Be("");
        }

        [Fact]
        public void ShouldReturnSecondCategoryImageUrlIfFirstCategoryImageUrlIsEmptyButSecondCategoryImageUrlIsNot()
        {
            // Arrange
            var groupCategoryWithOutImageUrl = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "",
                Name = "name",
                Slug = "slug"
            };
            var groupCategoryWithImageUrl = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "categoryImageUrl",
                Name = "name",
                Slug = "slug"
            };

            var groupWithImage = new GroupBuilder().Image(string.Empty).Categories(new List<GroupCategory> { { groupCategoryWithOutImageUrl }, { groupCategoryWithImageUrl } }).Build();

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

    }
}
