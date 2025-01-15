namespace StockportWebapp.Utils;

public static class GroupPermissionHelper
{
    public static string GetPermisison(string letter) =>
        letter.ToUpper() switch
        {
            "A" => "Administrator",
            "E" => "Editor",
            _ => string.Empty,
        };
}