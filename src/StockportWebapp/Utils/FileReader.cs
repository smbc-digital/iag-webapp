﻿namespace StockportWebapp.Utils;

[ExcludeFromCodeCoverage]
public class FileReader
{
    public string GetStringResponseFromFile(string file)
    {
        Assembly assembly = this.GetType().GetTypeInfo().Assembly;
        string[] resources = assembly.GetManifestResourceNames();
        string resourceName = resources.FirstOrDefault(f => f.Equals($"{file}", StringComparison.OrdinalIgnoreCase));
        string json;

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))

        using (StreamReader reader = new StreamReader(stream))
        {
            json = reader.ReadToEnd();
        }

        return json;
    }
}