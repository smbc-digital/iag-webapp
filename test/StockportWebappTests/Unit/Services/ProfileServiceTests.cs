using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory.Trivia;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.Repositories.Responses;
using StockportWebapp.Services.Profile;
using StockportWebapp.Services.Profile.Entities;
using StockportWebapp.Utils;
using Xunit;

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
        private readonly Mock<ITriviaFactory> _triviaFactory;
        private readonly Mock<IDynamicTagParser<InlineQuote>> _inlineQuoteTagParser;

        public ProfileServiceTests()
        {
            _triviaFactory = new Mock<ITriviaFactory>();
            _repository = new Mock<IRepository>();
            _parser = new Mock<ISimpleTagParserContainer>();
            _markdownWrapper = new MarkdownWrapper();
            _logger = new Mock<ILogger<Alert>>();
            _viewRender = new Mock<IViewRender>();
            _inlineQuoteTagParser = new Mock<IDynamicTagParser<InlineQuote>>();
            _alerts = new AlertsInlineTagParser(_viewRender.Object, _logger.Object, new FeatureToggles() { SemanticInlineAlert = true });
            _service = new ProfileService(_repository.Object, _parser.Object, _markdownWrapper, _alerts, _triviaFactory.Object, _inlineQuoteTagParser.Object);
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
                TriviaSection = new List<Trivia>(),
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
