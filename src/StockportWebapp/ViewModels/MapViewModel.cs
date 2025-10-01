namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class MapViewModel(string tagData)
{
    [Required]
    public string TagData { get; set; } = tagData;
    public string Title { get; set; } = string.Empty;
}