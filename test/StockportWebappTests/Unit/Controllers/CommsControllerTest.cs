using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class CommsControllerTest
    {
        private readonly Mock<IRepository> _mockRepository = new Mock<IRepository>();
        private readonly CommsController _controller;
//        private static readonly News ExampleNews = new News(
//            "News 2nd September",
//            "news-2nd-september",
//            "test",
//            "",
//            "",
//            "test",
//            new List<Crumb>(), new DateTime(2019, 9, 2), new DateTime(2019, 9, 2), new List<Alert>(),
//            new List<string>(), new List<Document>());

        public CommsControllerTest()
        {
            _controller = new CommsController(_mockRepository.Object);
        }

        [Fact]
        public async Task Index_ShouldGetLatestNews()
        {
            var exampleNews = new News(
                "News 2nd September",
                "news-2nd-september",
                "test",
                "purpose",
                "",
                "",
                "test",
                new List<Crumb>(), new DateTime(2019, 9, 2), new DateTime(2019, 9, 2), new List<Alert>(),
                new List<string>(), new List<Document>());

            // Arrange
            _mockRepository
                .Setup(_ => _.GetLatest<News>(It.IsAny<int>()))
                .ReturnsAsync(HttpResponse.Successful(200, new List<News>{ exampleNews }));
            _mockRepository
                .Setup(_ => _.Get<CommsHomepage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(HttpResponse.Successful(200, new CommsHomepage()));

            // Act
            var view = await _controller.Index() as ViewResult;
            var model = view.Model as CommsHomepageViewModel;

            // Assert
            model.LatestNews.Should().Be(exampleNews);
        }
    }
}
