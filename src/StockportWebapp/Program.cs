using Microsoft.AspNetCore;

namespace StockportWebapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder<Startup>(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.Sources.Clear();
                    config.SetBasePath(hostContext.HostingEnvironment.ContentRootPath + "/app-config");
                    config
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json")
                        .AddJsonFile("appversion.json", true);
                    var tempConfig = config.Build();
                    config.AddJsonFile(
                        $"{tempConfig["secrets-location"]}/appsettings.{hostContext.HostingEnvironment.EnvironmentName}.secrets.json");
                });

    }
}
