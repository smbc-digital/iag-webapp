namespace StockportWebapp.Models;

public class TPOItem
{
    [JsonPropertyName("ogc_fid")]
    public string OgcFid { get; set; }

    [JsonPropertyName("tpo_number")]
    public string Tpo_number { get; set; }

    [JsonPropertyName("tpo_name")]
    public string Tpo_name { get; set; }

    [JsonPropertyName("tree_species")]
    public string Tree_species { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lng")]
    public string Lng { get; set; }

	[JsonPropertyName("documents")]
	public List<Document> Documents { get; set; }
}