using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
