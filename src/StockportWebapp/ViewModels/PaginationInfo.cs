namespace StockportWebapp.ViewModels;

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalEntries { get; set; }
    public int PageSize { get; set; }
}