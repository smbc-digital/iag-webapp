using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;

namespace StockportWebappTests.Unit.Startup
{
    public class StartupTest
    {
        [Theory]
        [InlineData("test")]
        [InlineData("test2")]
        public void CheckAppSettingsForEnvironments(string environment)
        {
            var env = new HostingEnvironment {EnvironmentName = environment, ContentRootPath = Directory.GetCurrentDirectory() };
            var startup = new StockportWebapp.Startup(env);

            var googleTag  = startup.Configuration["TestConfigSetting"];

            googleTag.Should().Be(environment);
        }
    }
}
