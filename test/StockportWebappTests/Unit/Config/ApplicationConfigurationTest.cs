using System;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Moq;
using StockportWebapp.Config;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Config
{
    public class ApplicationConfigurationTest
    {
        private readonly ApplicationConfiguration _config;
        private readonly IConfiguration _configuration;

        public ApplicationConfigurationTest(IConfiguration configuration)
        {
            _configuration = configuration;

            var path = Path.GetFullPath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                "..", "..", ".."));

            var startup = new StockportWebapp.Startup(
                _configuration, 
                new HostingEnvironment
            {
                EnvironmentName = "test",
                ContentRootPath = path
            });

            var appSettings = startup.Configuration;
            _config = new ApplicationConfiguration(appSettings);
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenEmailAlertsUrlExistsForBusinessSpecificId()
        {
            var url = _config.GetEmailAlertsUrl("businessid");

            url.IsValid().Should().BeTrue();
            url.ToString().Should().Be("//url.com");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenSearchUrlExistsForSpecificBusinessId()
        {
            var url = _config.GetSearchUrl("businessid");

            url.IsValid().Should().BeTrue();
            url.ToString().Should().Be("//search-url.com");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenPostcodeSearchUrlExistsForSpecificBusinessId()
        {
            var url = _config.GetPostcodeSearchUrl("businessid");

            url.IsValid().Should().BeTrue();
            url.ToString().Should().Be("//postcode-url.com");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenGoogleAnalyticsCodeExistsForSpecificBusinessId()
        {
            var url = _config.GetGoogleAnalyticsCode("businessid");

            url.IsValid().Should().BeTrue();
            url.ToString().Should().Be("BASE");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenAddThisShareIdExistsForBusinessId()
        {
            var shareId = _config.GetAddThisShareId("businessid");

            shareId.IsValid().Should().BeTrue();
            shareId.ToString().Should().Be("share-id");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenEmailHostExistsForBusinessId()
        {
            var host = _config.GetEmailHost("businessid");

            host.IsValid().Should().BeTrue();
            host.ToString().Should().Be("a-host.com");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenEmailRegionExistsForBusinessId()
        {
            var region = _config.GetEmailRegion("businessid");

            region.IsValid().Should().BeTrue();
            region.ToString().Should().Be("eu");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldBeAValidAppSettingWhenEmailEmailFromExistsForBusinessId()
        {
            var emailFrom = _config.GetEmailEmailFrom("businessid");

            emailFrom.IsValid().Should().BeTrue();
            emailFrom.ToString().Should().Be("email@email.com");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnInvalidAppSettingWhenPropertyDoesntExist()
        {
            var url = _config.GetEmailAlertsUrl("anotherbusinessid");
            url.IsValid().Should().BeFalse();
            url.ToString().Should().Be("");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnInvalidAppSettingWhenBusinessIdDoesntExist()
        {
            var url = _config.GetEmailAlertsUrl("businessiddoesntexist");
            url.IsValid().Should().BeFalse();
            url.ToString().Should().Be("");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnContentApiUri()
        {
            var uri = _config.GetContentApiUri();

            var expectedUri = new Uri("http://localhost:80/");

            uri.Should().Be(expectedUri);
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnStaticAssetsRootUrl()
        {
            var uri = _config.GetStaticAssetsRootUrl();

            const string url = "http://assets.example.com/";

            uri.Should().Be(url);
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnGetEventSubmissionEmail()
        {
            var email = _config.GetEventSubmissionEmail("businessId");

            email.IsValid().Should().BeTrue();
            email.ToString().Should().Be("email@email.com");
        }

        [Theory(Skip = "redundant - can not pass IConfiguration to startup")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("http://something:::invalid")]
        public void ShouldThrowAnArgumentErrorIfTheContentApiUrlIsNotValid(string url)
        {
            var appsettings = new Mock<IConfigurationRoot>();
            appsettings.Setup(o => o["ContentApiUrl"]).Returns(url);
            Action act = () => new ApplicationConfiguration(appsettings.Object);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Configuration of ContentApiUrl must exist and be a valid uri!");
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnGetFooterCache()
        {
            var setting = _config.GetFooterCache("businessId");

            setting.Should().Be(5);
        }

        [Fact(Skip = "redundant - can not pass IConfiguration to startup")]
        public void ShouldReturnGetFooterCacheForNonExistantSetting()
        {
            var setting = _config.GetFooterCache("businessId_doesnotexist");

            setting.Should().Be(0);
        }
    }
}
