using System.IO;

namespace StockportWebapp.Utils
{
    public interface IFileWrapper
    {
        bool Exists(string path);
        string[] ReadAllLines(string path);
    }

    public class FileWrapper : IFileWrapper
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}