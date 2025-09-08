using System.Text.Json.Serialization;

namespace StockportWebapp.Models;

public class ShedItem
{
    [JsonPropertyName("ogc_fid")]
    public string OgcFid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public string Slug => Name.ToLower()?.Replace(" ", "-");

    [JsonPropertyName("address")]
    public string Location { get; set; }

    [JsonPropertyName("grade")]
    public string Grade { get; set; }

    [JsonPropertyName("ward_name")]
    public string WardName { get; set; }

    [JsonPropertyName("he_ref")]
    public string HeRef { get; set; }

    [JsonPropertyName("images")]
    public List<string> Images { get; set; }

    [JsonPropertyName("date_listed")]
    public string DateListed { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("uniform_ref")]
    public string UniformRef { get; set; }

    [JsonPropertyName("ward_code")]
    public string WardCode { get; set; }

    [JsonPropertyName("easting")]
    public string Easting { get; set; }

    [JsonPropertyName("northing")]
    public string Northing { get; set; }

    [JsonPropertyName("alt_ref")]
    public string AltRef { get; set; }

    public ShedItem()
    { }
}