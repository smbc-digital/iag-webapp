using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace StockportWebappTests.Unit.Startup
{
    public class StartupTest
    {
        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        public void CheckAppSettingsForEnvironments(string environment)
        {
            var path = Path.GetFullPath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                "..", "..", ".."));

            var env = new HostingEnvironment {EnvironmentName = environment, ContentRootPath = path };
            var startup = new StockportWebapp.Startup(env, new LoggerFactory());

            var googleTag  = startup.Configuration["TestConfigSetting"];

            googleTag.Should().Be(environment);
        }
    }
}
