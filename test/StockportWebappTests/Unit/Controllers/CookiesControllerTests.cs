using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class CookiesControllerTests
    {
        private readonly Mock<ICookiesHelper> _mockFavouritesHelper;
        private readonly CookiesController _cookiesController;

        public CookiesControllerTests()
        {
            _mockFavouritesHelper = new Mock<ICookiesHelper>();
            _cookiesController = new CookiesController(_mockFavouritesHelper.Object);
        }

//        [Fact]
//        public void GetCookies_ShouldReturnListOfCookies()
//        {
//            // Arrange
//            _mockFavouritesHelper.Setup(_ => _.GetCookies<string>(It.IsAny<string>())).Returns(new List<string>
//            {
//                "any string"
//            });
//
//            // Act
//            List<string> result = _cookiesController.GetCookies(string.Empty);
//
//            // Assert
//            result.Count.Should().Equals(1);
//
//        }
//
//        [Fact]
//        public void GetCookies_ShouldCallCookiesHelper()
//        {
//            // Arrange
//            _mockFavouritesHelper.Setup(_ => _.GetCookies<string>(It.IsAny<string>())).Returns( new List<string>
//            {
//                "any string"
//            });
//
//            // Act
//            var result = _cookiesController.GetCookies(string.Empty);
//
//            // Assert
//            _mockFavouritesHelper.Verify(_ => _.GetCookies<string>(It.IsAny<string>()), Times.Once);
//            
//        }
    }
}
