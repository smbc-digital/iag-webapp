using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;

namespace StockportWebapp.Utils
{
    public abstract class EmailBuilder
    {
        public string GenerateEmailBodyFromHtml<T>(T details)
        {
            var result = string.Empty;

            var template = typeof(T).Name;

            var layout = File.ReadAllText($"emails/templates/_layout.html");
            var body = File.ReadAllText($"emails/templates/{template}.html");

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var tag = $"{{{{ {property.Name.ToLower()} }}}}";
                tag = tag.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
                var value = property.GetValue(details, null) == null ? string.Empty : property.GetValue(details, null);
                body = body.Replace(tag, value.ToString());
            }

            result = layout.Replace("{{ MAIN_BODY }}", body);

            return result;
        }
    }
}
