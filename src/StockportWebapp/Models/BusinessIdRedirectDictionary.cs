using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class BusinessIdRedirectDictionary : Dictionary<string, RedirectDictionary> { }

[ExcludeFromCodeCoverage]
public class RedirectDictionary : Dictionary<string, string>
{
    public RedirectDictionary() : base(StringComparer.CurrentCultureIgnoreCase) { }
}