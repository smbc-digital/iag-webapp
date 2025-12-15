namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class LeafletMapViewModel()
{
    [Required]
	public string MapName { get; set; } = string.Empty;
	public string Lat { get; set; } = string.Empty;
	public string Lng { get; set; } = string.Empty;
}