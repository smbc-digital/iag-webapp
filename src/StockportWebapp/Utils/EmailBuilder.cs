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

            var layout = GetEmailTemplateForLayout();
            var body = GetEmailTemplateForBody(template);

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var tag = $"{{{{ {property.Name.ToLower()} }}}}";
                tag = tag.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
                var value = property.GetValue(details, null) == null ? string.Empty : property.GetValue(details, null);
                value = value.ToString().Replace("\r\n", "<br />");
                body = body.Replace(tag, value.ToString());
            }

            result = layout.Replace("{{ MAIN_BODY }}", body);

            return result;
        }

        public virtual string GetEmailTemplateForLayout()
        {
            return new FileReader().GetStringResponseFromFile("StockportWebapp.Emails.Templates._Layout.html");
        }

        public virtual string GetEmailTemplateForBody(string template)
        {
            return new FileReader().GetStringResponseFromFile($"StockportWebapp.Emails.Templates.{template}.html");
        }
    }
}
