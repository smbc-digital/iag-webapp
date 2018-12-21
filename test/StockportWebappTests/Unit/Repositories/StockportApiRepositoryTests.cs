using Moq;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using Xunit;
using StockportWebapp.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using StockportWebappTests_Unit.Builders;
using Newtonsoft.Json;

namespace StockportWebappTests_Unit.Unit.Repositories
{
    public class StockportApiRepositoryTests
    {
        private Mock<IHttpClient> _httpClient;
        private Mock<IApplicationConfiguration> _applicationConfiguraiton;
        private Mock<IUrlGeneratorSimple> _simpleUrlGenerator;
        private Mock<ILogger<BaseRepository>> _logger;

        public StockportApiRepositoryTests()
        {
            _httpClient = new Mock<IHttpClient>();
            _applicationConfiguraiton = new Mock<IApplicationConfiguration>();
            _simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
            _logger = new Mock<ILogger<BaseRepository>>();
        }

        [Fact]
        public async void GetResponse_ShouldReturnEvent()
        {
            // Arrange
            var stockportApiRepository = new StockportApiRepository(_httpClient.Object, _applicationConfiguraiton.Object, _simpleUrlGenerator.Object, _logger.Object);
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());
            var seralisedEvents = JsonConvert.SerializeObject(builtEvents);

            // Mock
            _simpleUrlGenerator.Setup(o => o.StockportApiUrl<List<Event>>()).Returns("url");
            _httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(new HttpResponse(200, seralisedEvents, string.Empty));

            // Act
            var apiResponse = await stockportApiRepository.GetResponse<List<Event>>();

            // Assert
            apiResponse.Should().NotBeNull();
            apiResponse.Should().BeEquivalentTo(builtEvents);
        }

        [Fact]
        public async void GetResponseWithSlugAndQueries_ShouldReturnEvent()
        {
            // Arrange
            var stockportApiRepository = new StockportApiRepository(_httpClient.Object, _applicationConfiguraiton.Object, _simpleUrlGenerator.Object, _logger.Object);
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());
            var seralisedEvents = JsonConvert.SerializeObject(builtEvents);

            // Mock
            _simpleUrlGenerator.Setup(o => o.StockportApiUrl<List<Event>>()).Returns("url");
            _httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(new HttpResponse(200, seralisedEvents, string.Empty));

            // Act
            var apiResponse = await stockportApiRepository.GetResponse<List<Event>>("slug", new List<Query>() { new Query("name","value")});

            // Assert
            apiResponse.Should().NotBeNull();
            apiResponse.Should().BeEquivalentTo(builtEvents);
        }

        [Fact]
        public async void GetResponse_LogIfExceptionIsThrown()
        {
            //Arrange
            var stockportApiRepository = new StockportApiRepository(_httpClient.Object, _applicationConfiguraiton.Object, _simpleUrlGenerator.Object, _logger.Object);
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());
            var seralisedEvents = JsonConvert.SerializeObject(builtEvents);

            // Mock
            _simpleUrlGenerator.Setup(o => o.StockportApiUrl<List<Event>>()).Returns("url");
            _httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ThrowsAsync(new System.Exception());

            // Act
            var apiResponse = await stockportApiRepository.GetResponse<List<Event>>();

            LogTesting.Assert(_logger, LogLevel.Error, "Error getting response for url url");
            apiResponse.Should().BeNull();
        }
    }
}
