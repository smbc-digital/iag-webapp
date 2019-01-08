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
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using StockportWebapp.Parsers;
using StockportTagHelpers;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Utils;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace StockportWebappTests_Unit.Unit.Services
{
    public class ProfileServiceTests
    {
        private readonly ProfileService _service;
        private readonly Mock<IRepository> _repository;
        private readonly Mock<ISimpleTagParserContainer> _parser;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Alert> _alerts;
        private readonly Mock<ILogger<Alert>> _logger;
        private readonly Mock<IViewRender> _viewRender;
        public ProfileServiceTests()
        {
            _repository = new Mock<IRepository>();
            _parser = new Mock<ISimpleTagParserContainer>();
            _markdownWrapper = new MarkdownWrapper();
            _logger = new Mock<ILogger<Alert>>();
            _viewRender = new Mock<IViewRender>();
            _alerts =  new AlertsInlineTagParser(_viewRender.Object, _logger.Object, new FeatureToggles(){SemanticProfile = true} );
            _service = new ProfileService(_repository.Object, _parser.Object,_markdownWrapper, _alerts );
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
