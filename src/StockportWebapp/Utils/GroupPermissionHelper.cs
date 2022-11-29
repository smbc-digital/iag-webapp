namespace StockportWebapp.Utils
{
    public static class GroupPermissionHelper
    {
        public static string GetPermisison(string letter)
        {
            switch (letter.ToUpper())
            {
                case "A":
                    return "Administrator";
                case "E":
                    return "Editor";
                default:
                    return string.Empty;
            }
        }
    }
}
