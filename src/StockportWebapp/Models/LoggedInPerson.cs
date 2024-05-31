namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]

public class LoggedInPerson
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string RawCookie { get; set; }
}