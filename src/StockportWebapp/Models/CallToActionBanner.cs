using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class CallToActionBanner : Banner
{
    public string Image { get; set; }
    public string ButtonText { get; set; }
    public string AltText { get; set; }
}