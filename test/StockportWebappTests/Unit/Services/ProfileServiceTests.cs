using FluentAssertions;
using Moq;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using Xunit;
using StockportWebapp.Services.Profile;
using StockportWebapp.Http;
using StockportWebapp.Services.Profile.Entities;
using StockportWebapp.Models;
using System.Threading.Tasks;

namespace StockportWebappTests_Unit.Unit.Services
{
    public class ProfileServiceTests
    {
        private readonly ProfileService _service;
        private readonly Mock<IRepository> _repository;

        public ProfileServiceTests()
        {
            _repository = new Mock<IRepository>();

            _service = new ProfileService(_repository.Object);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnNullWhenFailure()
        {
            // Arrange
            var response = HttpResponse.Failure(500, "Test Error");
            _repository
                .Setup(_ => _.Get<ProfileEntity>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetProfile("testing slug");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetProfile_ShouldReturnProfileWhenSuccessful()
        {
            // Arrange
            var response = HttpResponse.Successful(200, new ProfileEntity());
            _repository
                .Setup(_ => _.Get<ProfileEntity>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetProfile("testing slug");

            // Assert
            result.Should().BeOfType<ProfileEntity>();
        }
    }
}
