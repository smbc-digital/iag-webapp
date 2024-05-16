using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class NavCardList
{
    public List<NavCard> Items { get; set; }
    public string ButtonText { get; set; }
}