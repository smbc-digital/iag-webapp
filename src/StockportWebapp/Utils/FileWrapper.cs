namespace StockportWebapp.Utils;

public interface IFileWrapper
{
    bool Exists(string path);
    string[] ReadAllLines(string path);
}

[ExcludeFromCodeCoverage]
public class FileWrapper : IFileWrapper
{
    public bool Exists(string path) =>
        File.Exists(path);

    public string[] ReadAllLines(string path) =>
        File.ReadAllLines(path);
}