using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Alert
    {
        public string Title { get; }
        public string SubHeading { get; }
        public string Body { get; }
        public string Severity { get; }

        public Alert(string title, string subHeading, string body, string severity)
        {
            Title = title;
            SubHeading = subHeading;
            Body = MarkdownWrapper.ToHtml(body);
            Severity = severity;
        }
    }

    public class NullAlert : Alert
    {
        public NullAlert() : base(string.Empty, string.Empty, string.Empty, string.Empty) { }
    }

    public static class Severity
    {
        public const string Warning = "Warning";
        public const string Error = "Error";
        public const string Information = "Information";
    }
}
