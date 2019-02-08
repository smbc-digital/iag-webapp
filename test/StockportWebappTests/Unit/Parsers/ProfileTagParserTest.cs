using System.Collections.Generic;
using Moq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class ProfileTagParserTest
    {
        private readonly Mock<IViewRender> _viewRenderer;
        private readonly ProfileTagParser _profileTagParser;
        private readonly Mock<ILogger<ProfileTagParser>> _mockLogger;

        public ProfileTagParserTest()
        {
            _viewRenderer = new Mock<IViewRender>();
            _mockLogger = new Mock<ILogger<ProfileTagParser>>();
            _profileTagParser = new ProfileTagParser(_viewRenderer.Object, _mockLogger.Object);
        }

        [Fact]
        public void ShouldReplaceProfileTagWithProfileView()
        {
            var content = "this is some test {{PROFILE:some-slug}}";
            var profile = new Profile
            {
                Slug = "some-slug",
                Title = "some-title",
                Image = "some-image-url",
                Subtitle = "some-subtitle",
                Body = "some-body"
            };
            var profiles = new List<Profile>() { profile };
            var renderResult = "RENDERED PROFILE CONTENT";

            _viewRenderer.Setup(o => o.Render("Profile", profile)).Returns(renderResult);

            var parsedHtml = _profileTagParser.Parse(content, profiles);

            _viewRenderer.Verify(o => o.Render("Profile", profile), Times.Once);
            parsedHtml.Should().Contain(renderResult);
        }

        [Fact]
        public void ShouldReplaceProfileTagWithProfileViewWithoutBody_WhenBodyNullOrEmpty()
        {
            var content = "this is some test {{PROFILE:some-slug}}";
            var profile = new Profile
            {
                Slug = "some-slug",
                Title = "some-title",
                Image = "some-image-url",
                Subtitle = "some-subtitle",
                Body = ""
            };
            var profiles = new List<Profile>() { profile };
            var renderResult = "RENDERED PROFILE CONTENT";

            _viewRenderer.Setup(o => o.Render("ProfileWithoutBody", profile)).Returns(renderResult);

            var parsedHtml = _profileTagParser.Parse(content, profiles);

            _viewRenderer.Verify(o => o.Render("ProfileWithoutBody", profile), Times.Once);
            parsedHtml.Should().Contain(renderResult);
        }

        [Fact]
        public void ShouldRemoveProfileTagsThatDontExist()
        {
            var content = "this is a test{{PROFILE:some-slug}}";
            var parsedHtml = _profileTagParser.Parse(content, new List<Profile>());
            _viewRenderer.Verify(o => o.Render("Profile", It.IsAny<Profile>()), Times.Never);
            parsedHtml.Should().Be("this is a test");
        }
    }
}
