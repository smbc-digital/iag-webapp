using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;

namespace StockportWebappTests_Unit.Unit.Startup
{
    public class StartupTest
    {
        private readonly IConfiguration _configuration;

        public StartupTest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Theory(Skip = "redundant - can not pass IConfiguration to startup")]
        [InlineData("test")]
        [InlineData("test2")]
        public void CheckAppSettingsForEnvironments(string environment)
        {
            var path = Path.GetFullPath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                "..", "..", ".."));

            var env = new HostingEnvironment { EnvironmentName = environment, ContentRootPath = path };

            var startup = new StockportWebapp.Startup(_configuration, env);

            var googleTag = startup.Configuration["TestConfigSetting"];

            googleTag.Should().Be(environment);
        }
    }
}
