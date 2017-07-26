using StockportWebapp.Utils;
using System;

namespace StockportWebapp.Models
{
    public class Alert
    {
        public string Title { get; }
        public string SubHeading { get; }
        public string Body { get; }
        public string Severity { get; }
        public DateTime SunriseDate { get; }
        public DateTime SunsetDate { get; }

        public Alert(string title, string subHeading, string body, string severity, DateTime sunriseDate, DateTime sunsetDate)
        {
            Title = title;
            SubHeading = subHeading;
            Body = MarkdownWrapper.ToHtml(body);
            Severity = severity;
            SunriseDate = sunriseDate;
            SunsetDate = sunsetDate;
        }
    }

    public class NullAlert : Alert
    {
        public NullAlert() : base(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue) { }
    }

    public static class Severity
    {
        public const string Warning = "Warning";
        public const string Error = "Error";
        public const string Information = "Information";
    }
}
