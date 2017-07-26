using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StockportWebappTests
{
    public abstract class TestingBaseClass
    {
        /// <summary>
        /// Gets the content of a embeded file as a string
        /// </summary>
        /// <param name="file">Resource path e.g. StockportConentApiTests.Unit.Test.json</param>
        /// <returns>String content of file</returns>
        protected string GetStringResponseFromFile(string file)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(f => f.Equals($"{file}", StringComparison.OrdinalIgnoreCase));
            string json;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
    }
}
