namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PrivacyNotice
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Purpose { get; set; }
    public string TypeOfData { get; set; }
    public string Legislation { get; set; }
    public string Obtained { get; set; }
    public string ExternallyShared { get; set; }
    public string RetentionPeriod { get; set; }
    public bool OutsideEu { get; set; }
    public bool AutomatedDecision { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public Topic ParentTopic { get; set; }
    
    public PrivacyNotice() { }
}