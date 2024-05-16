using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class CheckBoxItem
{
    public string Name { get; set; }

    public bool IsSelected { get; set; }
}
