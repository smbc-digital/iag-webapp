using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using StockportWebapp;
using StockportWebapp.Http;
using StockportWebappTests.Unit.Http;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using StockportWebapp.AmazonSES;
using StockportWebappTests.Unit.Fake;
using System.Reflection;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace StockportWebappTests
{
    public class TestAppFactory
    {
        public static TestServer MakeFakeApp(string businessId, string environment)
        {
            Environment.SetEnvironmentVariable("SES_ACCESS_KEY", "access-key");
            Environment.SetEnvironmentVariable("SES_SECRET_KEY", "secret-key");

            var hostBuilder = new WebHostBuilder()
                .UseStartup<FakeStartup>()
                .UseKestrel()
                .UseEnvironment(environment)
                //.ConfigureTestContent()
                .ConfigureTestServices()
                .UseContentRoot(Path.GetFullPath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    "..", "..", "..", "..", "..", "src", "StockportWebapp")));

            return new TestServer(hostBuilder);
        }
    }

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureTestServices(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services =>
            {
                services.AddMvcCore();
                services.Configure((Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions options) =>
                {
                    var previous = options.CompilationCallback;
                    options.CompilationCallback = (context) =>
                    {
                        previous?.Invoke(context);

                        var assembly = typeof(Startup).GetTypeInfo().Assembly;

                        var assemblies = assembly.GetReferencedAssemblies()
                                                 .Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location))
                                                 .ToList();
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.Corelib")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Threading.Tasks")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Dynamic.Runtime")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor.Runtime")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Html.Abstractions")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Text.Encodings.Web")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq.Expressions")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Humanizer")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("jose-jwt")).Location));
                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Security.Cryptography.X509Certificates")).Location));

                        // 

                        context.Compilation = context.Compilation.AddReferences(assemblies);
                    };
                });
            });
        }
    }

    public class FakeStartup : Startup
    {
        public FakeStartup(IHostingEnvironment env) : base(env)
        {
            
        }

        public LoggingHttpClient GetHttpClient(ILoggerFactory loggerFactory)
        {
            var fakeHttpClient = new FakeHttpClientFactory().Client;
            
            return new LoggingHttpClient(fakeHttpClient, loggerFactory.CreateLogger<LoggingHttpClient>());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IHttpClient>(p => GetHttpClient(p.GetService<ILoggerFactory>()));
            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => new System.Net.Http.HttpClient(new FakeResponseHandlerFactory().ResponseHandler));
            services.AddSingleton<IHttpEmailClient, FakeHttpEmailClient>();
            services.AddMvc(options => 
            {
                for (var i = 0; i < options.Filters.Count; i++)
                {
                    options.Filters.RemoveAt(i);
                }
            });
        }
    }

    internal class FakeResponseHandlerFactory : TestingBaseClass
    {
        public FakeResponseHandler ResponseHandler { get; private set; }

        public FakeResponseHandlerFactory()
        {
            ResponseHandler = new FakeResponseHandler();
            var urlsDict = new Dictionary<Uri, string>
            {
                {new Uri("http://localhost:5001/_healthcheck"), GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Healthcheck.json")}
            };
            foreach (var url in urlsDict.Keys)
            {
                var httpResponseMessage = new HttpResponseMessage {Content = new StringContent(urlsDict[url])};
                ResponseHandler.AddFakeResponse(url, httpResponseMessage);
            }
        }
    }

    internal class FakeHttpClientFactory : TestingBaseClass
    {
        public FakeHttpClient Client { get; }

        public FakeHttpClientFactory()
        {
            Client = new FakeHttpClient();

            Client.For("http://localhost:5001/api/unittest/article/non-existent-url")
                .Return(StockportWebapp.Http.HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/api/unittest/article/this-is-a-redirect-from")
                .Return(StockportWebapp.Http.HttpResponse.Failure(404, "does not exist"));
            Client.For("http://localhost:5001/api/healthystockport/start-page/start-page")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.StartPage.json")));
            Client.For("http://localhost:5001/api/healthystockport/topic/test-topic")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithAlerts.json")));
            Client.For("http://localhost:5001/api/healthystockport/article/physical-activity")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Article.json")));
            Client.For("http://localhost:5001/api/healthystockport/homepage")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.HomepageHealthyStockport.json")));
            Client.For("http://localhost:5001/api/healthystockport/news/latest/2")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.NewsListing.json")));
            Client.For("http://localhost:5001/api/healthystockport/events/latest/2")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventListing.json")));
            Client.For("http://localhost:5001/api/healthystockport/events/latest/2?featured=true")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventListingFeatured.json")));
            Client.For("http://localhost:5001/api/healthystockport/news")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Newsroom.json")));
            Client.For("http://localhost:5001/api/healthystockport/profile/test-profile")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Profile.json")));
            Client.For("http://localhost:5001/api/healthystockport/article/contact-us")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.ContactUsArticle.json")));
            Client.For("http://localhost:5001/api/healthystockport/article/about")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.StandaloneArticleWithProfile.json")));
            Client.For("http://localhost:5001/api/redirects")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Redirects.json")));
            Client.For("http://localhost:5001/api/stockportgov/homepage")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.HomepageStockportGov.json")));
            Client.For("http://localhost:5001/api/stockportgov/topic/test-topic")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithAlerts.json")));
            Client.For("http://localhost:5001/api/stockportgov/article/physical-activity")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Article.json")));
            Client.For("http://localhost:5001/api/stockportgov/start-page/start-page")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.StartPage.json")));
            Client.For("http://localhost:5001/api/stockportgov/news")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Newsroom.json")));
            Client.For("http://localhost:5001/api/stockportgov/news/latest/2")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.NewsListing.json")));
            Client.For("http://localhost:5001/api/stockportgov/news/latest/7")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.NewsListing.json")));
            Client.For("http://localhost:5001/api/stockportgov/news/test")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.News.json")));
            Client.For("http://localhost:5001/api/stockportgov/events/latest/2")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventListing.json")));
            Client.For("http://localhost:5001/api/stockportgov/events/latest/2?featured=true")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventListingFeatured.json")));
            Client.For("http://localhost:5001/api/stockportgov/profile/test-profile")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Profile.json")));
            Client.For("http://localhost:5001/api/stockportgov/footer")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Footer.json")));
            Client.For("http://localhost:5001/api/healthystockport/footer")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Footer.json")));
            Client.For("http://localhost:5001/api/stockportgov/eventhomepage")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventHomepage.json")));
            Client.For("http://localhost:5001/api/stockportgov/events")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.EventsCalendar.json")));
            Client.For("http://localhost:5001/api/stockportgov/events/event-of-the-century")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Event.json")));
            Client.For("http://localhost:5001/api/stockportgov/atoz/a")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.AtoZ.json")));
            Client.For("http://localhost:5001/api/stockportgov/showcase/a-showcase")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Showcase.json")));
            Client.For("http://localhost:5001/api/stockportgov/group/test-zumba-slug")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Group.json")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.GroupResults.json")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/?location=Stockport")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.GroupResults.json")));
            Client.For("http://localhost:5001/api/stockportgov/groupResults/?latitude=53.40581278523235&longitude=-2.158041000366211")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.GroupResults.json")));
            Client.For("http://localhost:5001/api/stockportgov/groupCategory/")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.GroupStart.json")));
            Client.For("http://localhost:5001/api/healthystockport/ContactUsId/test-email")
                .Return(StockportWebapp.Http.HttpResponse.Successful(200, GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.ContactUsId.json")));
            Client.ForPostAsync("https://www.google.com/recaptcha/api/siteverify")     
                .ReturnPostAsync(new HttpResponseMessage() { Content = new StringContent("{\"success\": true,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}") });
        }
    }
}