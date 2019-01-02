using System;
using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Collections.Generic;
using Xunit;
using StockportWebappTests_Unit.Builders;

namespace StockportWebappTests_Unit.Unit.Helpers
{
    public class GroupImageURLHelperTest
    {
        [Fact]
        public void ShouldReturnGroupsImageUrlIfExists()
        {
            // Arrange
            var groupWithImage = new GroupBuilder().Build();

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be(groupWithImage.ImageUrl);
        }

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
        public void ShouldReturnEmptyStringIfTheGroupImageUrlisEmptyAndThereAreNoGroupCategories()
        {
            // Arrange
            var group = new GroupBuilder().Image(string.Empty).Build();

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(group);

            // Assert
            outputImageUrl.Should().Be("");
        }

        [Fact]
        public void ShouldReturnSecondCategoryImageUrlIfGroupImageUrlIsEmptyAndFirstCategoryImageUrlIsEmptyButSecondCategoryImageUrlIsNot()
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
