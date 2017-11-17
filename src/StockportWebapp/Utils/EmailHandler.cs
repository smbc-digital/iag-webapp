using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using StockportWebapp.Entities;

namespace StockportWebapp.Utils
{
    public interface IEmailHandler
    {
        void SendEmail(EmailEntity email);
        string GenerateEmailBodyFromHtml<T>(T details, string templateName = null);
    }

    public class EmailHandler : IEmailHandler
    {
        public EmailHandler()
        {
            
        }

        public void SendEmail(EmailEntity email)
        {
            throw new NotImplementedException();
        }

        public string GenerateEmailBodyFromHtml<T>(T details, string templateName = null)
        {
            var template = typeof(T).Name;

            var layout = GetEmailTemplateForLayout();
            var body = GetEmailTemplateForBody(template);

            foreach (var property in typeof(T).GetProperties())
            {
                var tag = $"{{{{ {property.Name.ToLower()} }}}}";
                tag = tag.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
                var value = property.GetValue(details, null) == null ? string.Empty : property.GetValue(details, null);

                if (property.PropertyType == typeof(List<string>))
                {
                    if (value is List<string> items) value = string.Join(", ", items.ToArray()).Trim().TrimEnd(',');
                }
                else
                {
                    value = value.ToString().Replace("\r\n", "<br />");
                }

                body = body.Replace(tag, value.ToString());
            }

            var result = layout.Replace("{{ MAIN_BODY }}", body);

            return result;
        }

        private static string GetEmailTemplateForLayout()
        {
            return new FileReader().GetStringResponseFromFile("StockportWebapp.Emails.Templates._Layout.html");
        }

        private static string GetEmailTemplateForBody(string template)
        {
            return new FileReader().GetStringResponseFromFile($"StockportWebapp.Emails.Templates.{template}.html");
        }
    }
}
