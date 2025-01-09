namespace StockportWebapp.Utils;

public class EventFilter(string dateFrom, string dateTo, string dateRange)
{
    public string DateFrom { get; set; } = dateFrom;
    public string DateTo { get; set; } = dateTo;
    public string DateRange { get; set; } = dateRange;
}