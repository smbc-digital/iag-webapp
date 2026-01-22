namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PublicationTemplate
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public string Subtitle { get; set; }
    public string Summary { get; set; }
    public MediaAsset HeaderImage { get; set; }
    public DateTime DatePublished { get; set; }
    public DateTime ReviewDate { get; set; }
    public string PublicationTheme { get; set; }
    public List<PublicationPage> PublicationPages { get; set; }
    public string LogoAreaTitle { get; set; }
    public List<TrustedLogo> TrustedLogos { get; set; }
}