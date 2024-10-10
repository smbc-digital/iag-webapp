namespace StockportWebapp.ViewModels;

public class MapViewModel
{
    [Required]
    public string TagData { get; set; }
    public string Title { get; set; } = string.Empty;

    public MapViewModel(string title, string tagData)
    {
        Title = title;
        TagData = tagData;
    }

    public MapViewModel(string tagData)
    {
        TagData = tagData;
    }
}