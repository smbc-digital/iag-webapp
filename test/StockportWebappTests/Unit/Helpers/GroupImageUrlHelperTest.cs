using System;
using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Collections.Generic;
using Xunit;

namespace StockportWebappTests.Unit.Helpers
{
    public class GroupImageURLHelperTest
    {

        public GroupImageURLHelperTest()
        {
        }

        [Fact]
        public void ShouldReturnGroupsImageUrlIfExists()
        {
            // Arrange
            GroupCategory groupCategory = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "categoryImageUrl",
                Name = "name",
                Slug = "slug"
            };
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "groupImageUrl", "thumbnail", new List<GroupCategory>() { groupCategory }, new List<GroupSubCategory>(), new List<Crumb>(), new MapPosition(), false, null, null, DateTime.MinValue, DateTime.MinValue, "published", string.Empty, string.Empty, string.Empty, false, "", null, false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("groupImageUrl");
        }

        [Fact]
        public void ShouldReturnFirstCategoryImageUrlIfGroupImageUrlIsEmptyButFirstCategoryImageUrlIsNot()
        {
            // Arrange
            GroupCategory groupCategory = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "categoryImageUrl",
                Name = "name",
                Slug = "slug"
            };
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>() { groupCategory }, new List<GroupSubCategory>(), new List<Crumb>(), new MapPosition(), false, null, null, DateTime.MinValue, DateTime.MinValue, "published", string.Empty, string.Empty, string.Empty, false, string.Empty, null, false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

        [Fact]
        public void ShouldReturnEmptyStringIfTheGroupImageUrlisEmptyAndThereAreNoGroupCategories()
        {
            // Arrange
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>(), new List<GroupSubCategory>(), new List<Crumb>(), new MapPosition(), false, null, null, DateTime.MinValue, DateTime.MinValue, "published", string.Empty, string.Empty, string.Empty, false, string.Empty, null, false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("");
        }

        [Fact]
        public void ShouldReturnSecondCategoryImageUrlIfGroupImageUrlIsEmptyAndFirstCategoryImageUrlIsEmptyButSecondCategoryImageUrlIsNot()
        {
            // Arrange
            GroupCategory groupCategoryWithOutImageUrl = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "",
                Name = "name",
                Slug = "slug"
            };
            GroupCategory groupCategoryWithImageUrl = new GroupCategory()
            {
                Icon = "icon",
                ImageUrl = "categoryImageUrl",
                Name = "name",
                Slug = "slug"
            };

            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>() { groupCategoryWithOutImageUrl, groupCategoryWithImageUrl }, new List<GroupSubCategory>(), new List<Crumb>(), new MapPosition(), false, null, null, DateTime.MinValue, DateTime.MinValue, "published", string.Empty, string.Empty, string.Empty, false, string.Empty, null, false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

    }
}
