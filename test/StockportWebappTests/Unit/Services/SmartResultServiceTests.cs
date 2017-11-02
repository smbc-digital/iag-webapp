using FluentAssertions;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using Xunit;
using Microsoft.Extensions.Logging;
using StockportWebappTests.Builders;
using System;
using System.Collections.Generic;
using StockportWebapp.Wrappers;
using System.Net.Http;
using StockportWebapp.Entities;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Services
{
    public class SmartResultServiceTests
    {
        private readonly Mock<ISmartResultRepository> _repository;
        private readonly SmartResultService _service;

        public SmartResultServiceTests()
        {
            _repository = new Mock<ISmartResultRepository>();
            _service = new SmartResultService(_repository.Object, new Mock<ILogger<SmartResultService>>().Object, null);
        }

        [Fact]
        public async void GetSmartResult_ShouldCallRepository()
        {
            // Arrange
            const string slug = "a-slug";
            var model = new SmartResult()
            {
                Body = "body",
                Slug = slug
            };

            _repository.Setup(_ => _.GetSmartResult(slug)).ReturnsAsync(model);

            // Act
            await _service.GetSmartResult(slug);

            // Assert
            _repository.Verify(_ => _.GetSmartResult(slug),Times.Once);
        }

        [Fact]
        public async void GetSmartResult_ShouldReturnCorrectResultEntity()
        {
            // Arrange
            const string slug = "a-slug";
            var model = new SmartResult()
            {
                Body = "body",
                Slug = slug,
                Icon = "Exclamation Mark"
            };

            _repository.Setup(_ => _.GetSmartResult(slug)).ReturnsAsync(model);

            // Act
            var result = await _service.GetSmartResult(slug);

            // Assert
            result.Slug.Should().Be(model.Slug);
            result.Body.Should().Be(model.Body);
            result.IconClass.Should().Be("exclamation");
            result.IconColour.Should().Be("red");
        }

    }

}
