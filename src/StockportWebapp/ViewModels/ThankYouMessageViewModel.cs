namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ThankYouMessageViewModel
{
    public string ReturnUrl { get; set; } = string.Empty;
    public string ButtonText { get; set; } = "Return to previous page";
}