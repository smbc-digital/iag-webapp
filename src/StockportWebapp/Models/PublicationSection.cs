namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PublicationSection
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public JsonElement Body { get; set; }
    public IEnumerable<Alert> AlertsInline { get; set; }
    public List<InlineQuote> InlineQuotes { get; set; }
    public string LogoAreaTitle { get; set; }
    public List<TrustedLogo> TrustedLogos { get; set; }
}