namespace StockportWebapp.Models;

public class Alert
{
    public string Title { get; }
    public string SubHeading { get; }
    public string Body { get; }
    public string Severity { get; }
    public DateTime SunriseDate { get; }
    public DateTime SunsetDate { get; }
    public string Slug { get; }
    public bool IsStatic { get; }

    public Alert(string title, string subHeading, string body, string severity, DateTime sunriseDate, DateTime sunsetDate, string slug, bool isStatic)
    {
        Title = title;
        SubHeading = subHeading;
        Body = MarkdownWrapper.ToHtml(body);
        Severity = severity;
        SunriseDate = sunriseDate;
        SunsetDate = sunsetDate;
        Slug = slug;
        IsStatic = isStatic;
    }
}

public class NullAlert : Alert
{
    public NullAlert() : base(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, String.Empty, false) { }
}

public static class Severity
{
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Information = "Information";
    public const string Success = "Success";
}
