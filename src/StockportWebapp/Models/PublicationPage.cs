using System.Text.Json;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PublicationPage
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public List<PublicationSection> PublicationSections { get; set; }
    public JsonElement Body { get; set; }
}