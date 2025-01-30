namespace StockportWebapp.Utils;

public static class StatusColourHelper
{
    public static string GetStatusColour(string status) =>
        status switch
        {
            "Published" => "green",
            "Archived" => "red",
            _ => "green",
        };
}
