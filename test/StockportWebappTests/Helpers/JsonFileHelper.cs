using System.IO;

namespace StockportWebappTests.Helpers
{
    public static class JsonFileHelper
    {
        public static string GetStringResponseFromFile(string filePath)
        {
            return File.ReadAllText($"../../../Unit/MockResponses/{filePath}");
        }
    }
}