using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class RedisValueData
{
    public string Key { get; set; }
    public string Expiry { get; set; }
    public int NumberOfItems { get; set; }
}
