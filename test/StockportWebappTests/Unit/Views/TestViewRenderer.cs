
using System;
using System.IO;

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.DependencyInjection;
using RenderRazorToString;
using System.Diagnostics;

namespace StockportWebappTests.Unit.Views
{
    public class TestViewRenderer
    {
        private string appPath;
        private string appName;

        public TestViewRenderer(string appPath, string appName)
        {
            this.appPath = appPath;
            this.appName = appName;
        }

        public string RenderView<T>(string templateName, T model)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var renderer = provider.GetRequiredService<RazorViewToStringRenderer>();

            var content = renderer.RenderViewToString(templateName, model);
            return content;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            services.AddSingleton(applicationEnvironment);

            var appDirectory = Directory.GetCurrentDirectory() + "../../../" + appPath + "/" + appName;

            var environment = new HostingEnvironment
            {
                WebRootFileProvider = new PhysicalFileProvider(appDirectory),
                ApplicationName = appName
            };
            services.AddSingleton<IHostingEnvironment>(environment);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(new PhysicalFileProvider(appDirectory));
            });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<DiagnosticSource>(diagnosticSource);

            services.AddLogging();
            services.AddMvc();
            services.AddSingleton<RazorViewToStringRenderer>();
        }
    }

}
