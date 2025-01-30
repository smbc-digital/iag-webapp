﻿namespace StockportWebapp.Utils;

public static class FileHelper
{
    public static string GetFileNameFromPath(IFormFile file) =>
        string.IsNullOrEmpty(file?.FileName)
            ? string.Empty
            : Path.GetFileName(file.FileName.Replace("\\", "/"));
}