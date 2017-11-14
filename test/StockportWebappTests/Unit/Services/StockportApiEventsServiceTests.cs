using FluentAssertions;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using Xunit;
using Microsoft.Extensions.Logging;
using StockportWebappTests.Builders;
using System;
using StockportWebapp.Wrappers;
using System.Net.Http;
using StockportWebapp.Utils;
using System.Collections.Generic;
using StockportWebapp.Models;
using System.Linq;
using StockportWebapp.ContentFactory;

namespace StockportWebappTests.Unit.Services
{
    public class StockportApiEventsServiceTests
    {
        private Mock<IStockportApiRepository> _stockportApiRepository = new Mock<IStockportApiRepository>();
        private Mock<IUrlGeneratorSimple> _urlGeneratorSimple = new Mock<IUrlGeneratorSimple>();
        private StockportApiEventsService StockportApiEventsService;
        private Mock<IEventFactory> _eventFactory = new Mock<IEventFactory>();

        public StockportApiEventsServiceTests()
        {
            StockportApiEventsService = new StockportApiEventsService(_stockportApiRepository.Object, _urlGeneratorSimple.Object, _eventFactory.Object);
        }

        [Fact]
        public async void GetEventsByCategory_ShouldReturnListOfEventsWhenCategorySet()
        {
            //Arrange
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());       

            //Mock
            _stockportApiRepository.Setup(x => x.GetResponse<List<Event>>("by-category", It.IsAny<List<Query>>())).ReturnsAsync(builtEvents);

            //Act
            var result = await StockportApiEventsService.GetEventsByCategory("Fayre");

            //Assert
            result.Should().HaveCount(1);
            result.First().Title.Should().Be("title");
        }

        [Fact]
        public async void GetEventsByCategory_ShouldReturnNullWhenCategorySetIsEmpty()
        {
            //Arrange
            var builtEvents = new List<Event>();
            builtEvents.Add(new EventBuilder().Build());

            //Mock
            _stockportApiRepository.Setup(x => x.GetResponse<List<Event>>("by-category", It.IsAny<List<Query>>())).ReturnsAsync((List<Event>)null);

            //Act
            var result = await StockportApiEventsService.GetEventsByCategory("");

            //Assert
            result.Should().BeNull();
        }
    }
}
