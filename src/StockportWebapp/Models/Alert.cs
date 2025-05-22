namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Alert(string title,
                string body,
                string severity,
                DateTime sunriseDate,
                DateTime sunsetDate,
                string slug,
                bool isStatic,
                string imageUrl)
{
    public string Title { get; } = title;
    public string Body { get; } = MarkdownWrapper.ToHtml(body);
    public string Severity { get; } = severity;
    public DateTime SunriseDate { get; } = sunriseDate;
    public DateTime SunsetDate { get; } = sunsetDate;
    public string Slug { get; } = slug;
    public bool IsStatic { get; } = isStatic;
    public string ImageUrl { get; } = imageUrl;
}

[ExcludeFromCodeCoverage]
public static class Severity
{
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Information = "Information";
    public const string Success = "Success";
    public const string Condolence = "Condolence";
}