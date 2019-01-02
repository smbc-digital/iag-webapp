using System.IO;

namespace StockportWebappTests_Integration.Helpers
{
    public static class JsonFileHelper
    {
        public static string GetStringResponseFromFile(string filePath)
        {
            return File.ReadAllText($"../../../MockResponses/{filePath}");
        }
    }
}