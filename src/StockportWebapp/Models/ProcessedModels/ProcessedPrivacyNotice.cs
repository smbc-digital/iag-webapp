namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedPrivacyNotice(string slug,
                                    string title,
                                    string category,
                                    string purpose,
                                    string typeOfData,
                                    string legislation,
                                    string obtained,
                                    string externallyShared,
                                    string retentionPeriod,
                                    bool outsideEu,
                                    bool automatedDecision,
                                    IEnumerable<Crumb> breadcrumbs,
                                    Topic parentTopic) : IProcessedContentType
{
    public string Slug { get; set; } = slug;
    public string Title { get; set; } = title;
    public string Category { get; set; } = category;
    public string Purpose { get; set; } = purpose;
    public string TypeOfData { get; set; } = typeOfData;
    public string Legislation { get; set; } = legislation;
    public string Obtained { get; set; } = obtained;
    public string ExternallyShared { get; set; } = externallyShared;
    public string RetentionPeriod { get; set; } = retentionPeriod;
    public bool OutsideEu { get; set; } = outsideEu;
    public bool AutomatedDecision { get; set; } = automatedDecision;
    public IEnumerable<Crumb> Breadcrumbs { get; set; } = breadcrumbs;
    public Topic ParentTopic { get; set; } = parentTopic;
    public readonly string NavigationLink = TypeRoutes.GetUrlFor("privacy-notice", slug);
}