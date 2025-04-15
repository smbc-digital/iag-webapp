namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ExternalLinksViewModel
{
    public IEnumerable<ExternalLink> ExternalLinks { get; set; }
    public IEnumerable<SubItem> RelatedContent { get; set; }
}