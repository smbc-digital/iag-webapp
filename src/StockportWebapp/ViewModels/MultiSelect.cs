namespace StockportWebapp.ViewModels;

public class MultiSelect
{
    public int Limit { get; set; }

    public string Label { get; set; }

    public string ValueControlId { get; set; }

    public string InputName { get; set; }

    public List<string> AvailableValues { get; set; }
}
