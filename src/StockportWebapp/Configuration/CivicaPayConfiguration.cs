namespace StockportWebapp.Configuration;

public class CivicaPayConfiguration
{
    public const string ConfigValue = "CivicaPayConfiguration";
    public string CallingAppIdentifier { get; set; }
    public string CustomerID { get; set; }
    public string ApiPassword { get; set; }
}