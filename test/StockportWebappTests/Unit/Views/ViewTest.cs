
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

using FluentAssertions;
using Xunit;

using StockportWebapp.Models;
using System.Diagnostics;

namespace StockportWebappTests.Unit.Views
{
    public class ViewTest
    {
        public ViewTest()
        {
        }

        [Fact]
        public void RendersARazorTemplate()
        { 

            var html = RenderView("DisplayTemplates/Alert", new Alert("title", "sub", "the body", "Warning"));

            html.Should().Contain("the body");
        }


        public string RenderView<T>(string templateName, T model)
        {
            // Initialize the necessary services
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var renderer = provider.GetRequiredService<RazorViewToStringRenderer>();

            var content = renderer.RenderViewToString(templateName, model);

            Console.WriteLine(content);
            return content;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            services.AddSingleton(applicationEnvironment);

            var appDirectory = Directory.GetCurrentDirectory() + "../../../src/StockportWebapp";

            Console.WriteLine(appDirectory);

            var environment = new HostingEnvironment
            {
                WebRootFileProvider = new PhysicalFileProvider(appDirectory),
                ApplicationName = "StockportWebapp"
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
