using System.IO;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Utils
{
    public static class FileHelper
    {
        public static string GetFileNameFromPath(IFormFile file)
        {
            return string.IsNullOrEmpty(file?.FileName) ? string.Empty : Path.GetFileName(file.FileName.Replace("\\", "/"));
        }
    }
}
