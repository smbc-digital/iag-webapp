using System.Text.Json;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PublicationTemplate
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public string Subtitle { get; set; }
    public DateTime DatePublished { get; set; }
    public DateTime LastUpdated { get; set; }
    public MediaAsset HeroImage { get; set; }
    public List<PublicationPage> PublicationPages { get; set; }
}