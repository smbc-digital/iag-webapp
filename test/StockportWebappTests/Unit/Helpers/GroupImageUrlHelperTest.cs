using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Helpers;
using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Utils;
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
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "groupImageUrl", "thumbnail", new List<GroupCategory>() { groupCategory }, new List<Crumb>(), new MapPosition(), false);

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
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>() { groupCategory }, new List<Crumb>(), new MapPosition(), false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

        [Fact]
        public void ShouldReturnEmptyStringIfTheGroupImageUrlisEmptyAndThereAreNoGroupCategories()
        {
            // Arrange
            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>(), new List<Crumb>(), new MapPosition(), false);

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

            Group groupWithImage = new Group("name", "slug", "phoneNumber", "email", "website", "twitter", "facebook", "address", "description", "", "thumbnail", new List<GroupCategory>() { groupCategoryWithOutImageUrl, groupCategoryWithImageUrl }, new List<Crumb>(), new MapPosition(), false);

            // Act
            var outputImageUrl = GroupImageUrlHelper.GetImageUrl(groupWithImage);

            // Assert
            outputImageUrl.Should().Be("categoryImageUrl");
        }

    }
}
