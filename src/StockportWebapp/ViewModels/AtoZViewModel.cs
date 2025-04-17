namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class AtoZViewModel
{
    public List<AtoZ> Items { get; set; }
    public string CurrentLetter { get; set; }
    public List<Crumb> Breadcrumbs { get; set; }
}