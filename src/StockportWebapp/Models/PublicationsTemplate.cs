using System.Text.Json;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class PublicationsTemplate
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string MetaDescription { get; set; }

    public JsonElement Body { get; set; }
    // public object Body { get; set; }
}