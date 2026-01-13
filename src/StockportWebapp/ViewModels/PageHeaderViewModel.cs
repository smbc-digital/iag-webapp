namespace StockportWebapp.ViewModels;

public class PageHeaderViewModel
{
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? HeaderImageUrl { get; set; }
    public ContentBlock? HeaderHighlight { get; set; }
    public string? HeaderHighlightType { get; set; }
}