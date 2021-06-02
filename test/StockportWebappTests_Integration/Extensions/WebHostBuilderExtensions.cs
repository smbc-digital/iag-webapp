//using Microsoft.AspNetCore.Hosting;
//using Microsoft.CodeAnalysis;
//using Microsoft.Extensions.DependencyInjection;
//using StockportWebapp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace StockportWebappTests_Integration.Extensions
//{
//    internal static class WebHostBuilderExtensions
//    {
//        public static IWebHostBuilder ConfigureTestServices(this IWebHostBuilder builder)
//        {
//            return builder.ConfigureServices(services =>
//            {
//                services.AddMvcCore();
//                services.Configure((Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions options) =>
//                {
//                    var previous = options.CompilationCallback;
//                    options.CompilationCallback = (context) =>
//                    {
//                        previous?.Invoke(context);

//                        var assembly = typeof(Startup).GetTypeInfo().Assembly;

//                        var assemblies = assembly.GetReferencedAssemblies()
//                                                 .Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location))
//                                                 .ToList();
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.Corelib")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Threading.Tasks")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Dynamic.Runtime")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor.Runtime")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Html.Abstractions")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Text.Encodings.Web")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq.Expressions")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Humanizer")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("jose-jwt")).Location));
//                        assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Security.Cryptography.X509Certificates")).Location));

//                        context.Compilation = context.Compilation.AddReferences(assemblies);
//                    };
//                });
//            });
//        }
//    }
//}
