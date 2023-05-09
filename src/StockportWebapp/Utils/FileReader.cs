namespace StockportWebapp.Utils;

public class FileReader
{
    public string GetStringResponseFromFile(string file)
    {
        var assembly = this.GetType().GetTypeInfo().Assembly;
        var resources = assembly.GetManifestResourceNames();
        var resourceName = resources.FirstOrDefault(f => f.Equals($"{file}", StringComparison.OrdinalIgnoreCase));
        string json;
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            json = reader.ReadToEnd();
        }
        return json;
    }
}
