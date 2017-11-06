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
using StockportWebappTests.Builders;
using Newtonsoft.Json;

namespace StockportWebappTests.Unit.Repositories
{
    public class StockportApiRepositoryTests
    {
        [Fact]
        public async void GetResponse_ShouldReturnEvent()
        {
            // Arrange
            var httpClient = new Mock<IHttpClient>();
            var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
            var simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
            var logger = new Mock<ILogger<BaseRepository>>();
            var stockportApiRepository = new StockportApiRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, logger.Object);
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());
            var seralisedEvents = JsonConvert.SerializeObject(builtEvents);

            // Mock
            simpleUrlGenerator.Setup(o => o.StockportApiUrl<List<Event>>()).Returns("url");
            httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(new HttpResponse(200, seralisedEvents, string.Empty));

            // Act
            var apiResponse = await stockportApiRepository.GetResponse<List<Event>>();

            // Assert
            apiResponse.Should().NotBeNull();
            apiResponse.ShouldBeEquivalentTo(builtEvents);
        }

        [Fact]
        public async void GetResponse_LogIfExceptionIsThrown()
        {
            //Arrange
            var httpClient = new Mock<IHttpClient>();
            var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
            var simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
            var logger = new Mock<ILogger<BaseRepository>>();
            var stockportApiRepository = new StockportApiRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, logger.Object);
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());
            var seralisedEvents = JsonConvert.SerializeObject(builtEvents);

            // Mock
            simpleUrlGenerator.Setup(o => o.StockportApiUrl<List<Event>>()).Returns("url");
            httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ThrowsAsync(new System.Exception());

            // Act
            var apiResponse = await stockportApiRepository.GetResponse<List<Event>>();

            LogTesting.Assert(logger, LogLevel.Error, $"Error getting response for url url");
            apiResponse.Should().BeNull();
        }
    }
}
