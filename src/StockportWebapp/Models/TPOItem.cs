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

	[JsonPropertyName("date_Made")]
	public string Date_Made { get; set; }

	[JsonPropertyName("date_Confirmed")]
	public string Date_Confirmed { get; set; }

	[JsonPropertyName("date_Revoked")]
	public string Date_Revoked { get; set; }

	[JsonPropertyName("number_of_trees")]
	public string Number_of_trees { get; set; }

	[JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lng")]
    public string Lng { get; set; }

	[JsonPropertyName("documents")]
	public List<Document> Documents { get; set; }
}