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
using Microsoft.Extensions.Logging;
using StockportWebapp.Parsers;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Repositories.Responses;
using StockportWebapp.Utils;
using StockportWebapp.ContentFactory.InformationFactory;

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
        private readonly Mock<IInformationFactory> _informationFactory;
        private readonly Mock<IDynamicTagParser<InlineQuote>> _inlineQuoteTagParser;

        public ProfileServiceTests()
        {
            _informationFactory = new Mock<IInformationFactory>();
            _repository = new Mock<IRepository>();
            _parser = new Mock<ISimpleTagParserContainer>();
            _markdownWrapper = new MarkdownWrapper();
            _logger = new Mock<ILogger<Alert>>();
            _viewRender = new Mock<IViewRender>();
            _inlineQuoteTagParser = new Mock<IDynamicTagParser<InlineQuote>>();
            _alerts =  new AlertsInlineTagParser(_viewRender.Object, _logger.Object, new FeatureToggles(){SemanticInlineAlert = true} );
            _service = new ProfileService(_repository.Object, _parser.Object,_markdownWrapper, _alerts, _informationFactory.Object, _inlineQuoteTagParser.Object);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnNullWhenFailure()
        {
            // Arrange
            var response = HttpResponse.Failure(500, "Test Error");
            _repository
                .Setup(_ => _.Get<ProfileResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
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
            var response = HttpResponse.Successful(200, new ProfileResponse
            {
                Body = "Test",
                Slug = "test",
                Alerts = new List<Alert>(),
                Breadcrumbs = new List<Crumb>(),
                TriviaSection = new List<InformationItem>(),
                Image = "testimage",
                Teaser = "test",
                Title = "test"
            });
            _repository
                .Setup(_ => _.Get<ProfileResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(response);
            _parser
                .Setup(_ => _.ParseAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns("testProcessedBody");

            // Act
            var result = await _service.GetProfile("testing slug");

            // Assert
            result.Should().BeOfType<ProfileEntity>();
        }
    }
}
